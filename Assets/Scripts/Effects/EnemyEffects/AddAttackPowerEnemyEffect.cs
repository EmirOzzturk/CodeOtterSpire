using System.Collections.Generic;
using Action_System;
using UnityEngine;

public class AddAttackPowerEnemyEffect : Effect
{
    [SerializeField] private int attackPowerAmount;
    [SerializeField] private List<EnemyView> enemies;
    
    public override GameAction GetGameAction(List<CombatantView> targets, CombatantView caster)
    {
        AddAttackPowerGA AddAttackPowerGA = new(attackPowerAmount, targets, caster);
        return AddAttackPowerGA;
    }
}
