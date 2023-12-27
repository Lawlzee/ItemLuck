using BepInEx.Configuration;
using EntityStates.ScavBackpack;
using RoR2;
using RoR2.Artifacts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace ItemLuck;

public static class Hooks
{
    private static ConfigFile _config;

    public static void Init(ConfigFile config)
    {
        _config = config;

        On.RoR2.ItemCatalog.Init += ItemCatalog_Init;

        On.RoR2.ChestBehavior.Roll += ChestBehavior_Roll;
        On.RoR2.ShopTerminalBehavior.GenerateNewPickupServer_bool += ShopTerminalBehavior_GenerateNewPickupServer_bool;
        On.RoR2.RouletteChestController.GenerateEntriesServer += RouletteChestController_GenerateEntriesServer;
        On.RoR2.ShrineChanceBehavior.AddShrineStack += ShrineChanceBehavior_AddShrineStack;
        On.RoR2.FreeChestDropTable.GenerateDropPreReplacement += FreeChestDropTable_GenerateDropPreReplacement;
        On.RoR2.OptionChestBehavior.Roll += OptionChestBehavior_Roll;

        On.RoR2.BossGroup.DropRewards += BossGroup_DropRewards;
        On.EntityStates.ScavBackpack.Opening.FixedUpdate += Opening_FixedUpdate;
        On.RoR2.ChestBehavior.RollItem += ChestBehavior_RollItem;

        On.RoR2.DoppelgangerDropTable.GenerateDropPreReplacement += DoppelgangerDropTable_GenerateDropPreReplacement;
        On.RoR2.Artifacts.SacrificeArtifactManager.OnServerCharacterDeath += SacrificeArtifactManager_OnServerCharacterDeath;

        On.RoR2.InfiniteTowerWaveController.DropRewards += InfiniteTowerWaveController_DropRewards;
        On.RoR2.ArenaMonsterItemDropTable.GenerateUniqueDropsPreReplacement += ArenaMonsterItemDropTable_GenerateUniqueDropsPreReplacement;
        On.RoR2.ArenaMissionController.EndRound += ArenaMissionController_EndRound;
    }

    private static void ItemCatalog_Init(On.RoR2.ItemCatalog.orig_Init orig)
    {
        orig();
        ItemLuckPlugin.TierList = TierList.Create(_config);
        UpdatableOptions.UpdateDescriptions();
    }

    private static void ChestBehavior_Roll(On.RoR2.ChestBehavior.orig_Roll orig, ChestBehavior self)
    {
        if (ItemLuckPlugin.UpdateLuckEnabled && self.dropTable)
        {
            using var _ = ReplaceDropTable(self.dropTable, nameof(ChestBehavior_Roll));
            orig(self);
            return;
        }
        orig(self);
    }

    private static void ShopTerminalBehavior_GenerateNewPickupServer_bool(On.RoR2.ShopTerminalBehavior.orig_GenerateNewPickupServer_bool orig, ShopTerminalBehavior self, bool newHidden)
    {
        if (ItemLuckPlugin.UpdateLuckEnabled)
        {
            using var _ = ReplaceDropTable(self.dropTable, nameof(ShopTerminalBehavior_GenerateNewPickupServer_bool));
            orig(self, newHidden);
            return;
        }

        orig(self, newHidden);
    }

    private static void RouletteChestController_GenerateEntriesServer(On.RoR2.RouletteChestController.orig_GenerateEntriesServer orig, RouletteChestController self, Run.FixedTimeStamp startTime)
    {
        if (ItemLuckPlugin.UpdateLuckEnabled)
        {
            using var _ = ReplaceDropTable(self.dropTable, nameof(RouletteChestController_GenerateEntriesServer));
            orig(self, startTime);
            return;
        }

        orig(self, startTime);
    }

    private static void ShrineChanceBehavior_AddShrineStack(On.RoR2.ShrineChanceBehavior.orig_AddShrineStack orig, ShrineChanceBehavior self, Interactor activator)
    {
        if (ItemLuckPlugin.UpdateLuckEnabled)
        {
            using var _ = ReplaceDropTable(self.dropTable, nameof(ShrineChanceBehavior_AddShrineStack));
            orig(self, activator);
            return;
        }
        orig(self, activator);
    }

