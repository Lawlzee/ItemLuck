using RiskOfOptions.Options;
using RiskOfOptions;
using RoR2;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using BepInEx.Configuration;
using RiskOfOptions.OptionConfigs;

namespace ItemLuck;

public class TierList
{
    private readonly Dictionary<ItemIndex, ConfigEntry<Tier>> _itemConfigByItem;
    private readonly Dictionary<EquipmentIndex, ConfigEntry<Tier>> _equipmentConfigByItem;

    private TierList(Dictionary<ItemIndex, ConfigEntry<Tier>> itemConfigByItem, Dictionary<EquipmentIndex, ConfigEntry<Tier>> equipmentConfigByItem)
    {
        _itemConfigByItem = itemConfigByItem;
        _equipmentConfigByItem = equipmentConfigByItem;
    }

    public static TierList Create(ConfigFile config)
    {
        Log.Debug("Initializing TierList");
        DefaultTierList defaultTierList = DefaultTierList.Create();

        Dictionary<ItemIndex, Rank<ItemDef>> itemRankByIndex = defaultTierList.Items
            .ToDictionary(x => x.Def.itemIndex);

        Dictionary<ItemIndex, ConfigEntry<Tier>> itemConfigByItem = new Dictionary<ItemIndex, ConfigEntry<Tier>>();

        var items = ItemCatalog.itemDefs
            .Where(x => x.tier != ItemTier.NoTier)
            .Where(x => x.canRemove)
            .Where(x => !x.hidden)
            .Where(x => x.DoesNotContainTag(ItemTag.Scrap))
            .Where(x => x.DoesNotContainTag(ItemTag.WorldUnique))
            .Select(x => new
            {
                Name = NormaliseName(Language.GetString(x.nameToken)),
                ItemDef = x
            })
            .OrderBy(x => x.ItemDef.tier)
            .ThenBy(x => x.Name);

        foreach (var item in items)
        {
            Tier defaultTier = Tier.Unspecified;
            if (itemRankByIndex.TryGetValue(item.ItemDef.itemIndex, out Rank<ItemDef> rank))
            {
                defaultTier = rank.Tier;
            }

            string itemTierName = item.ItemDef.tier switch
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

            var itemConfig = config.Bind(itemTierName, item.Name, defaultTier, "test2");
            itemConfigByItem[item.ItemDef.itemIndex] = itemConfig;

            var choiceOption = new ChoiceConfig()
            {
                description = "test"
            };
            ModSettingsManager.AddOption(new ChoiceOption(itemConfig, choiceOption));

            itemConfig.SettingChanged += (_, _) =>
            {
                choiceOption.description = itemConfig.Value.ToString();
            };
        }

        Dictionary<EquipmentIndex, Rank<EquipmentDef>> equipmentRankByIndex = defaultTierList.Equipments
            .ToDictionary(x => x.Def.equipmentIndex);

        Dictionary<EquipmentIndex, ConfigEntry<Tier>> equipmentConfigByItem = new Dictionary<EquipmentIndex, ConfigEntry<Tier>>();

        var equipements = EquipmentCatalog.equipmentDefs
            .Where(x => x.canDrop)
            .Select(x => new
            {
                Name = NormaliseName(Language.GetString(x.nameToken)),
                EquipmentDef = x
            })
            .OrderBy(x => x.Name);

        foreach (var equipment in equipements)
        {
            Tier defaultTier = Tier.C;
            if (equipmentRankByIndex.TryGetValue(equipment.EquipmentDef.equipmentIndex, out Rank<EquipmentDef> rank))
            {
                defaultTier = rank.Tier;
            }

            var equipmentConfig = config.Bind("Equipments", equipment.Name, defaultTier);
            equipmentConfigByItem[equipment.EquipmentDef.equipmentIndex] = equipmentConfig;

            ModSettingsManager.AddOption(new ChoiceOption(equipmentConfig));
        }

        Log.Debug("TierList Initialized");
        return new TierList(itemConfigByItem, equipmentConfigByItem);

        string NormaliseName(string name)
        {
            //'=', '\n', '\t', '\\', '"', '\'', '[', ']'
            return Regex.Replace(name, @"[=\n\t\\""'\[\]]", "").Trim();
        }
    }

    public Tier? GetTier(ItemIndex itemIndex)
    {
        if (_itemConfigByItem.TryGetValue(itemIndex, out ConfigEntry<Tier> configEntry)) 
        { 
            return configEntry.Value; 
        }

        return null;
    }

    public Tier? GetTier(EquipmentIndex equipmentIndex)
    {
        if (_equipmentConfigByItem.TryGetValue(equipmentIndex, out ConfigEntry<Tier> configEntry))
        {
            return configEntry.Value;
        }

        return null;
    }
}
