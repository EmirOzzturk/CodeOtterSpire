using System;
using System.Collections.Generic;
using Action_System;
using UnityEngine;

public class DealDamageEnemyEffect : Effect
{
    public override GameAction GetGameAction(List<CombatantView> targets, CombatantView caster)
    {
        AttackHeroGA attackHeroGA = new((EnemyView) caster);
        return attackHeroGA;
    }

    public override int GetEffectValue()
    {
        return -1;
    }
}
