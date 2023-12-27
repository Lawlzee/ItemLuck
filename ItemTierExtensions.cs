using RoR2;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace ItemLuck;

public static class ItemTierExtensions
{
    public static string GetShortName(this ItemTier itemTier)
    {
        return itemTier switch
        {
            ItemTier.Tier1 => "White items",
            ItemTier.Tier2 => "Green items",
            ItemTier.Tier3 => "Red items",
            ItemTier.Lunar => "Lunar items",
            ItemTier.Boss => "Boss items",
            ItemTier.VoidTier1 => "Void items",
            ItemTier.VoidTier2 => "Void items",
            ItemTier.VoidTier3 => "Void items",
            ItemTier.VoidBoss => "Void items",
            _ => "Other items"
        };
    }

    public static string GetFullName(this ItemTier itemTier)
    {
        return itemTier switch
        {
            ItemTier.Tier1 => "White items",
            ItemTier.Tier2 => "Green items",
            ItemTier.Tier3 => "Red items",
            ItemTier.Lunar => "Lunar items",
            ItemTier.Boss => "Boss items",
            ItemTier.VoidTier1 => "Void white items",
            ItemTier.VoidTier2 => "Void green items",
            ItemTier.VoidTier3 => "Void red items",
            ItemTier.VoidBoss => "Void boss items",
            _ => "Other items"
        };
    }
}
