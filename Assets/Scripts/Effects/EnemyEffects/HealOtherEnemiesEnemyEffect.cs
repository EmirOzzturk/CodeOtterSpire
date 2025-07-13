using System.Collections.Generic;
using System.Linq;
using Action_System;
using UnityEngine;


public class HealOtherEnemiesEnemyEffect : EnemyEffect
{
    [SerializeField] private int healAmount;
    public override GameAction GetGameAction(List<CombatantView> targets, CombatantView caster)
    {
        // Düşmanlardan Caster'ı çıkartır.
        var enemyTargets = EnemySystem.Instance.Enemies
            .Where(t => t != caster)
            .ToCombatantList();
        
        HealDamageGA healDamageGa = new(healAmount, enemyTargets, caster);
        return healDamageGa;
    }
    
    public override int GetEffectValue()
    {
        return healAmount;
    }
}