using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class ImageHolder : ContentHolder
{
    [HideInInspector] public SimpleImage simpleImage;
    [SerializeField] TextMeshProUGUI paratextMesh;
    [SerializeField] Image image;
    Timer lectureTimeTimer;


    private void OnEnable()
    {
        Sprite spriteToLoad = Resources.Load<Sprite>("images/" + simpleImage.imgData.imagePath.Split('.')[0]);
        image.sprite = spriteToLoad;
        float ratio = image.rectTransform.sizeDelta.x / spriteToLoad.rect.width;
        image.rectTransform.sizeDelta = new Vector2(image.rectTransform.sizeDelta.x, spriteToLoad.rect.height * ratio);

        if (simpleImage.imgData.paratext.Length > 0)
            paratextMesh.text = simpleImage.imgData.paratext;
        else
            paratextMesh.gameObject.SetActive(false);
    }

    protected override void Start()
    {
        base.Start();
        lectureTimeTimer = GetComponent<Timer>();

        lectureTimeTimer.SetTimer(simpleImage.imgData.minimumReadTime);
        lectureTimeTimer.timerEndEvent.AddListener(simpleImage.Complete);
        lectureTimeTimer.StartTimer();

    }

    //À factoriser avec TextHolder
    void Update()
    {
        if (!FindObjectOfType<ContentsSupport>().IsOnScreen() && !lectureTimeTimer.over)
        {
            lectureTimeTimer.SetTimerActive(false);
        }
        else if (FindObjectOfType<ContentsSupport>().IsOnScreen() && !lectureTimeTimer.over && !lectureTimeTimer.IsActive())
        {
            lectureTimeTimer.SetTimerActive(true);
        }
    }
}
