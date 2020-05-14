using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Timer : MonoBehaviour
{
    float duration;
    [HideInInspector] public bool over;
    bool active;
    [HideInInspector] public UnityEvent timerEndEvent;

    public void SetTimer(float _duration)
    {
        duration = _duration;
        over = false;
        active = false;
    }

    public void SetTimerActive(bool active)
    {
        this.active = active;
    }
    public void StartTimer()
    {
        StartCoroutine(DoTimer());
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
            Debug.Log(timer);
            if (active)
            {
                timer += Time.deltaTime;
            }
            yield return null;
        }
        over = true;
        timerEndEvent?.Invoke();
        //simpleText.Complete();
    }
}
