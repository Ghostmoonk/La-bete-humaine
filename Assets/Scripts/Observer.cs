using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Observer
{
    public abstract void OnNotify();
    public abstract void OnComplete();
}

public class ContentDisplayer : Observer
{
    public delegate void DisplayNextContent();

    public DisplayNextContent disPlayNextContentDelegateFunction;

    public override void OnNotify()
    {
        //disPlayNextContentDelegateFunction?.Invoke();
    }
    public override void OnComplete()
    {
        disPlayNextContentDelegateFunction?.Invoke();
    }
}
