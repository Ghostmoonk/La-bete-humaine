﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Observer
{
    public abstract void OnComplete();
    //public abstract void OnComplete(Content content);
}

public class ContentDisplayer : Observer
{
    public delegate void DisplayNextContent();
    public delegate void DisplaySpecificContent(Content content);

    public delegate void OnContentCompleteDelegate();

    public DisplayNextContent displayNextContentDelegateFunction;
    //public DisplaySpecificContent displayContentDelegateFunction;
    public OnContentCompleteDelegate onContentCompleteDelegate;

    public override void OnComplete()
    {
        onContentCompleteDelegate?.Invoke();
        displayNextContentDelegateFunction?.Invoke();
    }

    //public override void OnComplete(Content content)
    //{
    //    displayContentDelegateFunction?.Invoke(content);
    //    displayContentDelegateFunction = null;
    //}
}
