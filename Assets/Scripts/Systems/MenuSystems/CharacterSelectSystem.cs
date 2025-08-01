using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CharacterSelectSystem : Singleton<CharacterSelectSystem>
{ 
    [field: SerializeField] public CardData LockedCardData { get; set; }
    [field: SerializeField] private SelectCardPanelUI SelectCardPanel { get; set; }
    [field: SerializeField] private SelectStarterDeckPanelUI SelectStarterDeckPanelUI { get; set; }
    
    //  ----------- CONST'S -----------
    const int UnlockedCardCount = 18;
    private static readonly IReadOnlyDictionary<CardRarity, int> DeckLimit = new Dictionary<CardRarity, int>
    {
        [CardRarity.Rare]      = 12,
        [CardRarity.Legendary] = 6,
        [CardRarity.Crimson]   = 2
    };
    
    //  ----------- PROPERTIES -----------
    public List<CardData> CurrentCards { get; private set; }
    private CardSelectorView SelectedCardSelectorView { get; set; }
    public HeroData SelectedHero { get; set; }
    private Dictionary<CardRarity, int> deckLimit = new Dictionary<CardRarity, int>();

    public void Start()
    {
        UpdateDeckLimit();
    }

    public void UpdateStarterDeck()
    {
        SelectedHero.Deck = CurrentCards.Where((cardData) => !cardData.IsLocked).ToList();
    }
    
    // -------- SELECT CARD PANEL --------
    public void OpenCardSelectPanel(CardSelectorView cardSelectorView)
    {
        SelectedCardSelectorView = cardSelectorView;
        var selectableCards = GetSelectableCards();
        SelectCardPanel.Show(selectableCards);
    }

    public void AddCardToCurrentCardsViaSelect(CardData cardData)
    {
        if (cardData == SelectedCardSelectorView.CardData)
        {
            SelectCardPanel.Hide();
        }
        else
        {
            RemoveCardFromCurrentCards(SelectedCardSelectorView.CardData);
            AddCardToCurrentCards(cardData);
            UpdateDeckLimit();
            SelectStarterDeckPanelUI.UpdateDeckInfoText();
            
            SelectedCardSelectorView.Setup(cardData);
        }
    }
    

    
    /// <summary>
    ///  İstenen rarity’de olup, toplam sınırı ve kopya sınırını aşmayacak kartları döndürür.
    /// </summary>
    public IEnumerable<CardData> GetSelectableCards()
    {
        if (SelectedHero is null)
            return Enumerable.Empty<CardData>();

        // Destede ilgili rarity’den kaç kart var?
        deckLimit[CardRarity.Common]    = CurrentCards.Count(c => c.CardRarity == CardRarity.Common);
        deckLimit[CardRarity.Rare]      = CurrentCards.Count(c => c.CardRarity == CardRarity.Rare);
        deckLimit[CardRarity.Legendary] = CurrentCards.Count(c => c.CardRarity == CardRarity.Legendary);
        deckLimit[CardRarity.Crimson]   = CurrentCards.Count(c => c.CardRarity == CardRarity.Crimson);

        // Hero havuzundan seçilebilen kartlar
        var selectable = SelectedHero.AllCards
            .Where(card =>
            {
                // Burada card adatadan sınırları al
                int duplicates = CurrentCards.Count(c => c.CardName == card.CardName);
                return duplicates < card.GetCopyLimit(); // Kopya limiti aşılmıyor mu? 
            })
            .Where(card =>
            {
                int deckCap = DeckLimit.TryGetValue(card.CardRarity,  out var cap)  ? cap  : int.MaxValue;
                return deckLimit[card.CardRarity] < deckCap;
            })
            .ToList();

        return selectable;
    }
    
    // -------- CURRENT CARDS WRAPPER FUNCTIONS --------
    public void AddCardToCurrentCards(CardData cardData)
    {
        CurrentCards.Add(cardData);
        SortCurrentCardsByRarity();
    }
    
    public bool RemoveCardFromCurrentCards(CardData cardData)
    {
        bool res = CurrentCards.Remove(cardData);
        SortCurrentCardsByRarity();
        return res;
    }
    
    public void SetCurrentCards(IEnumerable<CardData> cards)
    {
        if (cards == null)
            throw new ArgumentNullException(nameof(cards));


        // Dış listenin kazara değiştirilmesini önlemek için yeni bir kopya oluşturuyoruz
        CurrentCards = new List<CardData>(cards);

        // Eksik kart sayısını hesapla
        int missingCount = UnlockedCardCount - CurrentCards.Count;

        // Eksik adedi kilitli kartlarla doldur
        for (int i = 0; i < missingCount; i++)
        {
            CurrentCards.Add(LockedCardData);
        }
    }

    public void SortCurrentCardsByRarity()
    {
        CurrentCards = CurrentCards
            .OrderBy(c => c.CardRarity)
            .ToList();
    }

    private void UpdateDeckLimit()
    {
        deckLimit[CardRarity.Common]    = CurrentCards.Count(c => c.CardRarity == CardRarity.Common);
        deckLimit[CardRarity.Rare]      = CurrentCards.Count(c => c.CardRarity == CardRarity.Rare);
        deckLimit[CardRarity.Legendary] = CurrentCards.Count(c => c.CardRarity == CardRarity.Legendary);
        deckLimit[CardRarity.Crimson]   = CurrentCards.Where(c => !c.IsLocked)
            .Count(c => c.CardRarity == CardRarity.Crimson);
    }

    public Dictionary<CardRarity, int> GetDeckLimit()
    {
        return deckLimit;
    }
}