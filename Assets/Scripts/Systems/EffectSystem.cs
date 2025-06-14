using System;
using System.Collections;
using Action_System;
using UnityEngine;

public class EffectSystem : MonoBehaviour
{
    void OnEnable()
    {
        ActionSystem.AttachPerformer<PerformEffectGA>(PerformEffectPerformer);
    }

    void OnDisable()
    {
        ActionSystem.DetachPerformer<PerformEffectGA>();
    }

    // Performers
    private IEnumerator PerformEffectPerformer(PerformEffectGA performEffectGa)
    {
        if (performEffectGa.Effect == null) yield break;
        GameAction effectAction = performEffectGa.Effect.GetGameAction(performEffectGa.Targets, HeroSystem.Instance.HeroView);
        ActionSystem.Instance.AddReaction(effectAction);
        yield return null;
    }
}
