using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MeshMaskUI : MaskableGraphic
{
    [SerializeField] RectTransform rectT;
    float adaptativeWidth = 0f;
    Tween tween;

    protected override void OnPopulateMesh(VertexHelper vertexHelper)
    {
        vertexHelper.Clear();
        Vector3 vec_00 = new Vector3(-rectT.rect.width / 2, -rectT.rect.height / 2);
        Vector3 vec_01 = new Vector3(-rectT.rect.width / 2 + adaptativeWidth, -rectT.rect.height / 2);
        Vector3 vec_10 = new Vector3(-rectT.rect.width / 2, rectT.rect.height / 2);
        Vector3 vec_11 = new Vector3(-rectT.rect.width / 2 + adaptativeWidth, rectT.rect.height / 2);

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
        tween.Kill();
        tween = DOTween.To(() => adaptativeWidth, x => adaptativeWidth = x, rectT.rect.width, duration);
    }

    public void RetractWidth(float duration)
    {
        tween.Kill();
        tween = DOTween.To(() => adaptativeWidth, x => adaptativeWidth = x, 0f, duration);
    }
}
