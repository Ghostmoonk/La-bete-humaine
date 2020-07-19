using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

//When the timer is finished, do something
public class Timer : MonoBehaviour/*, ITimer*/
{
    [SerializeField] float duration;
    [HideInInspector] public bool over;
    public bool active;
    bool reset;
    public UnityEvent timerEndEvent;

    float timePassed = 0f;

    public void SetTimer(float _duration)
    {
        duration = _duration;
        over = false;
        active = false;
        Debug.Log("set !" + _duration);
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

    public float GetTimePassed()
    {
        return timePassed;
    }

    IEnumerator DoTimer()
    {
        float timer = 0f;
        while (timer < duration)
        {
            if (active)
            {
                timer += Time.deltaTime;
                timePassed += Time.deltaTime;
            }
            if (reset)
            {
                reset = false;
                timer = 0f;
            }
            yield return null;
        }
        over = true;
        timerEndEvent?.Invoke();
    }
}

//public interface ITimer
//{
//    void SetTimer(float duration);
//    void StartTimer();
//    void ResetTimer();
//}
