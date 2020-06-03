using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Shaker : MonoBehaviour
{
    public abstract void ContinuousShake();

    public abstract void StopShake();

    public abstract void Shake(float duration);

}

