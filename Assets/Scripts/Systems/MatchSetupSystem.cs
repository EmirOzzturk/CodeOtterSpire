using System;
using System.Collections.Generic;
using Action_System;
using Systems.Card_System;
using UnityEngine;

public class MatchSetupSystem : MonoBehaviour
{
    [SerializeField] private HeroData heroData;
    [SerializeField] private PerkData perkData;
    [SerializeField] private List<EnemyData> enemyDatas;
    [SerializeField] private int gameStartDrawCount = 4;

    private void Start()
    {
        HeroSystem.Instance.Setup(heroData);   
        ManaSystem.Instance.ResetManaText();
        EnemySystem.Instance.Setup(enemyDatas);
        CardSystem.Instance.Setup(heroData.Deck);

        if (perkData != null)
        {
            PerkSystem.Instance.AddPerk(new Perk(perkData));
        }

        DrawCardsGA drawCardsGa = new(gameStartDrawCount);
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
