using System.Collections;
using System.Collections.Generic;
using TextContent;
using UnityEngine;

public class SimpleImage : Content
{
    public ImageData imgData;

    public SimpleImage(ImageData data)
    {
        imgData = data;
    }
}
