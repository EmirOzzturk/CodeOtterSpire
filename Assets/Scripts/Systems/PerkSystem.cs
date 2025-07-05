using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PerkSystem : Singleton<PerkSystem>
{
    [SerializeField] private PerksUI perksUI;
    private readonly List<Perk> perks = new();

    public void RemoveAllPerks()
    {
        if (perks.Count == 0) return;
        foreach (var perk in perks.ToList())
        {
            RemovePerk(perk);
        }
    }
    
    public void AddPerk(Perk perk)
    {
        perks.Add(perk);
        perk.OnAdd();
        perksUI.AddPerkUI(perk);
    }
    public void RemovePerk(Perk perk)
    {
        perksUI.RemovePerkUI(perk);
        perk.OnRemove();
        perks.Remove(perk);
    }
}
