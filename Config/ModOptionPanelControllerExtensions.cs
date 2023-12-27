using RiskOfOptions.Components.Panel;
using RiskOfOptions.Components.RuntimePrefabs;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace ItemLuck;

public static class ModOptionPanelControllerExtensions
{
    private static readonly FieldInfo _panelField = typeof(ModOptionPanelController).GetField("_panel", BindingFlags.NonPublic | BindingFlags.Instance);

    public static ModOptionsPanelPrefab GetPanel(this ModOptionPanelController controller)
    {
        return (ModOptionsPanelPrefab)_panelField.GetValue(controller);
    }
}
