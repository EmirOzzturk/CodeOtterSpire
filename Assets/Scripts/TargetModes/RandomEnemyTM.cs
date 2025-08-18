using System.Collections.Generic;
using UnityEngine;

public class RandomEnemyTM : TargetMode
{
    public override List<CombatantView> GetTargets()
    {
        // Ensure there are available enemies before selecting a random target
        var enemies = EnemySystem.Instance.Enemies;
        if (enemies == null || enemies.Count == 0) return null;

        CombatantView target = enemies[Random.Range(0, enemies.Count)];
        return new() { target };
    }
}
