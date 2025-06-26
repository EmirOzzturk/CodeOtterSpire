using System.Collections.Generic;
using Action_System;
using UnityEngine;

public class AddAttackPowerGA : GameAction
{
    public int Amount { get; set; }
    public List<CombatantView> Targets { get; set; }
    public CombatantView Caster { get; private set; }

    public AddAttackPowerGA(int amount, List<CombatantView> targets, CombatantView caster)
    {
        Amount = amount;
        Targets = new(targets);
        Caster = caster;
    }
}
