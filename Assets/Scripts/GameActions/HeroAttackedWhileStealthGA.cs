using System.Collections.Generic;
using Action_System;
using UnityEngine;

public class HeroAttackedWhileStealthGA: GameAction, IHaveCaster
{
    public CombatantView Caster { get; private set; }

    public HeroAttackedWhileStealthGA(CombatantView caster)
    {
        Caster = caster;
    }
}