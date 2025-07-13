using System.Collections.Generic;
using System.Linq;
using Action_System;
using UnityEngine;

public class AddAttackPowerEnemyEffect : EnemyEffect
{
    [SerializeField] private int attackPowerAmount;
    
    public override GameAction GetGameAction(List<CombatantView> targets, CombatantView caster)
    {
        // Düşmanlardan Caster'ı çıkartır.
        var enemyTargets = EnemySystem.Instance.Enemies
            .Where(t => t != caster)
            .ToList();

        AddAttackPowerGA AddAttackPowerGA = new(attackPowerAmount, enemyTargets, caster);
        return AddAttackPowerGA;
    }

    public override int GetEffectValue()
    {
        return attackPowerAmount;
    }
}
