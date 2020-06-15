using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ManualMover : MonoBehaviour, IIndependantTween
{
    [SerializeField] float transitionDuration;
    [SerializeField] bool isUnityTimeScaleInDependant;
    [SerializeField] Ease ease;

    Tweener tween;
    bool allowed;
    [SerializeField] UnityEvent StartMoveEvents;
    [SerializeField] UnityEvent StopMoveEvents;

    float initialX;
    private void Start() => ResetInitialX();

    Transform currentTarget;

    private void OnEnable()
    {
        allowed = true;
    }

    public void MoveY(Transform target)
    {
        if (allowed)
        {
            if (target != currentTarget)
                if (tween != null)
                    if (tween.IsPlaying())
                        tween.Pause();

            currentTarget = target;
            StartMoveEvents?.Invoke();
            tween = transform.DOMove(new Vector3(initialX, target.position.y, target.position.z), transitionDuration).SetEase(ease).SetUpdate(isUnityTimeScaleInDependant);
            tween.OnComplete(() => { StopMoveEvents?.Invoke(); });
        }
    }

    public void ResetInitialX()
    {
        Debug.Log(transform.position.x);
        initialX = transform.position.x;
    }

    public void UpdateAllowToMove(bool allowed)
    {
        this.allowed = allowed;
        tween.Kill();
    }

    public void SetIndependantTween(bool IsIndependant)
    {
        isUnityTimeScaleInDependant = IsIndependant;
    }

}
