using System;
using System.Collections;
using System.Collections.Generic;
using TextContent;
using UnityEngine;

public abstract class Observer
{
    public abstract void OnComplete();
}

public class ContentDisplayer : Observer
{
    public delegate void DisplayNextContent();
    public delegate void DisplaySpecificContent(Content content);

    public delegate void OnContentCompleteDelegate();

    public DisplayNextContent displayNextContentDelegateFunction;
    public OnContentCompleteDelegate onContentCompleteDelegate;

    public override void OnComplete()
    {
        onContentCompleteDelegate?.Invoke();
        displayNextContentDelegateFunction?.Invoke();
    }

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

//Observer for fillgaps exercice 
public class FillGapsActivityObserver : Observer
{
    public delegate bool FillGapsDelegate();
    public FillGapsDelegate fillGapsDelegate;
    public delegate void GapSelectedDelegate(TextGap textGap);
    public GapSelectedDelegate gapSelectedDelegate;

    public override void OnComplete() { }

    //Fire when the gap is complete
    public void OnGapComplete()
    {
        fillGapsDelegate?.Invoke();
    }
    //Fire when the gap is selected
    public void OnGapSelected(TextGap textGap)
    {
        gapSelectedDelegate?.Invoke(textGap);
    }
}