using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
public class ContentHolder : MonoBehaviour, IActivatable
{
    [HideInInspector] public Animator animator;

    protected virtual void Start()
    {
        animator = GetComponent<Animator>();
    }

    public void Desactivate() { gameObject.SetActive(false); }

    public void Activate() { gameObject.SetActive(true); }

}
