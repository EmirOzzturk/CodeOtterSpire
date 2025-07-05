using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;

public class CombatantView : MonoBehaviour
{
    [SerializeField] private TMP_Text healthText;
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private StatusEffectsUI statusEffectsUI;
    
    public int MaxHealth { get; private set; }
    public int CurrentHealth { get; private set; }
    private Dictionary<StatusEffectType, int> statusEffects = new();

    protected void SetupBase(int health, Sprite image)
    {
        MaxHealth = CurrentHealth = health;
        spriteRenderer.sprite = image;
        UpdateHealthText();
    }

    public void Damage(int damage)
    {
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
