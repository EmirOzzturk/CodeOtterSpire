using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Görsel ve dövüşsel düşman mantığı.
/// </summary>
public class EnemyView : CombatantView
{
    [SerializeField] private IntentUI intentUI;

    public int AttackPower  { get; private set; }
    public IntentType IntentType { get; private set; }

    // Dışarıdan değiştirilemesin diye IReadOnlyList
    public IReadOnlyList<Effect> EnemyEffects { get; private set; } = new List<Effect>();

    #region Setup -------------------------------------------------------------

    public void Setup(EnemyData data)
    {
        // Listeyi kopyalamak, EnemyData'nın mutasyonundan korunur
        EnemyEffects = new List<Effect>(data.EnemyEffect ?? new List<Effect>());

        AttackPower = data.AttackPower;
        IntentType  = data.IntentType;

        UpdateIntentInitialUI();

        SetupBase(data.Health, data.Image);
    }

    private void UpdateIntentInitialUI()
    {
        if (intentUI == null) return;

        int value = IntentType == IntentType.ATTACKER
            ? AttackPower
            : (EnemyEffects.Count > 0 ? EnemyEffects[0].GetEffectValue() : 0);

        intentUI.UpdateIntentUI(IntentType, value);
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
        IntentType = type;
        intentUI?.UpdateIntentType(type);
    }

    #endregion ----------------------------------------------------------------
}