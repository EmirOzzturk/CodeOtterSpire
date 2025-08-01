using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CardSelectorView : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    [Header("UI")]
    [SerializeField] private TMP_Text title;
    [SerializeField] private TMP_Text description;
    [SerializeField] private TMP_Text cardType;
    [SerializeField] private TMP_Text mana;
    [SerializeField] private Image cardImage;

    [Header("Background")]
    [SerializeField] private Image backgroundImage;
    [SerializeField] private Sprite redBg;
    [SerializeField] private Sprite yellowBg;
    [SerializeField] private Sprite whiteBg;
    [SerializeField] private Sprite greenBg;

    [Header("Misc")]
    [SerializeField] private GameObject wrapper;
    [SerializeField] private TMP_Text selectCardText;
    [SerializeField] private GameObject cardLockGroup;

    public CardData CardData { get; private set; }
    
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (!CardData.IsChangeable) return;

        selectCardText.gameObject.SetActive(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (!CardData.IsChangeable) return;

        selectCardText.gameObject.SetActive(false);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (!CardData.IsChangeable) return;

        CharacterSelectSystem.Instance.OpenCardSelectPanel(this);
    }
    
    public void Setup(CardData cardData)
    {
        if (cardData == null) return;

        CardData = cardData;
        title.text = cardData.CardName;
        description.text = cardData.Description;
        cardType.text = cardData.CardType.ToString();
        mana.text = cardData.Mana.ToString();
        cardImage.sprite = cardData.Image;
        backgroundImage.sprite = GetBackgroundSprite(cardData);

        if (cardData.IsLocked)
        {
            cardLockGroup.SetActive(true);
        }
    }
    
    private Sprite GetBackgroundSprite(CardData cardData)
    {
        return cardData.CardColor switch
        {
            CardColor.RED    => redBg,
            CardColor.GREEN  => greenBg,
            CardColor.WHITE  => whiteBg,
            CardColor.YELLOW => yellowBg,
            _                => whiteBg,
        };
    }
}