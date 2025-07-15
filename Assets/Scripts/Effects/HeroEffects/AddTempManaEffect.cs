using System.Collections.Generic;
using Action_System;
using UnityEngine;

public class AddTempManaEffect : CardEffect
{
    [SerializeField] private int tempMana;
    
    public override GameAction GetGameAction(List<CombatantView> targets, CombatantView caster)
    {
        AddTempManaGA addTempManaGa = new(tempMana);
        return addTempManaGa;
    }
    
    public override int GetEffectValue()
    {
        return tempMana;
    }
}