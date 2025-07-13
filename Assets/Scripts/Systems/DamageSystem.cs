using System.Collections;
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
    public void CheckCombatantViewIsDead(CombatantView target)
    {
        if (target.CurrentHealth <= 0)
        {
            if (target is EnemyView enemyView)
            {
                KillEnemyGA killEnemyGa = new(enemyView);
                ActionSystem.Instance.AddReaction(killEnemyGa);
            }
            else
            {
                InCombatUISystem.Instance.ShowResultPanel(false);
            }
        }
    }

    private IEnumerator DealDamagePerformer(DealDamageGA dealDamageGA)
    {
        foreach (var target in dealDamageGA.Targets)
        {
            if (target == null) yield break;
            target.Damage(dealDamageGA.Amount);
            if (damageVFX != null)
            {
                Instantiate(damageVFX, target.transform.position, Quaternion.identity);
                SFXSystem.Instance.Play(SFXType.Hit);
                yield return Wait.Half;
            }
            
            CheckCombatantViewIsDead(target);
        }
    } 
    
    private IEnumerator HealDamagePerformer(HealDamageGA healDamageGA)
    {
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
