using UnityEngine;

[RequireComponent(typeof(CanvasGroup))]
public class ToggleFeaturePanel : MonoBehaviour
{
    [SerializeField] private KeyCode toggleKey = KeyCode.I;

    CanvasGroup cg;

    void Awake()
    {
        cg = GetComponent<CanvasGroup>();

        SetVisibility(false);
    }

    void Update()
    {
        if (Input.GetKeyDown(toggleKey))
            SetVisibility(!cg.interactable);   // mevcut durumu tersine çevir
    }

    void SetVisibility(bool show)
    {
        
        cg.alpha          = show ? 1f : 0f;   // görünürlük
        cg.interactable   = show;             // UI tıklanabilir mi?
        cg.blocksRaycasts = show;             // Raycast alır mı?
    }
}