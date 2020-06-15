﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ContextGroup : MonoBehaviour, IActivatable
{
    Animator animator;
    //[Tooltip("Time before the context text at the beginning of the scene disapear")]
    //[SerializeField] float lifeTime;

    public UnityEvent EndAppearEvent;
    public UnityEvent DisappearEvent;
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    public void DisapearDelay(float delay)
    {
        Invoke(nameof(Disapear), delay);
    }
    private void Disapear()
    {
        animator.SetBool("Active", false);
    }

    public void EndDisapear()
    {
        DisappearEvent?.Invoke();
    }

    public void InvokeEndAppearEvent()
    {
        EndAppearEvent?.Invoke();
    }

    public void Activate()
    {
        gameObject.SetActive(true);
    }

    public void Desactivate()
    {
        gameObject.SetActive(false);
    }

}