    private static void BossGroup_DropRewards(On.RoR2.BossGroup.orig_DropRewards orig, BossGroup self)
    {
        if (ItemLuckPlugin.UpdateLuckEnabled)
        {
            using var disposables = new CompositeDisposable();
            if (self.dropTable)
            {
                disposables.Add(ReplaceDropTable(self.dropTable, nameof(BossGroup_DropRewards)));
            }
            else
            {
                Log.Info($"dropTable is {self.dropTable?.GetType().Name ?? "null"}");
            }

            foreach (PickupDropTable dropTable in self.bossDropTables.Distinct())
            {
                disposables.Add(ReplaceDropTable(dropTable, nameof(BossGroup_DropRewards)));
            }

            orig(self);
            return;
        }

        orig(self);
    }

    private static void Opening_FixedUpdate(On.EntityStates.ScavBackpack.Opening.orig_FixedUpdate orig, Opening self)
    {
        if (ItemLuckPlugin.UpdateLuckEnabled)
        {
            var chestBehavior = self.GetComponent<ChestBehavior>();
            var dropTable = (BasicPickupDropTable)chestBehavior.dropTable;

            using var _ = CreateSelectorCopy(dropTable.selector, x => dropTable.selector = x);

            dropTable.selector.Clear();

            List<PickupIndex> lunarCoin = new List<PickupIndex>
                {
                    PickupCatalog.FindPickupIndex(RoR2Content.MiscPickups.LunarCoin.miscPickupIndex)
                };

            dropTable.Add(Run.instance.availableTier1DropList, chestBehavior.tier1Chance / Run.instance.availableTier1DropList.Count);
            dropTable.Add(Run.instance.availableTier2DropList, chestBehavior.tier2Chance / Run.instance.availableTier2DropList.Count);
            dropTable.Add(Run.instance.availableTier3DropList, chestBehavior.tier3Chance / Run.instance.availableTier3DropList.Count);
            dropTable.Add(Run.instance.availableLunarCombinedDropList, chestBehavior.lunarChance / Run.instance.availableLunarCombinedDropList.Count);
            dropTable.Add(lunarCoin, chestBehavior.lunarCoinChance);

            using var __ = ReplaceDropTable(dropTable, nameof(Opening_FixedUpdate));
            orig(self);
            return;
        }

        orig(self);
    }

    private static void ChestBehavior_RollItem(On.RoR2.ChestBehavior.orig_RollItem orig, ChestBehavior self)
    {
        if (ItemLuckPlugin.UpdateLuckEnabled)
        {
            self.Roll();
            return;
        }
        orig(self);
    }

    private static PickupIndex FreeChestDropTable_GenerateDropPreReplacement(On.RoR2.FreeChestDropTable.orig_GenerateDropPreReplacement orig, FreeChestDropTable self, Xoroshiro128Plus rng)
    {
        if (ItemLuckPlugin.UpdateLuckEnabled)
        {
            orig(self, rng);
            using var _ = ReplaceDropTable(self, nameof(FreeChestDropTable_GenerateDropPreReplacement));
            return PickupDropTable.GenerateDropFromWeightedSelection(rng, self.selector);
        }

        return orig(self, rng);
    }

    private static PickupIndex DoppelgangerDropTable_GenerateDropPreReplacement(On.RoR2.DoppelgangerDropTable.orig_GenerateDropPreReplacement orig, DoppelgangerDropTable self, Xoroshiro128Plus rng)
    {
        if (ItemLuckPlugin.UpdateLuckEnabled)
        {
            using var _ = ReplaceDropTable(self, nameof(DoppelgangerDropTable_GenerateDropPreReplacement));
            return orig(self, rng);
        }

        return orig(self, rng);
    }

    private static void SacrificeArtifactManager_OnServerCharacterDeath(On.RoR2.Artifacts.SacrificeArtifactManager.orig_OnServerCharacterDeath orig, DamageReport damageReport)
    {
        if (ItemLuckPlugin.UpdateLuckEnabled)
        {
            using var _ = ReplaceDropTable(SacrificeArtifactManager.dropTable, nameof(SacrificeArtifactManager_OnServerCharacterDeath));
            orig(damageReport);
            return;
        }

        orig(damageReport);
    }

    private static void OptionChestBehavior_Roll(On.RoR2.OptionChestBehavior.orig_Roll orig, OptionChestBehavior self)
    {
        if (ItemLuckPlugin.UpdateLuckEnabled)
        {
            using var _ = ReplaceDropTable(self.dropTable, nameof(OptionChestBehavior_Roll));
            orig(self);
            return;
        }

        orig(self);
    }

    private static void InfiniteTowerWaveController_DropRewards(On.RoR2.InfiniteTowerWaveController.orig_DropRewards orig, InfiniteTowerWaveController self)
    {
        if (ItemLuckPlugin.UpdateLuckEnabled)
        {
            using var _ = ReplaceDropTable(self.rewardDropTable, nameof(InfiniteTowerWaveController_DropRewards));
            orig(self);
            return;
        }

        orig(self);
    }

