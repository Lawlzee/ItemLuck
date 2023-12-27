using BepInEx.Configuration;
using RiskOfOptions.Components.Options;
using RiskOfOptions.OptionConfigs;
using RiskOfOptions.Options;
using RoR2;
using RoR2.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace ItemLuck;

public class UpdatableChoiceOption : ChoiceOption, IUpdatableOption
{
    private DropDownController _dropdownController;
    private Func<List<Element>> _getElements;

    public UpdatableChoiceOption(ConfigEntryBase configEntry, Func<List<Element>> getElements)
        : base(configEntry)
    {
        _getElements = getElements;
        UpdatableOptions.Instances.Add(this);
    }

    public override GameObject CreateOptionGameObject(GameObject prefab, Transform parent)
    {
        GameObject button = base.CreateOptionGameObject(prefab, parent);

        _dropdownController = button.GetComponentInChildren<DropDownController>();

        button.GetComponentInChildren<HGButton>().onSelect.AddListener(UpdatePanel);

        return button;
    }

    private string GetCurrentDescription()
    {
        var items = _getElements();

        StringBuilder sb = new StringBuilder();
        DescriptionBuilder descriptionBuilder = new DescriptionBuilder(sb);

        descriptionBuilder.AppendElements(items);

        return sb.ToString();
    }

    public void UpdateDescription()
    {
        string description = GetCurrentDescription();
        SetDescription(description, new BaseOptionConfig());
    }

    public void UpdatePanel()
    {
        var panel = _dropdownController.optionController.GetPanel();

        string description = GetCurrentDescription();
        panel.ModOptionsDescriptionPanel.GetComponentInChildren<HGTextMeshProUGUI>().SetText(description);
    }
}

