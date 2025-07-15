using Action_System;
using UnityEngine;

public class ApplyArmorGA : GameAction
{
    public int ArmorValue { get; private set; }
    public CombatantView Target { get; private set; }

    public ApplyArmorGA(int armorValue, CombatantView target)
    {
        ArmorValue = armorValue;
        Target = target;
    }
}