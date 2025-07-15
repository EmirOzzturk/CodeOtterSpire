using System.Collections.Generic;
using Action_System;
using UnityEngine;

public class DealDamageAndHealifDeadEffect : CardEffect
{
    [SerializeField] private int damageAmount;
    [SerializeField] private int healOnKillAmount;
    public override GameAction GetGameAction(List<CombatantView> targets, CombatantView caster)
    {
        DealDamageGA dealDamageGa = new(damageAmount, targets, caster, healOnKillAmount);
        return dealDamageGa;
    }
    
    public override int GetEffectValue()
    {
        return damageAmount;
    }
}