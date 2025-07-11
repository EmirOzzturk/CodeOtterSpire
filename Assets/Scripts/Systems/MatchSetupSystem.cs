using System;
using System.Collections.Generic;
using Action_System;
using Systems.Card_System;
using UnityEngine;

public class MatchSetupSystem : MonoBehaviour
{
    [SerializeField] private List<EnemyData> enemyDatas;

    private void Start()
    {
        HeroSystem.Instance.Setup();
        ManaSystem.Instance.ResetManaText();
        EnemySystem.Instance.Setup(enemyDatas);

        if (HeroSystem.Instance.GetInitialPerkData() != null)
        {
            foreach (PerkData perkData in HeroSystem.Instance.GetInitialPerkData())
            { 
                PerkSystem.Instance.AddPerk(new Perk(perkData));
            }
        }
        
        DrawCardsGA drawCardsGa = new(HeroSystem.Instance.CardDrawAmount);
        ActionSystem.Instance.Perform(drawCardsGa);
    }

    private void OnDestroy()
    {
        QuitCombat();
    }

    private void QuitCombat()
    {
        PerkSystem.Instance?.RemoveAllPerks();
    }
}
