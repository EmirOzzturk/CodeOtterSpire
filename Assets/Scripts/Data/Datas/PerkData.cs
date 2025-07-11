using SerializeReferenceEditor;
using UnityEngine;

[CreateAssetMenu(menuName = "Data/Perk")]
public class PerkData : ScriptableObject
{
    /*────────── GÖRSEL ──────────*/
    [field: Header("Görsel"), SerializeField]
    public Sprite Image { get; private set; }

    /*────────── KOŞUL ──────────*/
    [field: Header("    Koşul"), SerializeReference, SR]
    public PerkCondition PerkCondition { get; private set; }

    /*────────── EFEKT ──────────*/
    [field: Header("    Efekt"), SerializeReference, SR]
    public AutoTargetEffect AutoTargetEffect { get; private set; }

    /*────────── AYARLAR ──────────*/
    [field: Header("Ayarlar"), SerializeField]
    public bool UseAutoTarget { get; private set; } = true;

    [field: SerializeField]
    public bool UseActionCasterAsTarget { get; private set; } = true;
}
