using System;
using System.Collections;
using Action_System;
using UnityEngine;
using Utils;

public class StatusEffectSystem : Singleton<StatusEffectSystem>
{
    private void OnEnable()
    {
        ActionSystem.AttachPerformer<AddStatusEffectGA>(AddStatusEffectPerformer);
    }

    private void OnDisable()
    {
        ActionSystem.DetachPerformer<AddStatusEffectGA>();
    }

    private IEnumerator AddStatusEffectPerformer(AddStatusEffectGA addStatusEffectGA)
    {
        foreach (var target in addStatusEffectGA.Targets)
        {
            // Enemy tarafından Hero'ya AddStatusEffect varsa Hero targetable mı diye kontrol et.
            var heroView = HeroSystem.Instance.HeroView;
            if (target == heroView && addStatusEffectGA.Caster is EnemyView && HeroSystem.Instance.HeroView.Targetable == 0)
            {
                HeroAttackedWhileStealthGA heroAttackedWhileStealthGa = new(addStatusEffectGA.Caster);
                ActionSystem.Instance.AddReaction(heroAttackedWhileStealthGa);
                yield break;
            }
            
            target.AddStatusEffect(addStatusEffectGA.StatusEffect);
            yield return Wait.Quarter; // add animation
        }
    }
    
    // publics + Event Handlers
    public void OnAddStatusEffect(StatusEffect statusEffect, CombatantView combatantView)
    {
        switch (statusEffect.StatusEffectType)
        {
            case StatusEffectType.STEALTH:
                combatantView.Targetable = 0;
                break;
            default:
                Console.WriteLine("Bilinmeyen durum efekti.");
                break;
        }
    } 
    public void OnRemoveStatusEffect(StatusEffect statusEffect, CombatantView combatantView)
    {
        switch (statusEffect.StatusEffectType)
        {
            case StatusEffectType.STEALTH:
                combatantView.Targetable = 1;
                ActionSystem.Instance.AddReaction(new AddStatusEffectGA(
                    StatusEffectCreator.Instance.Create(StatusEffectType.DAMAGE_MULTIPLIER, 2),
                    new() { combatantView }));
                break;
            default:
                Console.WriteLine("Bilinmeyen durum efekti.");
                break;
        }
    } 
}
