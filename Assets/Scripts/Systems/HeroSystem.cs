using System;
using System.Collections;
using System.Collections.Generic;
using Action_System;
using UnityEngine;

public class HeroSystem : Singleton<HeroSystem>
{
    [SerializeField] private List<HeroData> HeroDataList;
    [SerializeField] public HeroView HeroView;
    
    public int MaxMana { get; private set; }
    public int Attack { get; private set; }
    public int Defense { get; private set; }
    public int CardDrawAmount { get; private set; }
    private List<PerkData> InitialPerkDatas = new();

    private void OnEnable()
    {
        ActionSystem.SubscribeReaction<EnemyTurnGA>(EnemyTurnPreReaction, ReactionTiming.PRE);
        ActionSystem.SubscribeReaction<EnemyTurnGA>(EnemyTurnPostReaction, ReactionTiming.POST);
        ActionSystem.AttachPerformer<HeroAttackedWhileStealthGA>(HeroAttackedWhileStealthPerformer);
    }

    private void OnDisable()
    {
        ActionSystem.DetachPerformer<HeroAttackedWhileStealthGA>();
        ActionSystem.UnsubscribeReaction<EnemyTurnGA>(EnemyTurnPreReaction, ReactionTiming.PRE);
        ActionSystem.UnsubscribeReaction<EnemyTurnGA>(EnemyTurnPostReaction, ReactionTiming.POST);            
    }

    private void Start()
    {
        if (SceneLoadSystem.Instance.GetCurrentSceneName() == "Level1")
        {
            HeroView.SetupHeroHealth = null;
        }
    }

    private void OnDestroy()
    {
        HeroView.SetupHeroHealth = HeroView.CurrentHealth;
    }
    
    // Publics
    public void Setup()
    {
        var heroData = HeroDataList.Find((heroData) => heroData.HeroEnum == SceneLoadSystem.Instance.heroEnum);
        
        MaxMana = heroData.MaxMana;
        Attack = heroData.Attack;
        Defense = heroData.Defense;
        CardDrawAmount = heroData.CardDrawAmount;
        InitialPerkDatas = heroData.InitialPerkDatas;
        
        HeroView.Setup(heroData);
        CardSystem.Instance.Setup(heroData.Deck);
    }

    public List<PerkData> GetInitialPerkData()
    {
        return InitialPerkDatas;
    }
    
    // Performers
    private IEnumerator HeroAttackedWhileStealthPerformer(HeroAttackedWhileStealthGA heroAttackedWhileStealthGa)
    {
        yield return null;
    }
    
    // Reactions
    private void EnemyTurnPreReaction(EnemyTurnGA enemyTurnGa)
    {
        TurnSystem.Instance.NextTurn();
        DiscardAllCardsGA discardAllCardsGa = new();
        ActionSystem.Instance.AddReaction(discardAllCardsGa);
        
        int vulnerableStacks = HeroView.GetStatusEffectStacks(StatusEffectType.VULNERABLE);
        if (vulnerableStacks > 0)
        {
            ApplyVulnerableGA applyVulnerableGa = new(vulnerableStacks, HeroView);
            ActionSystem.Instance.AddReaction(applyVulnerableGa);
        }
    }
    private void EnemyTurnPostReaction(EnemyTurnGA enemyTurnGa)
    {
        TurnSystem.Instance.NextTurn();
        
        int burnStacks = HeroView.GetStatusEffectStacks(StatusEffectType.BURN);
        if (burnStacks > 0)
        {
            ApplyBurnGA applyBurnGa = new(burnStacks, HeroView);
            ActionSystem.Instance.AddReaction(applyBurnGa);
        }
        
        int stealthStacks = HeroView.GetStatusEffectStacks(StatusEffectType.STEALTH);
        if (stealthStacks > 0)
        {
            ApplyStealthGA applyStealthGa = new(stealthStacks, HeroView);
            ActionSystem.Instance.AddReaction(applyStealthGa);
        }
        
        int damageMultiplierStacks = HeroView.GetStatusEffectStacks(StatusEffectType.DAMAGE_MULTIPLIER);
        if (damageMultiplierStacks > 0)
        {
            ApplyDamageMultiplierGA applyDamageMultiplierGa = new(damageMultiplierStacks, HeroView);
            ActionSystem.Instance.AddReaction(applyDamageMultiplierGa);
        }

        DrawCardsGA drawCardsGa = new(CardDrawAmount);
        ActionSystem.Instance.AddReaction(drawCardsGa);   
    }
}
