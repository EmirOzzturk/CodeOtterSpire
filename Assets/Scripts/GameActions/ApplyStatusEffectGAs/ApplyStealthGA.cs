using Action_System;
using UnityEngine;

public class ApplyStealthGA : GameAction
{
    public int TurnDuration { get; private set; }
    public CombatantView Target { get; private set; }

    public ApplyStealthGA(int turnDuration, CombatantView target)
    {
        TurnDuration = turnDuration;
        Target = target;
    }
}