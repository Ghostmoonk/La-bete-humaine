using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnvironmentTweener : MonoBehaviour
{
    [SerializeField] EnvironmentTweeningGroup[] groups;

    public void ChangeTint()
    {
        for (int i = 0; i < groups.Length; i++)
        {
            StartCoroutine(ChangeTint(groups[i]));
        }
    }

    private IEnumerator ChangeTint(EnvironmentTweeningGroup group)
    {
        yield return new WaitForSeconds(group.delay);
        for (int j = 0; j < group.parents.Length; j++)
        {
            for (int k = 0; k < group.parents[j].transform.childCount; k++)
            {
                if (group.parents[j].transform.GetChild(k).GetComponent<SpriteRenderer>())
                {
                    group.parents[j].transform.GetChild(k).GetComponent<SpriteRenderer>().DOColor(group.color, group.duration);
                }
                else if (group.parents[j].transform.GetChild(k).GetComponent<Image>())
                {
                    group.parents[j].transform.GetChild(k).GetComponent<Image>().DOColor(group.color, group.duration);
                }
            }
        }
        StopCoroutine(ChangeTint(group));
    }
}


[System.Serializable]
public struct EnvironmentTweeningGroup
{
    [Tooltip("Parnet objects containing childs to affect")]
    public GameObject[] parents;
    public Color color;
    public float duration;
    public float delay;
}