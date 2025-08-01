using System;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using TMPro;
using UnityEngine;

public class CombatantView : MonoBehaviour
{
    [SerializeField] private TMP_Text healthText;
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private StatusEffectsUI statusEffectsUI;
    
    public Action<StatusEffect, CombatantView> AddStatusEffectEvent;
    public Action<StatusEffect, CombatantView> RemoveStatusEffectEvent;
    
    public int MaxHealth { get; private set; }
    public int CurrentHealth { get; private set; }
    public int Targetable { get; set; } = 1;
    private List<StatusEffect> statusEffects = new();

    protected void SetupBase(int health, Sprite image, int? setupHealth)
    {
        MaxHealth = CurrentHealth = health;
        if (setupHealth.HasValue) CurrentHealth = setupHealth.Value;
        UpdateHealthText();

        spriteRenderer.sprite = image;

        AddStatusEffectEvent += StatusEffectSystem.Instance.OnAddStatusEffect;
        RemoveStatusEffectEvent += StatusEffectSystem.Instance.OnRemoveStatusEffect;
    }

    public void DestroyBase()
    {
        AddStatusEffectEvent -= StatusEffectSystem.Instance.OnAddStatusEffect;
        RemoveStatusEffectEvent -= StatusEffectSystem.Instance.OnRemoveStatusEffect;
    }

    public void Damage(int damage)
    {
        // 1) VULNERABLE varsa hasarı %50 artır
        var vulnerable = FindEffect(StatusEffectType.VULNERABLE);
        if (vulnerable?.Stack > 0)
            damage = Mathf.CeilToInt(damage * 1.5f);

        // 2) PERSISTENT_ARMOR sabit indirimi
        damage -= FindEffect(StatusEffectType.PERSISTENT_ARMOR)?.Stack ?? 0;

        // 3) ARMOR etkisi hasarı emer
        var armorEffect = FindEffect(StatusEffectType.ARMOR);
        int armor     = armorEffect?.Stack ?? 0;
        int absorbed  = Mathf.Min(armor, damage);
        int netDamage = Mathf.Max(damage - absorbed, 0);

        // 3a) Armour stack’ini düşür
        if (absorbed > 0 && armorEffect != null)
        {
            armorEffect.ReduceStack(absorbed);          // Stack 0 olursa içeride 0’a sabitlenir
            if (armorEffect.Stack == 0)                 // Tamamen bittiyse listeden çıkar
                statusEffects.Remove(armorEffect);

            statusEffectsUI.UpdateStatusEffectUI(armorEffect);
        }

        // 4) Sağlığı güncelle
        CurrentHealth = Mathf.Max(CurrentHealth - netDamage, 0);
        UpdateHealthText();
        transform.DOShakePosition(0.2f, 0.3f);
    }
    
    public void Heal(int amount)
    {
        CurrentHealth = Mathf.Clamp(CurrentHealth + amount, 0, MaxHealth);
        UpdateHealthText();
    }

    public void AddStatusEffect(StatusEffect statusEffect)
    {
        var aStatusEffect = statusEffects.FirstOrDefault(se => se.StatusEffectType == statusEffect.StatusEffectType);
        
        if (aStatusEffect != null)
        {
            aStatusEffect.Merge(statusEffect);
            statusEffectsUI.UpdateStatusEffectUI(aStatusEffect);
        }
        else
        {
            // Yoksa yeni StatusEffect'i ekle
            statusEffects.Add(statusEffect);
            AddStatusEffectEvent?.Invoke(statusEffect, this);
            statusEffectsUI.UpdateStatusEffectUI(statusEffect);
        }

    }

    public void RemoveStatusEffect(StatusEffect statusEffect, int stackCount)
    {
        var aStatusEffect = statusEffects.FirstOrDefault(se => se.StatusEffectType == statusEffect.StatusEffectType);
        if (aStatusEffect == null) return;

        if (aStatusEffect.ReduceStack(stackCount))
        {
            RemoveStatusEffectEvent?.Invoke(aStatusEffect, this);
            statusEffects.Remove(aStatusEffect);
        }

        statusEffectsUI.UpdateStatusEffectUI(aStatusEffect);
    }

    public int GetStatusEffectStacks(StatusEffectType statusEffectType)
    { 
        var statusEffect = statusEffects.FirstOrDefault(se => se.StatusEffectType == statusEffectType);
        return statusEffect?.Stack ?? 0;
    }
        
    private void UpdateHealthText()
    {
        healthText.text = "Health: " + CurrentHealth.ToString();
    }
    
    private StatusEffect FindEffect(StatusEffectType type) =>
        statusEffects.FirstOrDefault(se => se.StatusEffectType == type);
    
}
