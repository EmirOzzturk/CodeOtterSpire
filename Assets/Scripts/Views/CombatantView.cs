using System;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;

public class CombatantView : MonoBehaviour
{
    [SerializeField] private TMP_Text healthText;
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private StatusEffectsUI statusEffectsUI;
    
    public Action<StatusEffectType, CombatantView> AddStatusEffectEvent;
    public Action<StatusEffectType, CombatantView> RemoveStatusEffectEvent;
    
    public int MaxHealth { get; private set; }
    public int CurrentHealth { get; private set; }
    public int Targetable { get; set; } = 1;
    private Dictionary<StatusEffectType, int> statusEffects = new();

    protected void SetupBase(int health, Sprite image, int? setupHealth)
    {
        MaxHealth = CurrentHealth = health;
        if (setupHealth.HasValue) CurrentHealth = setupHealth.Value;
        UpdateHealthText();

        spriteRenderer.sprite = image;

        AddStatusEffectEvent += StatusEffectSystem.Instance.OnAddStatusEffect;
        RemoveStatusEffectEvent += StatusEffectSystem.Instance.OnRemoveStatusEffect;
    }

    public void Damage(int damage)
    {
        if (GetStatusEffectStacks(StatusEffectType.VULNERABLE) != 0)
        {
            damage = Mathf.CeilToInt(damage * 1.5f);   // yukarı yuvarla, küsurat kaybolmaz
        }
        
        int persistantArmor = GetStatusEffectStacks(StatusEffectType.PERSISTENT_ARMOR);
        damage -= persistantArmor;
        
        int armor      = GetStatusEffectStacks(StatusEffectType.ARMOR);
        int absorbed   = Mathf.Min(armor, damage);           // Zırhın emdiği hasar
        int netDamage  = damage - absorbed;                  // Sağlığa gidecek hasar

        RemoveStatusEffect(StatusEffectType.ARMOR, absorbed);
        CurrentHealth = Mathf.Max(CurrentHealth - netDamage, 0);

        UpdateHealthText();
        transform.DOShakePosition(0.2f, 0.3f);
    }

    public void Heal(int heal)
    {
        if (CurrentHealth + heal <= MaxHealth)
        {
            CurrentHealth += heal;
        }
        else
        {
            CurrentHealth = MaxHealth;
        }
        
        UpdateHealthText();
    }

    public void AddStatusEffect(StatusEffectType statusEffectType, int stackCount)
    {
        if (statusEffects.ContainsKey(statusEffectType))
        {
            statusEffects[statusEffectType] += stackCount;
        }
        else
        {
            statusEffects.Add(statusEffectType, stackCount);
            AddStatusEffectEvent.Invoke(statusEffectType, this);
        }
        statusEffectsUI.UpdateStatusEffectUI(statusEffectType, GetStatusEffectStacks(statusEffectType));
    }

    public void RemoveStatusEffect(StatusEffectType statusEffectType, int stackCount)
    {
        if (statusEffects.ContainsKey(statusEffectType))
        {
            statusEffects[statusEffectType] -= stackCount;
            if (statusEffects[statusEffectType] <= 0)
            {
                RemoveStatusEffectEvent.Invoke(statusEffectType, this);
                statusEffects.Remove(statusEffectType);
            }
        }
        statusEffectsUI.UpdateStatusEffectUI(statusEffectType, GetStatusEffectStacks(statusEffectType));
    }

    public int GetStatusEffectStacks(StatusEffectType statusEffectType)
    {
        return statusEffects.ContainsKey(statusEffectType) ? statusEffects[statusEffectType] : 0;
    }
        
    private void UpdateHealthText()
    {
        healthText.text = "Health: " + CurrentHealth.ToString();
    }
    
}
