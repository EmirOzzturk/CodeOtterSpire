using System;
using System.Collections.Generic;
using TMPro;            // Eğer StatusEffectUI içinde kullanıyorsanız
using UnityEngine;

/// <summary>
/// Oyuncu veya düşman üzerindeki tüm durum etkilerini (armor, burn vb.)
/// dinamik olarak gösterir.
/// Inspector’da <see cref="StatusSpritePair"/> listesi üzerinden
/// durum-sprite eşleşmelerini düzenleyebilirsiniz.
/// </summary>
public class StatusEffectsUI : MonoBehaviour
{
    [Serializable]
    private struct StatusSpritePair
    {
        public StatusEffectType statusType;
        public Sprite           sprite;
    }

    [Header("Sprite Eşleşmeleri")]
    [SerializeField] private List<StatusSpritePair> spritePairs = new();

    [Header("Prefab")]
    [SerializeField] private StatusEffectUI statusEffectUIPrefab;

    // —— Dahili ————————————————————————————————————————————————
    private readonly Dictionary<StatusEffectType, StatusEffectUI> _activeUIs   = new();
    private readonly Dictionary<StatusEffectType, Sprite>         _spriteLookup = new();

    private void Awake()
    {
        // Inspector’daki listeyi hızlı erişim için dictionary’ye aktar
        foreach (var pair in spritePairs)
        {
            if (pair.sprite == null)
            {
                Debug.LogWarning($"{name}: '{pair.statusType}' için sprite atanmamış.", this);
                continue;
            }
            if (_spriteLookup.ContainsKey(pair.statusType))
            {
                Debug.LogWarning($"{name}: '{pair.statusType}' için birden fazla sprite var. İlk değer kullanılacak.", this);
                continue;
            }
            _spriteLookup.Add(pair.statusType, pair.sprite);
        }
    }

    /// <summary>
    /// İlgili durum etkisini günceller veya kaldırır.
    /// </summary>
    public void UpdateStatusEffectUI(StatusEffectType type, int stackCount)
    {
        // ⬇ Kaldırılacaksa
        if (stackCount <= 0)
        {
            if (_activeUIs.TryGetValue(type, out var ui))
            {
                _activeUIs.Remove(type);
                Destroy(ui.gameObject);      // Basit örnek: pooling düşünüyorsanız burayı değiştirin
            }
            return;
        }

        // ⬇ Yoksa oluştur
        if (!_activeUIs.TryGetValue(type, out var statusUI))
        {
            statusUI = Instantiate(statusEffectUIPrefab, transform);
            _activeUIs.Add(type, statusUI);
        }

        // ⬇ Sprite seç ve UI’yı güncelle
        if (_spriteLookup.TryGetValue(type, out var sprite))
            statusUI.Set(sprite, stackCount);
        else
            Debug.LogWarning($"{name}: '{type}' türü için sprite bulunamadı.", this);
    }
}
