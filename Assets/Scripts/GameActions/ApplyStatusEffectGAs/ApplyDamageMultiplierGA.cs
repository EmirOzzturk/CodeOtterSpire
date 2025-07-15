using Action_System;
using UnityEngine;

public class ApplyDamageMultiplierGA : GameAction
{
    public int DamageMultiplier { get; private set; }
    public CombatantView Target { get; private set; }

    public ApplyDamageMultiplierGA(int damageMultiplier, CombatantView target)
    {
        DamageMultiplier = damageMultiplier;
        Target = target;
    }
}