using System.Collections.Generic;
using Action_System;
using UnityEngine;

public class DrawCardsEffect : Effect
{

    public override GameAction GetGameAction(List<CombatantView> targets, CombatantView caster)
    {

        DrawCardsGA drawCardsGa = new(1);
        return drawCardsGa;
    }
    
    public override int GetEffectValue()
    {
        return 1;
    }
}