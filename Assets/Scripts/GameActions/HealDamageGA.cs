using System.Collections.Generic;
using Action_System;
using UnityEngine;

public class HealDamageGA : GameAction, IHaveCaster
{
    public int Amount { get; set; }
    public List<CombatantView> Targets { get; set; }
    public CombatantView Caster { get; private set; }

    public HealDamageGA(int amount, List<CombatantView> targets, CombatantView caster)
    {
        Amount = amount;
        Targets = new(targets);
        Caster = caster;
    }
}
