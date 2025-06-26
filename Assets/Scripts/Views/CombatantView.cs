using DG.Tweening;
using TMPro;
using UnityEngine;

public class CombatantView : MonoBehaviour
{
    [SerializeField] private TMP_Text healthText;
    [SerializeField] private SpriteRenderer spriteRenderer;
    
    public int MaxHealth { get; private set; }
    public int CurrentHealth { get; private set; }

    protected void SetupBase(int health, Sprite image)
    {
        MaxHealth = CurrentHealth = health;
        spriteRenderer.sprite = image;
        UpdateHealthText();
    }

    public void Damage(int damage)
    {
        CurrentHealth -= damage;
        if (CurrentHealth < 0)
        {
            CurrentHealth = 0;
        }
        
        UpdateHealthText();
        transform.DOShakePosition(0.2f, 0.3f);
    }

    public void Heal(int heal)
    {
        if (CurrentHealth + heal <= MaxHealth)
        {
            CurrentHealth += heal;
        }
        else
        {
            CurrentHealth = MaxHealth;
        }
        
        UpdateHealthText();
    }
    
    private void UpdateHealthText()
    {
        healthText.text = "Health: " + CurrentHealth.ToString();
    }
}
