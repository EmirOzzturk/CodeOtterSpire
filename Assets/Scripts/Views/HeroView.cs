using System;
using UnityEngine;

public class HeroView : CombatantView
{
    public static int? SetupHeroHealth { get; set; }
    public void Setup(HeroData heroData)
    {
        SetupBase(heroData.Health, heroData.Image, SetupHeroHealth);
    }

    public void OnDestroy()
    {
        if (TurnSystem.Instance == null) return;
        DestroyBase();
    }
}
