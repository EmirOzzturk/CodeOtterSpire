using System.Collections;
using Action_System;
using UnityEngine;
using UnityEngine.Pool;        // ListPool kullandıysanız

/// <summary>
/// Oyuncunun mana yönetimi. <br/>
/// • Tur başında mana yeniler (+ bonus)  <br/>
/// • Kart oynarken mana harcar            <br/>
/// • Geçici bonus eklenip bir sonraki tur sıfırlanır
/// </summary>
public class ManaSystem : Singleton<ManaSystem>
{
    [Header("UI")]
    [SerializeField] private ManaUI manaUI;

    // ———⮕ Properties ——————————————————————————————————————————
    private   int MaxMana          => HeroSystem.Instance.MaxMana;
    public    int CurrentMana      { get; private set; }
    private   int nextTurnBonus;          // “+X mana” buff’ları burada tutulur

    // ———⮕ Life-cycle ——————————————————————————————————————————
    private void OnEnable()
    {
        ActionSystem.AttachPerformer<SpendManaGA>(SpendManaPerformer);
        ActionSystem.AttachPerformer<RefillManaGA>(RefillManaPerformer);
        ActionSystem.AttachPerformer<AddTempManaGA>(AddTempManaPerformer);

        ActionSystem.SubscribeReaction<EnemyTurnGA>(OnEnemyTurnFinished, ReactionTiming.POST);
    }

    private void OnDisable()
    {
        ActionSystem.UnsubscribeReaction<EnemyTurnGA>(OnEnemyTurnFinished, ReactionTiming.POST);

        ActionSystem.DetachPerformer<AddTempManaGA>();
        ActionSystem.DetachPerformer<RefillManaGA>();
        ActionSystem.DetachPerformer<SpendManaGA>();
    }

    // ———⮕ Queries ——————————————————————————————————————————
    public bool HasEnoughMana(int amount)       => CurrentMana >= amount;

    // ———⮕ API ——————————————————————————————————————————————
    /// <summary>Haricî sistemler çağırabilir (sahne restart, save-load vb.)</summary>
    public void FullRefillNow()
    {
        CurrentMana = MaxMana;
        manaUI?.UpdateManaText(CurrentMana);
    }

    /// <summary> Sonraki tur için geçici mana ekle (stack’lenir). </summary>
    public void AddTemporaryBonus(int amount)
    {
        if (amount <= 0) return;
        nextTurnBonus += amount;

        // “+X mana buff ikonu” göstermek istiyorsanız burada UI tetikleyin
    }

    // ———⮕ Performers ——————————————————————————————————————————
    private IEnumerator SpendManaPerformer(SpendManaGA ga)
    {
        CurrentMana = Mathf.Max(0, CurrentMana - ga.Amount);
        manaUI.UpdateManaText(CurrentMana);
        yield return null;
    }

    private IEnumerator RefillManaPerformer(RefillManaGA ga)
    {
        // Bonus + normal mana
        CurrentMana = MaxMana + nextTurnBonus;
        nextTurnBonus = 0;                    // Bonus tek kullanım → sıfırla
        manaUI.UpdateManaText(CurrentMana);
        yield return null;
    }

    private IEnumerator AddTempManaPerformer(AddTempManaGA ga)
    {
        AddTemporaryBonus(ga.TempManaAmount);
        yield return null;
    }

    // ———⮕ Reactions ——————————————————————————————————————————
    /// <summary> Düşman turu bittiğinde otomatik mana doldur. </summary>
    private void OnEnemyTurnFinished(EnemyTurnGA ga)
    {
        ActionSystem.Instance.AddReaction(new RefillManaGA());
    }
}
