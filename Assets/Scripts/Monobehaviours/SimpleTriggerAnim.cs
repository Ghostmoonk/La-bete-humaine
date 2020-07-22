using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Is useless, because I didn't know we already have those functions available as an entry of an UnityEvent
public class SimpleTriggerAnim : MonoBehaviour, IActivatable
{
    Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    public void Trigger(string triggerName)
    {
        animator.SetTrigger(triggerName);
    }

    public void Desactivate()
    {
        gameObject.SetActive(false);
    }

    public void Activate()
    {
        gameObject.SetActive(true);
    }
}
