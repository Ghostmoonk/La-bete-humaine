using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpriteChanger : MonoBehaviour
{
    [SerializeField] SpriteSwaps[] spriteSwapSets;

    public void ChangeSprite(int setId)
    {
        for (int i = 0; i < spriteSwapSets[setId].spriteSwap.Length; i++)
        {
            Debug.Log(spriteSwapSets[setId].spriteSwap[i].obj);
            if (spriteSwapSets[setId].spriteSwap[i].obj.GetComponent<Image>())
                spriteSwapSets[setId].spriteSwap[i].obj.GetComponent<Image>().sprite = spriteSwapSets[setId].spriteSwap[i].sprite;
            else if (spriteSwapSets[setId].spriteSwap[i].obj.GetComponent<SpriteRenderer>())
                spriteSwapSets[setId].spriteSwap[i].obj.GetComponent<SpriteRenderer>().sprite = spriteSwapSets[setId].spriteSwap[i].sprite;

        }
    }
}

[System.Serializable]
public struct SpriteSwaps
{
    public SpriteSwap[] spriteSwap;
    [System.Serializable]
    public struct SpriteSwap
    {
        public GameObject obj;
        public Sprite sprite;
    }
}