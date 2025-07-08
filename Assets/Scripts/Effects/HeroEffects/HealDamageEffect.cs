using System.Collections.Generic;
using Action_System;
using UnityEngine;

public class HealDamageEffect : Effect
{
    [SerializeField] private int healAmount;
    public override GameAction GetGameAction(List<CombatantView> targets, CombatantView caster)
    {
        HealDamageGA healDamageGa = new(healAmount, targets, caster);
        return healDamageGa;
    }
    
    public override int GetEffectValue()
    {
        return healAmount;
    }
}
