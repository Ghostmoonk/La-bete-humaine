using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    int[] array = new int[] { 19, 10, 7, 3, 1, 0, 2, 7, 6, 1, 4, 0 };

    void Start()
    {
        Debug.Log("taille" + array.Length);
        for (int i = 0; i < array.Length - 1; i++)
        {
            int cur = array[i];
            int swapindex = i;
            for (int k = i + 1; k < array.Length; k++)
            {
                if (array[k] > cur)
                {
                    cur = array[k];
                    swapindex = k;
                }

                //if (array[k] < array[k - 1])
                //{
                //    int temp = array[k - 1];
                //    array[k - 1] = array[k];
                //    array[k] = temp;
                //}
            }
            array[swapindex] = array[i];
            array[i] = cur;
        }
        for (int i = 0; i < array.Length; i++)
        {
            Debug.Log(i + " - " + array[i]);
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
