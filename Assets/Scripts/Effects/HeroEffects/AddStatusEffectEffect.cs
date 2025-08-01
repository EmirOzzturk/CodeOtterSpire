using System.Collections.Generic;
using Action_System;
using UnityEngine;

public class AddStatusEffectEffect : CardEffect
{
    [SerializeField] private StatusEffectData statusEffectData;
    [SerializeField] private int stackCount;
    public override GameAction GetGameAction(List<CombatantView> targets, CombatantView caster)
    {
        AddStatusEffectGA addStatusEffectGa = new (new StatusEffect(statusEffectData, stackCount), targets, caster);
        return addStatusEffectGa;
    }
    
    public override int GetEffectValue()
    {
        return stackCount;
    }
}
