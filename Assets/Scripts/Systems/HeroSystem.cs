using System.Collections.Generic;
using Action_System;
using Systems.Card_System;
using UnityEngine;

public class HeroSystem : Singleton<HeroSystem>
{
    [SerializeField] private HeroEnum HeroEnum;
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
    }

    private void OnDisable()
    {
        ActionSystem.UnsubscribeReaction<EnemyTurnGA>(EnemyTurnPreReaction, ReactionTiming.PRE);
        ActionSystem.UnsubscribeReaction<EnemyTurnGA>(EnemyTurnPostReaction, ReactionTiming.POST);            
    }
    
    // Publics
    public void Setup(HeroEnum hero)
    {
        var heroData = HeroDataList[(int)hero];
        
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
    
    // Reactions
    private void EnemyTurnPreReaction(EnemyTurnGA enemyTurnGa)
    {
        DiscardAllCardsGA discardAllCardsGa = new();
        ActionSystem.Instance.AddReaction(discardAllCardsGa);
    }
    private void EnemyTurnPostReaction(EnemyTurnGA enemyTurnGa)
    {
        int burnStacks = HeroView.GetStatusEffectStacks(StatusEffectType.BURN);
        if (burnStacks > 0)
        {
            ApplyBurnGA applyBurnGa = new(burnStacks, HeroView);
            ActionSystem.Instance.AddReaction(applyBurnGa);
        }
        
        for (int i = 0; i < CardDrawAmount; i++)
        {
            DrawCardsGA drawCardsGa = new(1);
            ActionSystem.Instance.AddReaction(drawCardsGa);   
        }
    }
}
