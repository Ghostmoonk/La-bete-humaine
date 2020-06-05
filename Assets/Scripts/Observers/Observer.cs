using System;
using System.Collections;
using System.Collections.Generic;
using TextContent;
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

public class GlossaryObserver : Observer
{
    public delegate void GlossaryDelegate(Vector3 vec3, GlossaryData data);

    public GlossaryDelegate glossaryDelegate;

    public override void OnComplete()
    {

    }
    public void OnHighligh(Vector3 pos, GlossaryData data)
    {
        glossaryDelegate?.Invoke(pos, data);
    }

}

public class SliderObserver : Observer
{
    public delegate void SliderDelegate(ReferenceImage refImg);

    public SliderDelegate sliderDelegate;

    public override void OnComplete()
    {

    }

    public void OnSlide(ReferenceImage refImg)
    {
        sliderDelegate?.Invoke(refImg);
    }
}