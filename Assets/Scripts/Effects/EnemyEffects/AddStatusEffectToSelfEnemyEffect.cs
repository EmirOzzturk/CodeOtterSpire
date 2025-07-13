using System.Collections.Generic;
using Action_System;
using UnityEngine;

public class AddStatusEffectToSelfEnemyEffect : EnemyEffect
{
    [SerializeField] private StatusEffectType statusEffectType;
    [SerializeField] private int stackCount;
    public override GameAction GetGameAction(List<CombatantView> targets, CombatantView caster)
    {
        return new AddStatusEffectGA(statusEffectType, stackCount, new() { caster });
    }

    public override int GetEffectValue()
    {
        return stackCount;
    }
}
