using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class LoadInfos : MonoBehaviour, IActivatable
{
    Animator animator;

    public UnityEvent EndLoadInfos;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    public void Activate()
    {
        gameObject.SetActive(true);
    }

    public void Desactivate()
    {
        EndLoadInfos?.Invoke();
        gameObject.SetActive(false);
    }

    public void FadeOut()
    {
        animator.SetBool("Active", false);
    }
}

public interface IActivatable
{
    void Activate();

    void Desactivate();
}
