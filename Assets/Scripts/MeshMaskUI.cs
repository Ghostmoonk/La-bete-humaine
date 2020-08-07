using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class MeshMaskUI : MaskableGraphic
{
    [SerializeField] RectTransform maskT;
    [SerializeField] RectTransform rectT;
    [SerializeField] Vector2 offset;
    float adaptativeWidth = 0f;
    Tween tween;

    protected override void Start()
    {
        maskT.sizeDelta = rectT.sizeDelta + offset;
        Debug.Log(rectT.sizeDelta + offset);
    }

    protected override void OnPopulateMesh(VertexHelper vertexHelper)
    {
        vertexHelper.Clear();
        Vector3 vec_00 = new Vector3(-maskT.rect.width / 2, -maskT.rect.height / 2);
        Vector3 vec_01 = new Vector3(-maskT.rect.width / 2 + adaptativeWidth * 2, -maskT.rect.height / 2 - offset.y);
        Vector3 vec_10 = new Vector3(-maskT.rect.width / 2, maskT.rect.height / 2);
        Vector3 vec_11 = new Vector3(-maskT.rect.width / 2 + adaptativeWidth * 2, maskT.rect.height / 2 + offset.y);

        vertexHelper.AddUIVertexQuad(new UIVertex[]
        {
            new UIVertex{position = vec_00, color = Color.white},
            new UIVertex{position = vec_01, color = Color.white},
            new UIVertex{position = vec_11, color = Color.white},
            new UIVertex{position = vec_10, color = Color.white}
        });
    }


    private void Update()
    {
        SetVerticesDirty();
    }

    public void ExtendWidth(float duration)
    {
        maskT.sizeDelta = rectT.sizeDelta + offset;
        tween.Kill();
        tween = DOTween.To(() => adaptativeWidth, x => adaptativeWidth = x, maskT.rect.width, duration);
        Debug.Log(maskT.rect.width);
    }

    public void RetractWidth(float duration)
    {
        tween.Kill();
        tween = DOTween.To(() => adaptativeWidth, x => adaptativeWidth = x, 0f, duration);
    }
}
