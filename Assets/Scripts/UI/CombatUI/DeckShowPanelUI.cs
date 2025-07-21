using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DeckShowPanelUI : MonoBehaviour
{
    [Header("UI Refs")]
    [SerializeField] private Transform contentRoot;          // kartların ekleneceği parent
    [SerializeField] private Button closeButton;
    [SerializeField] private CardThumbnailView thumbnailPrefab;

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

    public void Show(IEnumerable<Card> remainingCards)
    {
        InteractionSystem.Instance.IsModalOpen = true;
        gameObject.SetActive(true);

        // Eski thumbnail'ları temizle
        foreach (Transform child in contentRoot) Destroy(child.gameObject);

        // Her kart için bir thumbnail oluştur
        foreach (Card card in remainingCards)
        {
            var thumb = Instantiate(thumbnailPrefab, contentRoot);
            thumb.Setup(card.cardData);
        }
    }

    public void Hide()
    {
        gameObject.SetActive(false);
        InteractionSystem.Instance.IsModalOpen = false;
    }
    
}
