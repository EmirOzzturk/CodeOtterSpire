using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

/// <summary>
/// Intent tipine göre ikon ve değer gösteren UI bileşeni.
/// Inspector’da intent-sprite eşleşmelerini düzenlemek için
/// <see cref="IntentSpritePair"/> listesini kullanır.
/// </summary>
public class IntentUI : MonoBehaviour
{
    [Serializable]
    private struct IntentSpritePair
    {
        public IntentType intentType;
        public Sprite      sprite;
    }

    [Header("Sprite Eşleşmeleri")]
    [Tooltip("Inspector’dan her IntentType için bir Sprite tanımlayın.")]
    [SerializeField] private List<IntentSpritePair> spritePairs = new();

    [Header("UI Referansları")]
    [SerializeField] private TMP_Text      valueText;
    [SerializeField] private SpriteRenderer iconRenderer;   // Canvas kullanıyorsanız Image tercih edin

    // Runtime’da hızlı erişim için dictionary oluşturuyoruz.
    private readonly Dictionary<IntentType, Sprite> _spriteLookup = new();

    private void Awake()
    {
        // Listeyi dictionary’ye dönüştür – duplicate veya null’lara karşı koruma
        foreach (var pair in spritePairs)
        {
            if (pair.sprite == null)
            {
                Debug.LogWarning($"{name}: '{pair.intentType}' için sprite atanmamış.", this);
                continue;
            }

            if (_spriteLookup.ContainsKey(pair.intentType))
            {
                Debug.LogWarning($"{name}: '{pair.intentType}' için birden fazla sprite tanımlandı. İlk değer kullanılacak.", this);
                continue;
            }

            _spriteLookup.Add(pair.intentType, pair.sprite);
        }
    }

    /// <summary>
    /// Hem ikon hem metni günceller (en yaygın kullanım).
    /// </summary>
    public void Refresh(IntentType intentType, int intentValue)
    {
        UpdateIntentType(intentType);
        UpdateIntentValue(intentValue);
    }

    /// <summary>
    /// Yalnızca değeri günceller.
    /// </summary>
    public void UpdateIntentValue(int intentValue)
    {
        valueText.text = intentValue.ToString();
    }

    /// <summary>
    /// Yalnızca ikonu günceller.
    /// </summary>
    public void UpdateIntentType(IntentType intentType)
    {
        if (_spriteLookup.TryGetValue(intentType, out var sprite))
        {
            iconRenderer.sprite = sprite;
        }
        else
        {
            Debug.LogWarning($"{name}: '{intentType}' türü için sprite bulunamadı.", this);
        }
    }
}
