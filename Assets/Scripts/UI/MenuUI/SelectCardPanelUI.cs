using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SelectCardPanelUI : MonoBehaviour
{
    [Header("Prefabs")]
    [field: SerializeField] public SelectCardView selectCardViewPrefab;
    [Header("UI Refs")]
    [SerializeField] private Transform contentRoot;          // kartların ekleneceği parent
    [SerializeField] private Button closeButton;
        
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

        // Eski card'ları temizle
        foreach (Transform child in contentRoot) Destroy(child.gameObject);
                
        foreach (CardData cardData in cardsToShow)
        {
            var cardSelector = Instantiate(selectCardViewPrefab, contentRoot);
            cardSelector.Setup(cardData);
        }
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }
}