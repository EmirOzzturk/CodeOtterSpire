using System.Collections.Generic;
using System.Linq;
using Action_System;
using UnityEngine;

public class HealAllDamageEnemyEffect : EnemyEffect
{
    [SerializeField] private int healAmount;
    public override GameAction GetGameAction(List<CombatantView> targets, CombatantView caster)
    {
        HealDamageGA healDamageGa = new(healAmount, EnemySystem.Instance.Enemies.ToCombatantList(), caster);
        return healDamageGa;
    }
    
    public override int GetEffectValue()
    {
        return healAmount;
    }
}
