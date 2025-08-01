using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CharacterIconUI : MonoBehaviour
{
    [Header("Data")]
    [SerializeField] private HeroData heroData;          // Inspector’dan atanacak

    [Header("UI Referances")]
    [SerializeField] private Image    heroSplashArt;
    [SerializeField] private TMP_Text heroName;
    [SerializeField] private TMP_Text heroDescription;
    [SerializeField] private TMP_Text healthText;

    private void Start()
    {
        if (heroData.HeroEnum == HeroEnum.HUNTER)
        {
            Select();
        }
    }
    
    /// <summary>
    /// UI elemanlarını HeroData’daki verilerle doldurur.
    /// </summary>
    public void Select()
    {
        heroSplashArt.sprite = heroData.HeroSplashArt;
        heroName.text        = heroData.HeroName;
        heroDescription.text = heroData.HeroDescription;
        healthText.text      = "HP: " + heroData.Health.ToString();

        CharacterSelectSystem.Instance.SelectedHero = heroData;
        CharacterSelectSystem.Instance.SetCurrentCards(heroData.Deck);
    }
    
}