using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContextGroup : MonoBehaviour
{
    Animator animator;
    [Tooltip("Time before the context text at the beginning of the scene disapear")]
    [SerializeField] float lifeTime;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        Invoke(nameof(Disapear), lifeTime);
    }

    private void Disapear()
    {
        animator.SetBool("Active", false);
    }
}
