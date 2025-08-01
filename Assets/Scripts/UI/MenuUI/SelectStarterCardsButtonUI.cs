using UnityEngine;

public class SelectStarterCardsButtonUI: MonoBehaviour
{
    [field: SerializeField, Header("UI Refs")] private SelectStarterDeckPanelUI selectStarterDeckPanelUI;

    public void ShowStarterDeckPanel()
    {
        selectStarterDeckPanelUI.Show(CharacterSelectSystem.Instance.CurrentCards);
    }
        
}