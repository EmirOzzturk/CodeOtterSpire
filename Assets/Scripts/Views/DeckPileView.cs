using UnityEngine;

public class DeckPileView : MonoBehaviour
{
    [SerializeField] private DeckShowPanelUI deckShowPanelUI;

    private void OnMouseDown()
    {
        // İstersen InteractionSystem kontrolü ekle
        var remaining = CardSystem.Instance.GetRemainingDrawPileForDeckShow();
        deckShowPanelUI.Show(remaining);
    }
}