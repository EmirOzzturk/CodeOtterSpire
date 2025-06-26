using System;
using System.Collections;
using Action_System;
using UnityEngine;

public class ManaSystem : Singleton<ManaSystem>
{
    [SerializeField] private ManaUI manaUI;
    private const int MAX_MANA = 3;
    private int currentMana =  MAX_MANA;

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
        return currentMana >= amount;
    }

    public void ResetManaText()
    {
        if (manaUI == null) return;
        manaUI.UpdateManaText(MAX_MANA);
    }
    
    // Performers
    private IEnumerator SpendManaPerformer(SpendManaGA spendManaGa)
    {   
        currentMana -= spendManaGa.Amount;
        manaUI.UpdateManaText(currentMana);
        yield return null;
    }
    private IEnumerator RefillManaPerformer(RefillManaGA refillManaGa)
    {
        currentMana = MAX_MANA;
        manaUI.UpdateManaText(currentMana);
        yield return null;
    }
    
    // Reactions
    private void EnemyTurnPostReactions(EnemyTurnGA enemyTurnGa)
    {
        RefillManaGA refillManaGA = new();
        ActionSystem.Instance.AddReaction(refillManaGA);
    }
}
