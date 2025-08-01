using UnityEngine;

public class StatusEffectCreator : Singleton<StatusEffectCreator>
{
    [SerializeField] private StatusEffectLibrary library;

    public StatusEffect Create(StatusEffectType type, int stack = 1)
    {
        var data = library.Get(type);           // Oyun boyunca hep aynı referans
        return new StatusEffect(data, stack);   // Model örneği
    }
}