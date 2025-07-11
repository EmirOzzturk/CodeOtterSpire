using System;
using System.Collections;
using Action_System;
using UnityEngine;

public class ManaSystem : Singleton<ManaSystem>
{
    [SerializeField] private ManaUI manaUI;
    private int MaxMana => HeroSystem.Instance.MaxMana;
    
    private int currentMana;
    public int CurrentMana
    {
        get => currentMana;
        private set => currentMana = Math.Clamp(value, 0, MaxMana);
    }
    
    private void OnEnable()
    {
        ActionSystem.AttachPerformer<SpendManaGA>(SpendManaPerformer);
        ActionSystem.AttachPerformer<RefillManaGA>(RefillManaPerformer);
        ActionSystem.SubscribeReaction<EnemyTurnGA>(EnemyTurnPostReactions, ReactionTiming.POST);
    }

    private void OnDisable()
    {
        ActionSystem.UnsubscribeReaction<EnemyTurnGA>(EnemyTurnPostReactions,  ReactionTiming.POST);
        ActionSystem.DetachPerformer<RefillManaGA>();
        ActionSystem.DetachPerformer<SpendManaGA>();
    }
    
    // Publics
    public bool HasEnoughMana(int amount)
    {
        return CurrentMana >= amount;
    }

    public void ResetManaText()
    {
        if (manaUI == null) return;
        currentMana = MaxMana;
        manaUI.UpdateManaText(MaxMana);
    }
    
    // Performers
    private IEnumerator SpendManaPerformer(SpendManaGA spendManaGa)
    {   
        CurrentMana -= spendManaGa.Amount;
        manaUI.UpdateManaText(CurrentMana);
        yield return null;
    }
    private IEnumerator RefillManaPerformer(RefillManaGA refillManaGa)
    {
        CurrentMana = MaxMana;
        manaUI.UpdateManaText(CurrentMana);
        yield return null;
    }
    
    // Reactions
    private void EnemyTurnPostReactions(EnemyTurnGA enemyTurnGa)
    {
        RefillManaGA refillManaGA = new();
        ActionSystem.Instance.AddReaction(refillManaGA);
    }
}
