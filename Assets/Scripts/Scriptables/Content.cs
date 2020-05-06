using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public abstract class Content
{
    public string name;

    protected virtual void PrintName()
    {
        Debug.Log(name);
    }
}
