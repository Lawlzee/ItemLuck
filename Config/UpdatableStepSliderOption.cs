using BepInEx.Configuration;
using RiskOfOptions.Components.Options;
using RiskOfOptions.Components.Panel;
using RiskOfOptions.Components.RuntimePrefabs;
using RiskOfOptions.OptionConfigs;
using RiskOfOptions.Options;
using RoR2;
using RoR2.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using UnityEngine;

namespace ItemLuck;

public class UpdatableStepSliderOption : StepSliderOption, IUpdatableOption
{
    private ModSettingsStepSlider _silder;

    public UpdatableStepSliderOption(ConfigEntry<float> configEntry, StepSliderConfig config)
        : base(configEntry, config)
    {
        UpdatableOptions.Instances.Add(this);
    }

    public override GameObject CreateOptionGameObject(GameObject prefab, Transform parent)
    {
        GameObject button = base.CreateOptionGameObject(prefab, parent);

        _silder = button.GetComponentInChildren<ModSettingsStepSlider>();

        button.GetComponentInChildren<HGButton>().onSelect.AddListener(UpdatePanel);

        return button;
    }

    private string GetCurrentDescription()
    {
        var tiers = TierList.GetValidItems()
            .Select(x => new
            {
                Tier = x.tier,
                Element = new Element(
                    false,
                    Language.GetString(x.nameToken),
                    (ItemLuckPlugin.TierList.GetTier(x.itemIndex) ?? Tier.Unspecified).ApplyItemLuck(1))
            })
            .GroupBy(x => x.Tier)
            .OrderBy(x => x.Key)
            .Select(x => new
            {
                Color = ColorCatalog.GetColor(ItemTierCatalog.GetItemTierDef(x.Key).colorIndex),
                CategoryName = x.Key.GetFullName(),
                Elements = x
                    .Select(x => x.Element)
                    .OrderBy(x => x.Name)
                    .ToList()
            })
            .ToList();

        tiers.Add(new
        {
            Color = ColorCatalog.GetColor(ColorCatalog.ColorIndex.Equipment),
            CategoryName = "Equipments",
            Elements = TierList.GetValidEquipments()
                .Select(x => new Element(
                    false,
                    Language.GetString(x.nameToken),
                    (ItemLuckPlugin.TierList.GetTier(x.equipmentIndex) ?? Tier.Unspecified).ApplyItemLuck(1)))
                .ToList()
        });

        StringBuilder sb = new StringBuilder();
        DescriptionBuilder descriptionBuilder = new DescriptionBuilder(sb);

        foreach (var tier in tiers)
        {
            sb.Append($"<color=#{ColorUtility.ToHtmlStringRGB(tier.Color)}>");
            sb.Append(tier.CategoryName);
            sb.Append(":</color>");
            sb.AppendLine();

            descriptionBuilder.AppendElements(tier.Elements);
            sb.AppendLine();
        }

        return sb.ToString();
    }

    public void UpdateDescription()
    {
        string description = GetCurrentDescription();
        SetDescription(description, new());
    }

    public void UpdatePanel()
    {
        ModOptionsPanelPrefab panel = _silder.optionController.GetPanel();

        string description = GetCurrentDescription();
        panel.ModOptionsDescriptionPanel.GetComponentInChildren<HGTextMeshProUGUI>().SetText(description);
    }
}
