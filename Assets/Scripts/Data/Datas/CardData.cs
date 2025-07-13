using System.Collections.Generic;
using SerializeReferenceEditor;
using UnityEngine;

[CreateAssetMenu(fileName = "CardData", menuName = "Data/Card")]
public class CardData : ScriptableObject
{
    /*────────── TEMEL BİLGİLER ──────────*/
    [field: Header("Temel Bilgiler"), SerializeField]
    public string CardName { get; private set; }

    [field: TextArea, SerializeField]
    public string Description { get; private set; }

    /*────────── KAYNAK & GÖRSEL ──────────*/
    [field: Header("Görsel"), SerializeField]
    public Sprite Image { get; private set; }
    
    [field: Header("Kaynak"), SerializeField]
    public int Mana { get; private set; }
    
    /*────────── PERKLER ──────────*/
    [field: Header("Perkler"), SerializeField]
    public List<PerkData> Perks { get; private set; }

    /*────────── EFEKTLER ──────────*/
    [field: Header("    Efektler"), SerializeReference, SR]
    public CardEffect ManualTargetEffect { get; private set; } = null;

    [field: SerializeField]
    public List<AutoTargetEffect> OtherEffects { get; private set; }
}