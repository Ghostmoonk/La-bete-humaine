using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class LoadInfos : MonoBehaviour, IActivatable
{
    [SerializeField] Animator animator;

    public UnityEvent EndLoadInfos;

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
