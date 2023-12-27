using System;
using System.Collections.Generic;
using System.Text;

namespace ItemLuck;

public interface IUpdatableOption
{
    void UpdateDescription();
    void UpdatePanel();
}

public static class UpdatableOptions
{
    public static List<IUpdatableOption> Instances { get; } = new List<IUpdatableOption>();

    public static void UpdateDescriptions()
    {
        foreach (var option in Instances)
        {
            option.UpdateDescription();
        }
    }
}
