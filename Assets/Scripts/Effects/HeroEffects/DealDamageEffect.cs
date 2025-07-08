using System.Collections.Generic;
using Action_System;
using UnityEngine;

public class DealDamageEffect : Effect
{
    [SerializeField] private int damageAmount;
    public override GameAction GetGameAction(List<CombatantView> targets, CombatantView caster)
    {
        DealDamageGA dealDamageGa = new(damageAmount, targets, caster);
        return dealDamageGa;
    }
    
    public override int GetEffectValue()
    {
        return damageAmount;
    }
}
