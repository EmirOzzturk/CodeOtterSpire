using System;
using System.Collections.Generic;
using Action_System;
using UnityEngine;
using UnityEngine.Pool;

public class Perk
{
    public Sprite Image => data.Image;
    
    private readonly PerkData data;
    private readonly PerkCondition condition;
    private readonly List<AutoTargetEffect> effects;

    public Perk(PerkData perkData)
    {
        data = perkData;
        condition = data.PerkCondition;
        effects = data.AutoTargetEffects;
    }

    public void OnAdd()
    {
        condition.SubscribeCondition(Reaction);
    }

    public void OnRemove()
    {
        condition.UnsubscribeCondition(Reaction);
    }

    private void Reaction(GameAction gameAction)
    {
        // 1) Guard clause – koşul sağlanmıyorsa direkt çık
        if (condition == null || !condition.SubConditionIsMet())
            return;

        foreach (var autoTargetEffect in effects)
        {
            // 2) Pooled list – GC alloc yok, try/finally ile güvenli iade
            var targets = ListPool<CombatantView>.Get();
            try
            {
                // ——— Hedef topla ————————————————————————————
                if (data.UseActionCasterAsTarget && gameAction is IHaveCaster { Caster: not null } haveCaster)
                    targets.Add(haveCaster.Caster);

                if (data.UseAutoTarget)
                    targets.AddRange(autoTargetEffect.TargetMode.GetTargets());

                // ——— GA oluştur & ActionSystem’e ekle ——————————
                var action = autoTargetEffect.Effect.GetGameAction(targets, HeroSystem.Instance.HeroView);
                ActionSystem.Instance.AddReaction(action);
            }
            finally
            {
                ListPool<CombatantView>.Release(targets);
            }
        }
    }
}
