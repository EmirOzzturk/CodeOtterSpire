using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Data/Hero")]
public class HeroData : ScriptableObject
{
    /*────────── TANIM ──────────*/
    [field: SerializeField, Header("Tanım")] public string HeroName { get; private set; }
    [field: SerializeField,TextArea] public string HeroDescription { get; private set; }
    [field: SerializeField] public HeroEnum HeroEnum { get; private set; }
    /*────────── GÖRSELLER ──────────*/
    [field: SerializeField, Header("Görseller")] public Sprite HeroSplashArt { get; private set; }

    [field: SerializeField] public Sprite Image { get; private set; }


    /*────────── İSTATİSTİKLER ──────────*/
    
    [field: SerializeField, Header("İstatistikler")] public int Health { get; private set; }
    [field: SerializeField] public int Attack { get; private set; }
    [field: SerializeField] public int Defense { get; private set; }
    [field: SerializeField] public int MaxMana { get; private set; }
    [field: SerializeField] public int CardDrawAmount { get; private set; }

    /*────────── BAŞLANGIÇ KARTLAR / PERKLER ──────────*/
    [field: SerializeField, Header("Başlangıç Verileri")] public List<CardData> Deck { get; set; }

    [field: SerializeField] public List<PerkData> InitialPerkDatas { get; private set; }
    
    /*────────── BÜTÜN KARAKTER KARTLARI ──────────*/
    [field: SerializeField, Header("Karaktere Özel Bütün Kartlar")] public List<CardData> AllCards { get; private set; }
    
}