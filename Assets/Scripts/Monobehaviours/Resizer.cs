using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
}
