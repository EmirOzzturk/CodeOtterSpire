using System;
using System.Collections;
using Action_System;
using UnityEngine;
using Utils;

public class StatusEffectApplySystem : Singleton<StatusEffectApplySystem>
{
    [SerializeField] private GameObject burnVFX;

    private void OnEnable()
    {
        ActionSystem.AttachPerformer<ApplyBurnGA>(ApplyBurnPerformer);
        ActionSystem.AttachPerformer<ApplyArmorGA>(ApplyArmorPerformer);
        ActionSystem.AttachPerformer<ApplyStealthGA>(ApplyStealthPerformer);
        ActionSystem.AttachPerformer<ApplyDamageMultiplierGA>(ApplyDamageMultiplierPerformer);
        ActionSystem.AttachPerformer<ApplyVulnerableGA>(ApplyVulnerablePerformer);
    }
    private void OnDisable()
    {
        ActionSystem.DetachPerformer<ApplyVulnerableGA>();   
        ActionSystem.DetachPerformer<ApplyDamageMultiplierGA>();   
        ActionSystem.DetachPerformer<ApplyStealthGA>();   
        ActionSystem.DetachPerformer<ApplyArmorGA>();   
        ActionSystem.DetachPerformer<ApplyBurnGA>();   
    }

    private IEnumerator ApplyBurnPerformer(ApplyBurnGA applyBurnGa)
    {
        CombatantView target = applyBurnGa.Target as CombatantView;
        Instantiate(burnVFX, target.transform.position, Quaternion.identity);
        target.Damage(applyBurnGa.BurnDamage);
        target.RemoveStatusEffect(StatusEffectType.BURN, 1);
        yield return Wait.One;
        
        DamageSystem.Instance.CheckCombatantViewIsDead(target);
    }
    
    private IEnumerator ApplyArmorPerformer(ApplyArmorGA applyArmorGa)
    {
        CombatantView target = applyArmorGa.Target as CombatantView;
        // Instantiate(burnVFX, target.transform.position, Quaternion.identity);
        AddStatusEffectGA addStatusEffectGa = new(StatusEffectType.ARMOR, applyArmorGa.ArmorValue,
            new() { applyArmorGa.Target });
        
        ActionSystem.Instance.Perform(addStatusEffectGa);
        yield return Wait.One;
    }
    
    private IEnumerator ApplyStealthPerformer(ApplyStealthGA applyStealthGa)
    {
        CombatantView target = applyStealthGa.Target as CombatantView;
        
        target.RemoveStatusEffect(StatusEffectType.STEALTH, 1);
        // Instantiate(burnVFX, target.transform.position, Quaternion.identity);

        AddStatusEffectGA addStatusEffectGa = new(StatusEffectType.DAMAGE_MULTIPLIER, 2,
            new() { applyStealthGa.Target });
        
        ActionSystem.Instance.Perform(addStatusEffectGa);
        yield return Wait.One;
    }
    
    private IEnumerator ApplyDamageMultiplierPerformer(ApplyDamageMultiplierGA applyDamageMultiplierGa)
    {
        CombatantView target = applyDamageMultiplierGa.Target as CombatantView;
        
        target.RemoveStatusEffect(StatusEffectType.DAMAGE_MULTIPLIER, 2);
        // Instantiate(burnVFX, target.transform.position, Quaternion.identity);
        
        yield return Wait.Quarter;
    }

    private IEnumerator ApplyVulnerablePerformer(ApplyVulnerableGA applyVulnerableGa)
    {
        CombatantView target = applyVulnerableGa.Target as CombatantView;
        target.RemoveStatusEffect(StatusEffectType.VULNERABLE, 1);
        yield return Wait.Seconds(0.1f);
    }
}