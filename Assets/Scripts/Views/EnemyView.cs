using System.Collections;
using System.Collections.Generic;
using Action_System;
using Unity.VisualScripting;
using UnityEngine;

/// <summary>
/// Görsel ve dövüşsel düşman mantığı.
/// </summary>
public class EnemyView : CombatantView
{
    [SerializeField] private IntentUI intentUI;

    public int AttackPower  { get; private set; }
    public int CurrentIndexForIntentEffect  { get; private set; }
    public List<IntentType> IntentTypes { get; private set; }
    public List<List<EnemyEffect>> EnemyEffects { get; private set; }

    #region Setup -------------------------------------------------------------

    public void Setup(EnemyData data)
    {
        CurrentIndexForIntentEffect = 0;
        
        IntentTypes = data.GetAllIntents();
        EnemyEffects = data.GetEffectGroups();
        AttackPower = data.AttackPower;

        PerformInitialEffects(data);
        
        SetupBase(data.Health, data.Image);
        UpdateIntentInitialUI();

        TurnSystem.Instance.OnTurnChanged += NextIntent;
    }

    private void UpdateIntentInitialUI()
    {
        if (intentUI == null) return;

        int value = IntentTypes[CurrentIndexForIntentEffect] == IntentType.ATTACKER
            ? AttackPower
            : (EnemyEffects.Count > 0 ? EnemyEffects[CurrentIndexForIntentEffect][0].GetEffectValue() : 0);

        intentUI.UpdateIntentUI(IntentTypes[CurrentIndexForIntentEffect], value);
    }

    #endregion ----------------------------------------------------------------

    #region Combat ------------------------------------------------------------

    public void AddAttackPower(int amount)
    {
        AttackPower += amount;
        intentUI?.UpdateIntentValue(AttackPower); // Toplamı göster
    }

    public void UpdateIntentType(IntentType type)
    {
        IntentTypes[CurrentIndexForIntentEffect] = type;
        intentUI?.UpdateIntentType(type);
    }

    public void NextIntent(TurnSystem.Turn turn)
    {
        if (turn == TurnSystem.Turn.Player)
        {
            CurrentIndexForIntentEffect = TurnSystem.Instance.GetPostTurnRemainder(IntentTypes.Count);
            UpdateIntentInitialUI();
        }
    }

    public IEnumerator PerformInitialEffects(EnemyData data)
    {
        // Liste yoksa ya da eleman içermiyorsa hiçbir şey yapma
        if (data.InitialEffects is not { Count: > 0 })
            yield break;

        // Buraya gelmişsek en az bir efekt var
        var heroTarget = new List<CombatantView> { HeroSystem.Instance.HeroView };

        foreach (var effect in data.InitialEffects)
        {
            ActionSystem.Instance.AddReaction(
                new PerformEffectGA(effect, heroTarget, this)
            );
        }

        yield return null;
    }
    #endregion ----------------------------------------------------------------
    
    #region Getters ----------------------------------------------------------------

    public IntentType GetCurrentIntentType()
    {
        return IntentTypes[CurrentIndexForIntentEffect];
    }

    public List<EnemyEffect> GetCurrentEffects()
    {
        return EnemyEffects[CurrentIndexForIntentEffect];
    }
    
    #endregion ----------------------------------------------------------------

}