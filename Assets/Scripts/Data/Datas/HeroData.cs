using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Data/Hero")]
public class HeroData : ScriptableObject
{
    /*────────── GÖRSELLER ──────────*/
    [field: SerializeField, Header("Görseller")] public string HeroName { get; private set; }

    [field: SerializeField] public Sprite HeroSplashArt { get; private set; }

    [field: SerializeField] public Sprite Image { get; private set; }

    /*────────── TANIM ──────────*/
    [field: SerializeField, Header("Tanım")] public HeroEnum HeroEnum { get; private set; }

    /*────────── İSTATİSTİKLER ──────────*/
    
    [field: SerializeField, Header("İstatistikler")] public int Health { get; private set; }
    [field: SerializeField] public int Attack { get; private set; }
    [field: SerializeField] public int Defense { get; private set; }
    [field: SerializeField] public int MaxMana { get; private set; }
    [field: SerializeField] public int CardDrawAmount { get; private set; }

    /*────────── BAŞLANGIÇ DESTESİ / PERKLER ──────────*/
    [field: SerializeField, Header("Başlangıç Verileri")] public List<CardData> Deck { get; private set; }

    [field: SerializeField] public List<PerkData> InitialPerkDatas { get; private set; }
}