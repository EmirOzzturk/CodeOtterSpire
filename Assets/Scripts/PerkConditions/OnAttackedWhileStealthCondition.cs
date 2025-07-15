using System;
using Action_System;
using UnityEngine;

public class OnAttackedWhileStealthCondition : PerkCondition
{
    public override bool SubConditionIsMet()
    {
        return true;
    }
    public override void SubscribeCondition(Action<GameAction> reaction)
    {
        ActionSystem.SubscribeReaction<HeroAttackedWhileStealthGA>(reaction, reactionTiming);
    }

    public override void UnsubscribeCondition(Action<GameAction> reaction)
    {
        ActionSystem.UnsubscribeReaction<HeroAttackedWhileStealthGA>(reaction, reactionTiming);
    }

}