    private static PickupIndex[] ArenaMonsterItemDropTable_GenerateUniqueDropsPreReplacement(On.RoR2.ArenaMonsterItemDropTable.orig_GenerateUniqueDropsPreReplacement orig, ArenaMonsterItemDropTable self, int maxDrops, Xoroshiro128Plus rng)
    {
        if (ItemLuckPlugin.UpdateLuckEnabled)
        {
            using var _ = ReplaceDropTable(self, nameof(ArenaMonsterItemDropTable_GenerateUniqueDropsPreReplacement));
            return orig(self, maxDrops, rng);
        }

        return orig(self, maxDrops, rng);
    }

    private static void ArenaMissionController_EndRound(On.RoR2.ArenaMissionController.orig_EndRound orig, ArenaMissionController self)
    {
        if (ItemLuckPlugin.UpdateLuckEnabled)
        {
            using var disposables = new CompositeDisposable();

            foreach (var rewardOrder in self.playerRewardOrder)
            {
                disposables.Add(ReplaceDropTable(rewardOrder, nameof(ArenaMissionController_EndRound)));
            }

            orig(self);
            return;
        }

        orig(self);
    }

    private static IDisposable ReplaceDropTable(PickupDropTable dropTable, string caller)
    {
        if (dropTable is BasicPickupDropTable basicDropTable)
        {
            IDisposable disposable = CreateSelectorCopy(basicDropTable.selector, x => basicDropTable.selector = x);
            ReplaceDropTableSelector(basicDropTable.selector);
            return disposable;
        }

        if (dropTable is ExplicitPickupDropTable explicitDropTable)
        {
            IDisposable disposable = CreateSelectorCopy(explicitDropTable.weightedSelection, x => explicitDropTable.weightedSelection = x);
            ReplaceDropTableSelector(explicitDropTable.weightedSelection);
            return disposable;
        }

        if (dropTable is FreeChestDropTable freeChestDropTable)
        {
            IDisposable disposable = CreateSelectorCopy(freeChestDropTable.selector, x => freeChestDropTable.selector = x);
            ReplaceDropTableSelector(freeChestDropTable.selector);
            return disposable;
        }

        if (dropTable is DoppelgangerDropTable doppelgangerDropTable)
        {
            IDisposable disposable = CreateSelectorCopy(doppelgangerDropTable.selector, x => doppelgangerDropTable.selector = x);
            ReplaceDropTableSelector(doppelgangerDropTable.selector);
            return disposable;
        }

        if (dropTable is ArenaMonsterItemDropTable arenaMonsterItemDropTable)
        {
            IDisposable disposable = CreateSelectorCopy(arenaMonsterItemDropTable.selector, x => arenaMonsterItemDropTable.selector = x);
            ReplaceDropTableSelector(arenaMonsterItemDropTable.selector);
            return disposable;
        }

        Log.Warning($"Failed to override {caller} dropTable");
        Log.Warning($"{caller} dropTable is of type {dropTable?.GetType().FullName ?? "null"}");

        return Disposable.Empty;

        void ReplaceDropTableSelector(WeightedSelection<PickupIndex> selector)
        {
            float itemLuckValue = ItemLuckPlugin.ItemLuck.Value;

            for (int i = 0; i < selector.Count; i++)
            {
                ref var choice = ref selector.choices[i];

                float oldWeight = choice.weight;

                Tier tier = ItemLuckPlugin.TierList.GetTier(choice.value.itemIndex)
                    ?? ItemLuckPlugin.TierList.GetTier(choice.value.equipmentIndex)
                    ?? Tier.Unspecified;

                float newWeigth = tier.ApplyItemLuck(choice.weight);

                selector.ModifyChoiceWeight(i, newWeigth);
                Log.Debug($"{Language.GetString(choice.value.pickupDef.nameToken)} weight changed from {oldWeight} to {newWeigth}");
            }

            Log.Debug($"{caller} {dropTable.GetType().Name} ({dropTable.name}) replaced");
        }
    }

    private static IDisposable CreateSelectorCopy(WeightedSelection<PickupIndex> selector, Action<WeightedSelection<PickupIndex>> setSelector)
    {
        WeightedSelection<PickupIndex> oldSelector = selector;

        setSelector(new WeightedSelection<PickupIndex>
        {
            Capacity = selector.Capacity,
            choices = selector.choices.ToArray(),
            totalWeight = selector.totalWeight,
            Count = selector.Count
        });

        return new Disposable(() =>
        {
            setSelector(oldSelector);
        });
    }
}
