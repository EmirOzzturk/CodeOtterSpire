using System.Collections.Generic;
using Action_System;
using UnityEngine;

public class AddStatusEffectGA : GameAction, IHaveCaster
{
    public StatusEffect StatusEffect { get; private set; }
    public List<CombatantView> Targets { get; private set; }
    public CombatantView Caster { get; private set; }

    public AddStatusEffectGA(StatusEffect statusEffect, List<CombatantView> targets, CombatantView caster = null)
    {
        StatusEffect = statusEffect;
        Targets = targets;
        Caster = caster;
    }
    
    
}
