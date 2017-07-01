using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Text))]
public class LevelTime : MonoBehaviour
{
    private Text text;

    private void Awake()
    {
        text = GetComponent<Text>();
    }

    private void Update()
    {
        float levelTime = GameManager.instance.LevelTime;
        text.text = levelTime.ToString("0");

        if(levelTime <= GameManager.instance.TotalLevelTime/10)
        {
            text.color = Color.red;
        }
        else
        {
            text.color = Color.white;
        }
    }
}