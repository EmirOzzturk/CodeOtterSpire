using System;
using Action_System;
using UnityEngine;

public class OnDrawCondition : PerkCondition
{
    public override bool SubConditionIsMet()
    {
        return true;
    }
    public override void SubscribeCondition(Action<GameAction> reaction)
    {
        ActionSystem.SubscribeReaction<DrawCardsGA>(reaction, reactionTiming);
    }

    public override void UnsubscribeCondition(Action<GameAction> reaction)
    {
        ActionSystem.UnsubscribeReaction<DrawCardsGA>(reaction, reactionTiming);
    }

}
