using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class NoTM : TargetMode
{
    public override List<CombatantView> GetTargets()
    {
        return new List<CombatantView>();
    }
}
