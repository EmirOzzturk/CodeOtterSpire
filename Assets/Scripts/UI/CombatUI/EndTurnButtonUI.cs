using Action_System;
using UnityEngine;

public class EndTurnButtonUI : MonoBehaviour
{
    public void OnClick()
    {
        EnemyTurnGA enemyTurnGa = new();
        ActionSystem.Instance.Perform(enemyTurnGa);
    }
}
