using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SelectStarterDeckPanelUI: MonoBehaviour
{
        [Header("Prefabs")]
        [field: SerializeField] public CardSelectorView cardSelectorPrefab;
        [Header("UI Refs")]
        [SerializeField] private Transform contentRoot;          // kartların ekleneceği parent
        [SerializeField] private Button closeButton;
        [SerializeField] private TMP_Text deckInfoText;
        
        public List<CardData> CardsToShow { get; set; }
        
        private void Awake()
        {
                closeButton.onClick.AddListener(Hide);
        }

        private void OnDestroy()
        {
                closeButton.onClick.RemoveAllListeners();
        }
    
        private void Update()
        {
                if (Input.GetKeyDown(KeyCode.Escape))
                        Hide();
        }

        public void Show(IEnumerable<CardData> cardsToShow)
        {
                gameObject.SetActive(true);
                UpdateDeckInfoText();
                
                // Eski card'ları temizle
                foreach (Transform child in contentRoot) Destroy(child.gameObject);
                
                foreach (CardData cardData in cardsToShow)
                {
                        var cardSelector = Instantiate(cardSelectorPrefab, contentRoot);
                        cardSelector.Setup(cardData);
                }
        }

        public void Hide()
        {
                gameObject.SetActive(false);
        }

        public void UpdateDeckInfoText()
        {
                var totalCardbyCardRarity = CharacterSelectSystem.Instance.GetDeckLimit();
                deckInfoText.text = "Common Cards Count: "+totalCardbyCardRarity[CardRarity.Common]+"" +
                                    "\nRare Cards Count: "+totalCardbyCardRarity[CardRarity.Rare]+"" +
                                    "\nLegendary Cards Count: "+totalCardbyCardRarity[CardRarity.Legendary]+"" +
                                    "\nCrimson Cards Count: "+totalCardbyCardRarity[CardRarity.Crimson]+"";
        }
        
}