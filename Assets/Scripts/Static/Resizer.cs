using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public static class Resizer
{
    //Resize la hauteur par rapport à tous les enfants (Pallie un bug Unity)
    public static void ResizeHeight(RectTransform rectT)
    {
        float newHeight = 0;

        foreach (RectTransform rect in rectT.transform.GetComponentsInChildren<RectTransform>())
        {
            newHeight += rect.sizeDelta.y;
        }
        rectT.sizeDelta = new Vector2(rectT.sizeDelta.x, newHeight);
    }

    public static void ResizeLayout(RectTransform parent)
    {
        foreach (RectTransform rect in parent.GetComponentsInChildren<RectTransform>())
        {
            LayoutRebuilder.ForceRebuildLayoutImmediate(rect);
        }
    }
}
