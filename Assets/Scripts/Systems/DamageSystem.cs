using System.Collections;
using System.Collections.Generic;
using Action_System;
using UnityEngine;
using Utils;

public class DamageSystem : Singleton<DamageSystem>
{
    [SerializeField] private GameObject damageVFX;
    [SerializeField] private GameObject healVFX;

    private void OnEnable()
    {
        ActionSystem.AttachPerformer<DealDamageGA>(DealDamagePerformer);
        ActionSystem.AttachPerformer<HealDamageGA>(HealDamagePerformer);
    }

    private void OnDisable()
    {
        ActionSystem.DetachPerformer<HealDamageGA>();
        ActionSystem.DetachPerformer<DealDamageGA>();
    }
    
    // publics
    public bool CheckCombatantViewIsDead(CombatantView target)
    {
        if (target.CurrentHealth <= 0)
        {
            if (target is EnemyView enemyView)
            {
                KillEnemyGA killEnemyGa = new(enemyView);
                ActionSystem.Instance.AddReaction(killEnemyGa);
                return true;
            }
            else
            {
                InCombatUISystem.Instance.ShowResultPanel(false);
                return true;
            }
        }
        else
        {
            return false;
        }
    }

    private IEnumerator DealDamagePerformer(DealDamageGA dealDamageGA)
    {
        foreach (var target in dealDamageGA.Targets)
        {
            if (target == null) yield break;

            var damageMultiplier = dealDamageGA.Caster.GetStatusEffectStacks(StatusEffectType.DAMAGE_MULTIPLIER);
            var finalDamage = damageMultiplier == 0 ? dealDamageGA.Amount : damageMultiplier * dealDamageGA.Amount;
            target.Damage(finalDamage);
            
            if (damageVFX != null)
            {
                Instantiate(damageVFX, target.transform.position, Quaternion.identity);
                SFXSystem.Instance.Play(SFXType.Hit);
                yield return Wait.Half;
            }

            if (CheckCombatantViewIsDead(target))
            {
                HealDamageGA healDamageGA = new(dealDamageGA.HealOnKill,
                    new() { HeroSystem.Instance.HeroView }, HeroSystem.Instance.HeroView);
                ActionSystem.Instance.AddReaction(healDamageGA);
            }
        }
    } 
    
    private IEnumerator HealDamagePerformer(HealDamageGA healDamageGA)
    {
        if (healDamageGA.Amount == 0) yield break;
        foreach (var target in healDamageGA.Targets)
        {
            target.Heal(healDamageGA.Amount);
            if (healVFX != null)
            {
                Instantiate(healVFX, target.transform.position, Quaternion.identity);
                SFXSystem.Instance.Play(SFXType.Heal);
                yield return Wait.Seconds(0.2f);
            }
        }
    } 
}
