using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

//Supprimable
public class SceneTransitionManager : MonoBehaviour
{
    private static SceneTransitionManager instance;
    public static SceneTransitionManager Instance
    {
        get
        {
            return instance;
        }
    }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this);
        }
    }

    public float transitionTime;

}
