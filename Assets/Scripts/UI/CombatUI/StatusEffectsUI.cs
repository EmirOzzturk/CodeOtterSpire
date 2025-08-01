using System.Collections.Generic;
using UnityEngine;

public class StatusEffectsUI : MonoBehaviour
{
    [Header("Prefab")]
    [SerializeField] private StatusEffectUI statusEffectUIPrefab;

    // —— Dahili ————————————————————————————————————————————————
    private readonly Dictionary<StatusEffect, StatusEffectUI> _activeUIs   = new();

    /// <summary>
    /// İlgili durum etkisini günceller veya kaldırır.
    /// </summary>
    public void UpdateStatusEffectUI(StatusEffect statusEffect)
    {
        // ⬇ Kaldırılacaksa
        if (statusEffect.Stack <= 0)
        {
            if (_activeUIs.TryGetValue(statusEffect, out var ui))
            {
                _activeUIs.Remove(statusEffect);
                Destroy(ui.gameObject);
            }
            return;
        }

        // ⬇ Yoksa oluştur
        if (!_activeUIs.TryGetValue(statusEffect, out var statusUI))
        {
            statusUI = Instantiate(statusEffectUIPrefab, transform);
            _activeUIs.Add(statusEffect, statusUI);
        }

        // ⬇ Sprite seç ve UI’yı güncelle
        if (statusEffect.Sprite is not null)
        {
            statusUI.Set(statusEffect.Sprite, statusEffect.Stack);
        }
    }
}
