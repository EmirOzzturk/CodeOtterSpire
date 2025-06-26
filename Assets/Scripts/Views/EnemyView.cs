using TMPro;
using UnityEngine;

public class EnemyView : CombatantView
{
    [SerializeField] private TMP_Text attackText;
    
    public int AttackPower { get; private set; }

    public void Setup(EnemyData enemyData)
    {
        AttackPower = enemyData.AttackPower;
        UpdateAttackText();
        SetupBase(enemyData.Health, enemyData.Image);
    }

    public void AddAttackPower(int power)
    {
        AttackPower+=power;
        UpdateAttackText();
    }

    private void UpdateAttackText()
    {
        attackText.text = "Attack: " + AttackPower;
    }
}
