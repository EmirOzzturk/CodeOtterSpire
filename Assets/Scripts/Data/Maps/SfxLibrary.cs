using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "AudioData/SFX Library")]
public class SfxLibrary : ScriptableObject
{
    [System.Serializable]
    private struct Pair
    {
        public SFXType type;
        public SFXData data;
    }

    [SerializeField] private List<Pair> pairs = new List<Pair>();

    private Dictionary<SFXType, SFXData> cache;

    private void OnEnable()
    {
        cache = new Dictionary<SFXType, SFXData>();
        foreach (var p in pairs) cache[p.type] = p.data;
    }

    public SFXData Get(SFXType type) =>
        cache != null && cache.TryGetValue(type, out var data) ? data : null;
}