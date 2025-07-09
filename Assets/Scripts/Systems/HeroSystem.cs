using Action_System;
using Systems.Card_System;
using UnityEngine;

public class HeroSystem : Singleton<HeroSystem>
{
    [field: SerializeField] public HeroView HeroView { get; private set; }

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
    public void Setup(HeroData heroData)
    {
        HeroView.Setup(heroData);
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
        
        for (int i = 0; i < CardSystem.Instance.GetDrawCardsCount(); i++)
        {
            DrawCardsGA drawCardsGa = new(1);
            ActionSystem.Instance.AddReaction(drawCardsGa);   
        }
    }
}
