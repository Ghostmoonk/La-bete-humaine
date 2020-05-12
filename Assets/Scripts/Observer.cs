using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Observer
{
    public abstract void OnComplete();
}

public class ContentDisplayer : Observer
{
    public delegate void DisplayNextContent();

    public DisplayNextContent displayNextContentDelegateFunction;

    public override void OnComplete()
    {
        displayNextContentDelegateFunction?.Invoke();
    }
}
