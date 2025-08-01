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
    
    [field: SerializeField]
    public CardType CardType { get; private set; }
    
    [field: SerializeField]
    public CardRarity CardRarity { get; private set; }

    /*────────── KAYNAK & GÖRSEL ──────────*/
    [field: Header("Görsel"), SerializeField]
    public Sprite Image { get; private set; }
    [field: SerializeField]
    public CardColor CardColor { get; private set; }
    
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
    
    /*────────── DESTE OLUŞTURMADA KULLANILAN ÖZELLİKLER ──────────*/
    [field: Header("Deste Oluşturmada Kullanılan Özellikler"), SerializeField]
    public bool IsLocked { get; set; } = false;
    
    [field: SerializeField]
    public bool IsChangeable { get; set; } = true;
    
    
    /*────────── SPECIAL GETTER ──────────*/
    public int GetCopyLimit()
    {
        return CardRarity switch
        {
            CardRarity.Rare      => 3,
            CardRarity.Legendary => 2,
            CardRarity.Crimson   => 2,
            _                    => int.MaxValue   // Common (veya tanımsız) → sınırsız
        };
    }
}