using BepInEx;
using BepInEx.Configuration;
using RiskOfOptions.Options;
using RiskOfOptions;
using RoR2;
using System;
using System.Collections.Generic;
using System.Linq;
using EntityStates.ScavBackpack;
using RoR2.Artifacts;
using UnityEngine;
using System.IO;
using RiskOfOptions.OptionConfigs;
using R2API;
using HarmonyLib;
using RoR2.ExpansionManagement;

namespace ItemLuck
{
    [BepInDependency("com.rune580.riskofoptions")]
    [BepInPlugin(PluginGUID, PluginName, PluginVersion)]
    public class ItemLuckPlugin : BaseUnityPlugin
    {
        public const string PluginGUID = "Lawlzee.ItemLuck";
        public const string PluginAuthor = "Lawlzee";
        public const string PluginName = "ItemLuck";
        public const string PluginVersion = "1.0.0";

        private ConfigEntry<bool> _modEnabled;
        private ConfigEntry<float> _itemLuck;
        private TierList _tierList;

        private ItemDef _itemLuckItem;
        private ItemDef _voidItemLuckItem;
        //todo: recycler

        private bool UpdateLuckEnabled => _modEnabled.Value && _itemLuck.Value != 0;

        public void Awake()
        {
            Log.Init(Logger);

            On.RoR2.Items.ContagiousItemManager.Init += ContagiousItemManager_Init; ;
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

            _modEnabled = Config.Bind("Configuration", "Mod enabled", true, "Mod enabled");
            ModSettingsManager.AddOption(new CheckBoxOption(_modEnabled));

            _itemLuck = Config.Bind("Configuration", "Item luck", 0f, "todo");
            ModSettingsManager.AddOption(new StepSliderOption(_itemLuck, new StepSliderConfig() { min = -5, max = 5, increment = 0.05f, formatString = "{0:+0.##;-0.##;0.##}" }));

            var texture = LoadTexture("icon.png");
            var sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0, 0));
            ModSettingsManager.SetModIcon(sprite);

            _itemLuckItem = CreateLuckItem(
                tokenName: "ItemLuck",
                model: "RabbitFoot.png",
                texture: "BorderedRabbitFoot.png",
                itemTier: ItemTier.Tier3,
                name: "Lucky Rabbit's Foot",
                pickup: "Increases item luck",
                description: "Increases <style=cIsUtility>item luck</style> by <style=cIsUtility>1</style> <style=cStack>(+1 per stack)</style>",
                lore: "In the depths of the enchanted forest, there once lived a particularly fortunate rabbit. Legend has it that this rabbit's foot was said to bring luck to anyone who possessed it. The rabbit's foot is believed to hold the essence of the rabbit's incredible luck, making it a highly sought-after item among adventurers. When carried, it increases the odds of finding valuable items and hidden treasures, making it a must-have for treasure hunters and explorers.",
                expansion: null);

