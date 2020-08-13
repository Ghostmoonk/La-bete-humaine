using System.Collections;
using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class WeatherUI : MonoBehaviour, INeedToBeSetByScriptable, IHaveTextChanging
{
    [Header("Components")]
    [SerializeField] TextMeshProUGUI reliefText;
    [SerializeField] Image reliefImg;
    [SerializeField] TextMeshProUGUI weatherText;
    [SerializeField] RectTransform weatherPopUp;
    [SerializeField] ToggleMover weatherPopUpToggler;
    [SerializeField] TextMeshProUGUI weatherPopUpTitle;
    [SerializeField] TextMeshProUGUI weatherPopUpText;
    [SerializeField] TextMeshProUGUI weatherPopUpParatext;

    MeteoData currentMeteoData;
    [Header("Animation")]
    [SerializeField] float changeTextSpeed;

    [SerializeField] UnityEvent OnDataSet;


    public void SetData(ScriptableObject data)
    {
        if (weatherPopUpToggler.GetToggler())
        {
            weatherPopUpToggler.MoveBySizeX(weatherPopUp);
            weatherPopUpToggler.EndToggleOff.AddListener(delegate { SetData(data); });
        }
        else
        {
            MeteoData weatherData = (MeteoData)data;
            currentMeteoData = weatherData;
            weatherPopUpTitle.text = weatherData.weatherTitle;
            weatherPopUpText.text = weatherData.weatherText;
            weatherPopUpParatext.text = weatherData.weatherParatext;
            Resizer.ResizeLayout(weatherPopUp);

            OnDataSet?.Invoke();
            weatherPopUpToggler.EndToggleOff.RemoveAllListeners();
            weatherPopUpToggler.MoveBySizeX(weatherPopUp);
        }
    }

    public void SetCurrentReliefIcon(float fadeOutTime)
    {
        if (currentMeteoData.newReliefIcon != null)
        {
            if (reliefImg.sprite != currentMeteoData.newReliefIcon)
            {

                reliefImg.GetComponent<GraphicFader>().EndFadeOutText.AddListener(delegate
                {
                    reliefImg.sprite = currentMeteoData.newReliefIcon;
                    reliefImg.GetComponent<GraphicFader>().EndFadeOutText.RemoveAllListeners();
                });

                reliefImg.GetComponent<GraphicFader>().FadeOut(fadeOutTime);
            }
        }
    }

    public void SetCurrentWeatherText()
    {
        if (currentMeteoData == null)
            return;
        if (reliefText.text != currentMeteoData.newRelief.ToString() && currentMeteoData.newRelief.ToString().Length > 1)
            ChangeText(reliefText, currentMeteoData.newRelief.ToString(), changeTextSpeed);

        if (weatherText.text != currentMeteoData.meteoText && currentMeteoData.meteoText.Length > 1)
            ChangeText(weatherText, currentMeteoData.meteoText, changeTextSpeed);

    }

    public void ChangeText(TextMeshProUGUI textMesh, string newText, float changeTextSpeed = 0)
    {
        StartCoroutine(TextModifier.Instance.ProgressChangeText(textMesh, newText, changeTextSpeed));
    }

}

public interface INeedToBeSetByScriptable
{
    void SetData(ScriptableObject data);
}
