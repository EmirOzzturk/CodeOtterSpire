using System.Collections.Generic;
using Action_System;
using UnityEngine;

public class DealDamageGA : GameAction, IHaveCaster
{
    public int Amount { get; set; }
    public List<CombatantView> Targets { get; set; }
    public CombatantView Caster { get; private set; }
    public int HealOnKill { get; set; }

    public DealDamageGA(int amount, List<CombatantView> targets, CombatantView caster, int healOnKill = 0)
    {
        Amount = amount;
        Targets = new(targets);
        Caster = caster;
        HealOnKill = healOnKill;
    }
}
