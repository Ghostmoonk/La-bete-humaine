using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Timer : MonoBehaviour
{
    [SerializeField] float duration;
    [HideInInspector] public bool over;
    public bool active;
    bool reset;
    public UnityEvent timerEndEvent;

    public void SetTimer(float _duration)
    {
        duration = _duration;
        over = false;
        active = false;
    }

    public void SetTimerActive(bool active)
    {
        if (this.active != active)
            this.active = active;
    }
    public void StartTimer()
    {
        StartCoroutine(DoTimer());
    }

    public void ResetTimer()
    {
        if (over)
            StartTimer();

        else
            reset = true;

    }

    public bool IsActive()
    {
        return active;
    }

    IEnumerator DoTimer()
    {
        float timer = 0f;
        while (timer < duration)
        {
            if (active)
            {
                timer += Time.deltaTime;
            }
            if (reset)
            {
                reset = false;
                timer = 0f;
            }
            yield return null;
        }
        Debug.Log("timer over");
        over = true;
        timerEndEvent?.Invoke();
        //simpleText.Complete();
    }
}
