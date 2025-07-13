using System;
using System.Collections;
using System.Collections.Generic;
using Action_System;
using DG.Tweening;
using NUnit.Framework;
using UnityEngine;
using Utils;

public class EnemySystem : Singleton<EnemySystem>
{
    [SerializeField] private EnemyBoardView enemyBoardView;
    [SerializeField] private GameObject AddAttackPowerVFX;
    public List<EnemyView> Enemies => enemyBoardView.EnemyViews;
    
    void OnEnable()
    {
        ActionSystem.AttachPerformer<EnemyTurnGA>(EnemyTurnPerformer);
        ActionSystem.AttachPerformer<AttackHeroGA>(AttackHeroPerformer);
        ActionSystem.AttachPerformer<KillEnemyGA>(KillEnemyPerformer);
        ActionSystem.AttachPerformer<AddAttackPowerGA>(AddAttackPowerPerformer);
    }

    void OnDisable()
    {
        ActionSystem.DetachPerformer<AddAttackPowerGA>();
        ActionSystem.DetachPerformer<KillEnemyGA>();
        ActionSystem.DetachPerformer<EnemyTurnGA>();
        ActionSystem.DetachPerformer<AttackHeroGA>();
    }

    public void Setup(List<EnemyData> enemyDatas)
    {
        foreach (var enemyData in enemyDatas)
        {
            enemyBoardView.AddEnemy(enemyData);        
        }
        
        // Her eksik enemy'de x eksenine 3 ekle
        enemyBoardView.transform.position += new Vector3(9 - (enemyDatas.Count * 3), 0, 0);
    }
    
    // Performers
    private IEnumerator EnemyTurnPerformer(EnemyTurnGA enemyTurnGa)
    {
        foreach (var enemy in enemyBoardView.EnemyViews)
        {
            int burnStacks = enemy.GetStatusEffectStacks(StatusEffectType.BURN);
            if (burnStacks > 0)
            {
                ApplyBurnGA applyBurnGa = new(burnStacks, enemy);
                ActionSystem.Instance.AddReaction(applyBurnGa);
            }
            
            if (enemy.EnemyEffects is null)
            {
                Debug.LogWarning($"{enemy.name} → EnemyEffects listesi boş.");
                yield break;
            }
            
            // 2) Tüm efektleri sırayla uygula
            var targets = new List<CombatantView> { HeroSystem.Instance.HeroView };

            foreach (var effect in enemy.GetCurrentEffects())
            {
                ActionSystem.Instance.AddReaction(
                    new PerformEffectGA(effect, targets, enemy)
                );
            }

        }
        yield return null;
    }

    private IEnumerator AttackHeroPerformer(AttackHeroGA attackHeroGa)
    {
        EnemyView attacker = attackHeroGa?.Attacker;
        if (attacker == null) yield break;
        
        Tween tween = attacker.transform.DOMoveX(attacker.transform.position.x - 1f, 0.15f);
        yield return tween.WaitForCompletion();
        attacker.transform.DOMoveX(attacker.transform.position.x + 1f, 0.25f);
        DealDamageGA dealDamageGa = new(attacker.AttackPower, new() { HeroSystem.Instance.HeroView }, attackHeroGa.Caster);
        ActionSystem.Instance.AddReaction(dealDamageGa);
    }

    private IEnumerator KillEnemyPerformer(KillEnemyGA killEnemyGa)
    {
        yield return enemyBoardView.RemoveEnemy(killEnemyGa.EnemyView);
        if (enemyBoardView.EnemyViews is { Count: 0 })
        {
            InCombatUISystem.Instance.ShowResultPanel(true);
        }
    }

    private IEnumerator AddAttackPowerPerformer(AddAttackPowerGA addAttackPowerGa)
    {
        // Hedef yoksa coroutine'i hemen bitir
        if (addAttackPowerGa.Targets == null || addAttackPowerGa.Targets.Count == 0)
            yield break;
        
        // for döngüsü: GC yok, indeks erişimi hızlı
        for (int i = 0; i < addAttackPowerGa.Targets.Count; i++)
        {
            var target = addAttackPowerGa.Targets[i];
            target.AddAttackPower(addAttackPowerGa.Amount);

            if (AddAttackPowerVFX != null)
            {
                Instantiate(AddAttackPowerVFX, target.transform.position  + Vector3.down, Quaternion.identity);
                SFXSystem.Instance.Play(SFXType.Buff);
                yield return Wait.Half;   // Statik nesne, GC yok
            }
        }
    }

}
