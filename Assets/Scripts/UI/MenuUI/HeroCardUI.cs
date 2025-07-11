using Interfaces;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HeroCardUI : MonoBehaviour, ICombatStarter
{
    [Header("Data")]
    [SerializeField] private HeroData heroData;          // Inspector’dan atanacak

    [Header("UI Referances")]
    [SerializeField] private Image    heroSplashArt;
    [SerializeField] private TMP_Text heroName;
    [SerializeField] private TMP_Text healthText;
    [SerializeField] private TMP_Text manaText;
    [SerializeField] private TMP_Text attackText;
    [SerializeField] private TMP_Text defenseText;

    // Awake, Start’tan önce çalıştığı için hatayı daha erken yakalar
    private void Awake()
    {
        if (heroData == null)
        {
            Debug.LogError($"[{nameof(HeroCardUI)}] Inspector’da 'Hero Data' atanmadı! ➜ {gameObject.name}", this);
            enabled = false;          // Bileşeni devre dışı bırak, sonraki hataları engelle
            return;
        }

        Setup();
    }

    /// <summary>
    /// UI elemanlarını HeroData’daki verilerle doldurur.
    /// </summary>
    private void Setup()
    {
        heroSplashArt.sprite = heroData.HeroSplashArt;
        heroName.text        = heroData.HeroName;
        healthText.text      = heroData.Health.ToString();
        manaText.text        = heroData.MaxMana.ToString();
        attackText.text      = heroData.Attack.ToString();
        defenseText.text     = heroData.Defense.ToString();
    }

    public void StartCombat()
    {
        SceneLoadSystem.Instance.heroEnum = heroData.HeroEnum;
        SceneLoadSystem.Instance.LoadScene("Level1");
    }

#if UNITY_EDITOR
    // Inspector’da değerler değiştiğinde otomatik güncelleme (Editor sırasında)
    private void OnValidate()
    {
        if (heroData != null && Application.isPlaying == false)
        {
            Setup();
        }
    }
#endif
}