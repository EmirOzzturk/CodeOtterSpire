using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CardThumbnailView : MonoBehaviour
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
    
    
    public void Setup(CardData cardData)
    {
        if (cardData == null) return;

        title.text = cardData.CardName;
        description.text = cardData.Description;
        cardType.text = cardData.CardType.ToString();
        mana.text = cardData.Mana.ToString();
        cardImage.sprite = cardData.Image;
        backgroundImage.sprite = GetBackgroundSprite(cardData);
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
