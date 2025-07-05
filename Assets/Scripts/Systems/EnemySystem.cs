using System;
using System.Collections;
using System.Collections.Generic;
using Action_System;
using DG.Tweening;
using UnityEngine;

public class EnemySystem : Singleton<EnemySystem>
{
    [SerializeField] private EnemyBoardView enemyBoardView;
    public List<EnemyView> Enemies => enemyBoardView.EnemyViews;
    
    void OnEnable()
    {
        ActionSystem.AttachPerformer<EnemyTurnGA>(EnemyTurnPerformer);
        ActionSystem.AttachPerformer<AttackHeroGA>(AttackHeroPerformer);
        ActionSystem.AttachPerformer<KillEnemyGA>(KillEnemyPerformer);
    }

    void OnDisable()
    {
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
    }
    
    // Performers
    private IEnumerator EnemyTurnPerformer(EnemyTurnGA enemyTurnGa)
    {
        foreach (var enemy in enemyBoardView.EnemyViews)
        {
            AttackHeroGA attackHeroGa = new(enemy);
            ActionSystem.Instance.AddReaction(attackHeroGa);
        }
        yield return null;
    }

    private IEnumerator AttackHeroPerformer(AttackHeroGA attackHeroGa)
    {
        EnemyView attacker = attackHeroGa.Attacker;
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
}