            _voidItemLuckItem = CreateLuckItem(
                tokenName: "VoidItemLuck",
                model: "Feather.png",
                texture: "BorderedFeather.png",
                itemTier: ItemTier.VoidTier3,
                name: "Unlucky Crow's Feather",
                pickup: "Decreases item luck",
                description: "Decreases <style=cIsUtility>item luck</style> by <style=cIsUtility>1</style> <style=cStack>(+1 per stack)</style>. <style=cIsVoid>Corrupts all Lucky Rabbit's Foot</style>.",
                lore: "The Unlucky Crow's Feather is a cursed artifact, said to be plucked from the tail of a legendary shadowy crow that brings misfortune to those who cross its path. Possessing this feather might make you seem more intimidating, but it will undoubtedly reduce your chances of finding good items. It is believed that this feather is tied to the mysterious Crow of Ill Omen, a creature that brings bad luck and misfortune to anyone who dares to collect its feathers.",
                expansion: ExpansionCatalog.expansionDefs.FirstOrDefault(def => def.nameToken == "DLC1_NAME"));
        }

        private ItemDef CreateLuckItem(
            string tokenName,
            string model,
            string texture,
            ItemTier itemTier,
            string name,
            string pickup,
            string description,
            string lore,
            ExpansionDef expansion)
        {
            var texture2d = LoadTexture(texture);
            var sprite = Sprite.Create(texture2d, new Rect(0, 0, texture2d.width, texture2d.height), new Vector2(0, 0));


            var prefab = ItemModelFactory.Create(LoadTexture(model), tokenName, 1 / 25f, 0.6f);
            DontDestroyOnLoad(prefab);

            var itemDef = ScriptableObject.CreateInstance<ItemDef>();
            itemDef.name = "ITEM_" + tokenName;
            itemDef.nameToken = "ITEM_" + tokenName + "_NAME";
            itemDef.pickupToken = "ITEM_" + tokenName + "_PICKUP";
            itemDef.descriptionToken = "ITEM_" + tokenName + "_DESCRIPTION";
            itemDef.loreToken = "ITEM_" + tokenName + "_LORE";
            itemDef.pickupModelPrefab = prefab;
            itemDef.pickupIconSprite = sprite;
            itemDef.hidden = false;
            itemDef.deprecatedTier = itemTier;
            itemDef.requiredExpansion = expansion;

            LanguageAPI.Add(itemDef.nameToken, name);
            LanguageAPI.Add(itemDef.pickupToken, pickup);
            LanguageAPI.Add(itemDef.descriptionToken, description);
            LanguageAPI.Add(itemDef.loreToken, lore);

            ItemAPI.Add(new CustomItem(itemDef, default(ItemDisplayRule[])));

            return itemDef;
        }

        private Texture2D LoadTexture(string name)
        {
            Texture2D texture = new Texture2D(2, 2);
            texture.LoadImage(File.ReadAllBytes(System.IO.Path.Combine(System.IO.Path.GetDirectoryName(Info.Location), name)));
            return texture;
        }


        private void ContagiousItemManager_Init(On.RoR2.Items.ContagiousItemManager.orig_Init orig)
        {
            ItemCatalog.itemRelationships[DLC1Content.ItemRelationshipTypes.ContagiousItem] = ItemCatalog.itemRelationships[DLC1Content.ItemRelationshipTypes.ContagiousItem]
                .AddToArray(new ItemDef.Pair()
                {
                    itemDef1 = _itemLuckItem,
                    itemDef2 = _voidItemLuckItem
                });

            orig();
        }

        private float GetItemLuckValue()
        {
            float itemLuck = _itemLuck.Value;
            foreach (CharacterMaster readOnlyInstances in CharacterMaster.readOnlyInstancesList)
            {
                itemLuck += readOnlyInstances.inventory.GetItemCount(_itemLuckItem);
                itemLuck -= readOnlyInstances.inventory.GetItemCount(_voidItemLuckItem);
            }
            return itemLuck;
        }

        private void ItemCatalog_Init(On.RoR2.ItemCatalog.orig_Init orig)
        {
            orig();
            _tierList = TierList.Create(Config);
        }

        
        private void ChestBehavior_Roll(On.RoR2.ChestBehavior.orig_Roll orig, ChestBehavior self)
        {
            if (UpdateLuckEnabled && self.dropTable)
            {
                using var _ = ReplaceDropTable(self.dropTable, nameof(ChestBehavior_Roll));
                orig(self);
                return;
            }
            orig(self);
        }

        private void ShopTerminalBehavior_GenerateNewPickupServer_bool(On.RoR2.ShopTerminalBehavior.orig_GenerateNewPickupServer_bool orig, ShopTerminalBehavior self, bool newHidden)
        {
            if (UpdateLuckEnabled)
            {
                using var _ = ReplaceDropTable(self.dropTable, nameof(ShopTerminalBehavior_GenerateNewPickupServer_bool));
                orig(self, newHidden);
                return;
            }

            orig(self, newHidden);
        }

        private void RouletteChestController_GenerateEntriesServer(On.RoR2.RouletteChestController.orig_GenerateEntriesServer orig, RouletteChestController self, Run.FixedTimeStamp startTime)
        {
            if (UpdateLuckEnabled)
            {
                using var _ = ReplaceDropTable(self.dropTable, nameof(RouletteChestController_GenerateEntriesServer));
                orig(self, startTime);
                return;
            }

            orig(self, startTime);
        }

        private void ShrineChanceBehavior_AddShrineStack(On.RoR2.ShrineChanceBehavior.orig_AddShrineStack orig, ShrineChanceBehavior self, Interactor activator)
        {
            if (UpdateLuckEnabled)
            {
                using var _ = ReplaceDropTable(self.dropTable, nameof(ShrineChanceBehavior_AddShrineStack));
                orig(self, activator);
                return;
            }
            orig(self, activator);
        }

        private void BossGroup_DropRewards(On.RoR2.BossGroup.orig_DropRewards orig, BossGroup self)
        {
            if (UpdateLuckEnabled)
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

        private void Opening_FixedUpdate(On.EntityStates.ScavBackpack.Opening.orig_FixedUpdate orig, Opening self)
        {
            if (UpdateLuckEnabled)
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

        private void ChestBehavior_RollItem(On.RoR2.ChestBehavior.orig_RollItem orig, ChestBehavior self)
        {
            if (UpdateLuckEnabled)
            {
                self.Roll();
                return;
            }
            orig(self);
        }

        private PickupIndex FreeChestDropTable_GenerateDropPreReplacement(On.RoR2.FreeChestDropTable.orig_GenerateDropPreReplacement orig, FreeChestDropTable self, Xoroshiro128Plus rng)
        {
            if (UpdateLuckEnabled)
            {
                orig(self, rng);
                using var _ = ReplaceDropTable(self, nameof(FreeChestDropTable_GenerateDropPreReplacement));
                return PickupDropTable.GenerateDropFromWeightedSelection(rng, self.selector);
            }

            return orig(self, rng);
        }

        private PickupIndex DoppelgangerDropTable_GenerateDropPreReplacement(On.RoR2.DoppelgangerDropTable.orig_GenerateDropPreReplacement orig, DoppelgangerDropTable self, Xoroshiro128Plus rng)
        {
            if (UpdateLuckEnabled)
            {
                using var _ = ReplaceDropTable(self, nameof(DoppelgangerDropTable_GenerateDropPreReplacement));
                return orig(self, rng);
            }

            return orig(self, rng);
        }

        private void SacrificeArtifactManager_OnServerCharacterDeath(On.RoR2.Artifacts.SacrificeArtifactManager.orig_OnServerCharacterDeath orig, DamageReport damageReport)
        {
            if (UpdateLuckEnabled)
            {
                using var _ = ReplaceDropTable(SacrificeArtifactManager.dropTable, nameof(SacrificeArtifactManager_OnServerCharacterDeath));
                orig(damageReport);
                return;
            }

            orig(damageReport);
        }

        private void OptionChestBehavior_Roll(On.RoR2.OptionChestBehavior.orig_Roll orig, OptionChestBehavior self)
        {
            if (UpdateLuckEnabled)
            {
                using var _ = ReplaceDropTable(self.dropTable, nameof(OptionChestBehavior_Roll));
                orig(self);
                return;
            }

            orig(self);
        }

        private void InfiniteTowerWaveController_DropRewards(On.RoR2.InfiniteTowerWaveController.orig_DropRewards orig, InfiniteTowerWaveController self)
        {
            if (UpdateLuckEnabled)
            {
                using var _ = ReplaceDropTable(self.rewardDropTable, nameof(InfiniteTowerWaveController_DropRewards));
                orig(self);
                return;
            }

            orig(self);
        }

        private PickupIndex[] ArenaMonsterItemDropTable_GenerateUniqueDropsPreReplacement(On.RoR2.ArenaMonsterItemDropTable.orig_GenerateUniqueDropsPreReplacement orig, ArenaMonsterItemDropTable self, int maxDrops, Xoroshiro128Plus rng)
        {
            if (UpdateLuckEnabled)
            {
                using var _ = ReplaceDropTable(self, nameof(ArenaMonsterItemDropTable_GenerateUniqueDropsPreReplacement));
                return orig(self, maxDrops, rng);
            }

            return orig(self, maxDrops, rng);
        }

        private void ArenaMissionController_EndRound(On.RoR2.ArenaMissionController.orig_EndRound orig, ArenaMissionController self)
        {
            if (UpdateLuckEnabled)
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

        private IDisposable ReplaceDropTable(PickupDropTable dropTable, string caller)
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
                float itemLuckValue = GetItemLuckValue();

                for (int i = 0; i < selector.Count; i++)
                {
                    ref var choice = ref selector.choices[i];

                    float oldWeight = choice.weight;

                    Tier tier = _tierList.GetTier(choice.value.itemIndex)
                        ?? _tierList.GetTier(choice.value.equipmentIndex)
                        ?? Tier.Unspecified;

                    float multiplier = tier.ToMultiplier();

                    const float c = -0.8f;
                    float newWeigth = (2 * choice.weight) / (1 + Mathf.Exp(multiplier * itemLuckValue * c));

                    selector.ModifyChoiceWeight(i, newWeigth);
                    Log.Debug($"{Language.GetString(choice.value.pickupDef.nameToken)} weight changed from {oldWeight} to {newWeigth}");
                }

                Log.Debug($"{caller} {dropTable.GetType().Name} replaced");
            }
        }

        private IDisposable CreateSelectorCopy(WeightedSelection<PickupIndex> selector, Action<WeightedSelection<PickupIndex>> setSelector)
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
}
