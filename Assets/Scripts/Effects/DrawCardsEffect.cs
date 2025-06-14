using System.Collections.Generic;
using Action_System;
using UnityEngine;

public class DrawCardsEffect : Effect
{
    [SerializeField] private int drawCardsAmount;

    public override GameAction GetGameAction(List<CombatantView> targets, CombatantView caster)
    {
        DrawCardsGA drawCardsGa = new(drawCardsAmount);
        return drawCardsGa;
    }
}