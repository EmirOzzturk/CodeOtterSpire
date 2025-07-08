using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class IntentUI : MonoBehaviour
{
    [SerializeField] private Sprite healSprite, attackSprite, powerBoostSprite;
    [SerializeField] private TMP_Text value;
    [SerializeField] private SpriteRenderer spriteRenderer;

    public void UpdateIntentUI(IntentType intentType, int intentValue)
    {
        spriteRenderer.sprite = GetSpriteByType(intentType);
        value.text = intentValue.ToString();
    }
    public void UpdateIntentValue(int intentValue)
    {
        value.text = intentValue.ToString();
    }    
    public void UpdateIntentType(IntentType intentType)
    {
        spriteRenderer.sprite = GetSpriteByType(intentType);
    }
    
    private Sprite GetSpriteByType(IntentType intentType)
    {
        return intentType switch
        {
            IntentType.HEALER => healSprite,
            IntentType.ATTACKER => attackSprite,
            IntentType.POWER_BOOSTER => powerBoostSprite,
            _ => null,
        };
    }
}
