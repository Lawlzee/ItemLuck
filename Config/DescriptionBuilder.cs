using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace ItemLuck;

public record Element(
    bool IsSelected,
    string Name,
    float Weigth);

public class DescriptionBuilder
{
    private readonly StringBuilder _sb;

    public DescriptionBuilder(StringBuilder sb)
    {
        _sb = sb;
    }

    public void AppendElements(List<Element> elements)
    {
        float totalWeigth = elements.Sum(x => x.Weigth);
        float defaultPercent = 1f / elements.Count;

        for (int i = 0; i < elements.Count; i++)
        {
            var element = elements[i];

            if (element.IsSelected)
            {
                _sb.Append("<color=\"yellow\">");
            }

            _sb.Append(element.Name);

            if (element.IsSelected)
            {
                _sb.Append("</color>");
            }

            _sb.Append(": ");

            float percent = element.Weigth / totalWeigth;
            float diff = percent - defaultPercent;
            bool isZero = Mathf.Abs(diff) <= 0.001f;

            if (!isZero)
            {
                if (percent > defaultPercent)
                {
                    _sb.Append("<color=\"green\">");
                }
                else
                {
                    _sb.Append("<color=\"red\">");
                }
            } 

            _sb.Append(string.Format("{0:0.##}%", percent * 100));

            if (!isZero)
            {
                _sb.Append("</color>");
                _sb.Append(" (");

                if (diff > 0)
                {
                    _sb.Append('+');
                }

                _sb.Append(string.Format("{0:0.##}%", diff * 100));

                _sb.Append(')');
            }

            _sb.AppendLine();
        }
    }
}
