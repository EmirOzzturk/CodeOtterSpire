
using System.Collections.Generic;
using UnityEngine;

public class Card
{
    public readonly CardData cardData;
    
    public string Title => cardData.CardName; // Name of the Object
    public string Description => cardData.Description;
    public string CardTypeString => cardData.CardType.ToString();
    public Sprite Image => cardData.Image;
    public CardEffect ManualTargetEffect => cardData.ManualTargetEffect;
    public List<PerkData> Perks => cardData.Perks;
    public List<AutoTargetEffect> OtherEffects => cardData.OtherEffects;
    public int Mana {get; private set;}


    public Card(CardData cardData)
    {
        this.cardData = cardData;
        Mana = cardData.Mana;
    }

    public Card(Card card)
    {
        this.cardData = card.cardData;
        Mana = cardData.Mana;
    }
}
