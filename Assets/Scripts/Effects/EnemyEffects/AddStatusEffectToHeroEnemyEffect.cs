using System.Collections.Generic;
using Action_System;
using UnityEngine;

public class AddStatusEffectToHeroEnemyEffect : EnemyEffect
{
    [SerializeField] private StatusEffectData statusEffectData;
    [SerializeField] private int stackCount;
    public override GameAction GetGameAction(List<CombatantView> targets, CombatantView caster)
    {
        return new AddStatusEffectGA(new StatusEffect(statusEffectData, stackCount), targets, caster);
    }

    public override int GetEffectValue()
    {
        return stackCount;
    }
}