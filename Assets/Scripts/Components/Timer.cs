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
        //active = false;
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
        //While the timer time is inferior of its duration limit
        while (timer < duration)
        {
            //If it's still active
            if (active)
            {
                timer += Time.deltaTime;
                timePassed += Time.deltaTime;
            }
            //If it resets, timer is set to 0
            if (reset)
            {
                reset = false;
                timer = 0f;
            }
            yield return null;
        }
        over = true;
        //Fire the event
        timerEndEvent?.Invoke();
    }
}

//public interface ITimer
//{
//    void SetTimer(float duration);
//    void StartTimer();
//    void ResetTimer();
//}
