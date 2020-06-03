using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;
using UnityEngine.UI;

public class ImageSlider : MonoBehaviour
{
    RectTransform containerTransform;
    [SerializeField] RectTransform imageTransform;
    [SerializeField] RectTransform frontTransform;
    [SerializeField] TextMeshProUGUI captionText;

    [HideInInspector] public bool active;

    private void Start()
    {
        active = false;
        containerTransform = GetComponent<RectTransform>();
    }

    public void ShowImage(Sprite sprite, float yAlignment, string caption = "")
    {
        ChangeImage(sprite);
        ChangeCaption(caption);
        Resize(sprite.rect.size);
        VerticalAlign(yAlignment);

        SlideOut();
    }

    private void ChangeImage(Sprite sprite)
    {
        imageTransform.GetComponent<Image>().sprite = sprite;
    }

    private void ChangeCaption(string source)
    {
        captionText.text = source;
    }

    private void VerticalAlign(float y)
    {
        containerTransform.position = new Vector3(containerTransform.position.x, y, containerTransform.transform.position.z);

        float frontMaxY = frontTransform.TransformPoint(new Vector3(frontTransform.position.x, frontTransform.position.y + frontTransform.sizeDelta.y / 2, frontTransform.position.z)).y;
        float frontMinY = frontTransform.TransformPoint(new Vector3(frontTransform.position.x, frontTransform.position.y - frontTransform.sizeDelta.y / 2, frontTransform.position.z)).y;

        //Il se peut que la taille du container prenne quelques frames à ajouter celle de son contenu à sizeDelta
        float containerSizeY;
        if (containerTransform.sizeDelta.y < imageTransform.sizeDelta.y)
            containerSizeY = containerTransform.sizeDelta.y + imageTransform.sizeDelta.y;
        else
            containerSizeY = containerTransform.sizeDelta.y;

        Vector3 containerMax = containerTransform.TransformPoint(new Vector3(containerTransform.position.x, containerTransform.position.y + containerSizeY / 2, containerTransform.position.z));
        Vector3 containerMin = containerTransform.TransformPoint(new Vector3(containerTransform.position.x, containerTransform.position.y - containerSizeY / 2, containerTransform.position.z));

        if (containerMax.y > frontMaxY)
        {
            containerTransform.position -= new Vector3(0, (containerMax.y - frontMaxY));

        }
        else if (containerMin.y < frontMinY)
        {
            containerTransform.position += new Vector3(0, (frontMinY - containerMin.y) / 2);
        }
    }

    private void Resize(Vector2 spriteSize)
    {
        imageTransform.GetComponent<RectTransform>().sizeDelta = spriteSize;
    }

    private void SlideOut()
    {
        containerTransform.anchoredPosition = new Vector3(frontTransform.anchorMin.x + imageTransform.sizeDelta.x / 2, containerTransform.anchoredPosition.y);
        containerTransform.DOKill();
        containerTransform.DOAnchorPosX(frontTransform.anchorMin.x - imageTransform.sizeDelta.x / 2, 2f);
        active = true;

        SoundManager.Instance.PlaySound(GetComponentInParent<AudioSource>(), "toggle-slider-in");
    }

    public void SlideIn()
    {
        containerTransform.DOKill();
        containerTransform.DOAnchorPosX(frontTransform.anchorMin.x + containerTransform.sizeDelta.x / 2, 2f);
        active = false;

        SoundManager.Instance.PlaySound(GetComponentInParent<AudioSource>(), "toggle-slider-in");
    }

}
