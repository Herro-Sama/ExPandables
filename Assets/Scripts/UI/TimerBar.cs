using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class TimerBar : MonoBehaviour
{
    private Image image;

    private void Awake()
    {
        image = GetComponent<Image>();
    }

    private void Update()
    {
        float fill =  GameManager.instance.LevelTime / GameManager.instance.TotalLevelTime;
        image.fillAmount = fill;

        if (fill <= 0.5f)
        {
            image.color = Color.Lerp(Color.yellow, Color.red, ((1 - (fill*2))));
        }
        else
        {
            image.color = Color.Lerp(Color.yellow, Color.green, (fill*2)-1);
        }      
    }
}