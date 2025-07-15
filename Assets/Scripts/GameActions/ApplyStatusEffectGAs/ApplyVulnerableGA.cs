using Action_System;
using UnityEngine;

public class ApplyVulnerableGA : GameAction
{
    public int TurnDuration { get; private set; }
    public CombatantView Target { get; private set; }

    public ApplyVulnerableGA(int turnDuration, CombatantView target)
    {
        TurnDuration = turnDuration;
        Target = target;
    }
}