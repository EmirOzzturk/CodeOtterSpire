using UnityEngine;

public class DiscardPileView : MonoBehaviour
{
    [SerializeField] private DeckShowPanelUI deckShowPanelUI;

    private void OnMouseDown()
    {
        // İstersen InteractionSystem kontrolü ekle
        var remaining = CardSystem.Instance.GetRemainingDiscardPile();
        deckShowPanelUI.Show(remaining);
    }
}