using System;
using System.Collections;
using Action_System;
using UnityEngine;
using Utils;

public class BurnSystem : Singleton<BurnSystem>
{
    [SerializeField] private GameObject burnVFX;

    private void OnEnable()
    {
        ActionSystem.AttachPerformer<ApplyBurnGA>(ApplyBurnPerformer);
    }
    private void OnDisable()
    {
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
}
