using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ManualMover : MonoBehaviour
{
    [SerializeField] float transitionDuration;
    [SerializeField] Ease ease;

    Tweener tween;

    [SerializeField] UnityEvent StartMoveEvents;
    [SerializeField] UnityEvent StopMoveEvents;

    float initialX;
    private void Start() => initialX = transform.position.x;

    Transform currentTarget;

    public void MoveY(Transform target)
    {
        if (target != currentTarget)
            if (tween != null)
                if (tween.IsPlaying())
                    tween.Pause();

        currentTarget = target;
        StartMoveEvents?.Invoke();
        tween = transform.DOMove(new Vector2(initialX, target.position.y), transitionDuration).SetEase(ease);
        tween.OnComplete(() => { StopMoveEvents?.Invoke(); });
    }

}
