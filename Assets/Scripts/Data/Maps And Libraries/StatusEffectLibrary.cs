using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(menuName = "Library/Status Effect Library")]
public class StatusEffectLibrary : ScriptableObject
{
    [SerializeField]
    private List<StatusEffectData> statusEffects;

    // Runtime’da hızlı erişim için cache.
    private Dictionary<StatusEffectType, StatusEffectData> _lookup;

    /// <summary> İstenen türe ait StatusEffectData’yı döner. </summary>
    public StatusEffectData Get(StatusEffectType type)
    {
        // İlk çağrıda sözlüğü oluştur.
        _lookup ??= statusEffects.ToDictionary(se => se.StatusEffectType);
        return _lookup.TryGetValue(type, out var data)
            ? data
            : throw new KeyNotFoundException($"StatusEffect {type} Library’de yok.");
    }

#if UNITY_EDITOR        // Editörde yinelenen type kontrolü, hatayı erken görmenizi sağlar.
    private void OnValidate()
    {
        var dups = statusEffects
            .GroupBy(se => se.StatusEffectType)
            .Where(g => g.Count() > 2)
            .Select(g => g.Key)
            .ToList();
        if (dups.Count > 0)
            Debug.LogError($"StatusEffectLibrary: Yinelenen StatusEffectType’lar: {string.Join(", ", dups)}", this);
    }
#endif
}