using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimeSelection : MonoBehaviour
{
    [SerializeField]
    private Button startLevelButton;

   public void StartDraining(bool addingTimeToLevel)
   {
        GameManager.instance.StartDrainingTime(addingTimeToLevel);
   }

    public void StopDraining()
    {
        GameManager.instance.StopDrainingTime();
    }

    public void StartLevel()
    {
        GameManager.instance.StartLevel();
    }

    private void Update()
    {
        if(GameManager.instance.LevelTime <= 0)
        {
            startLevelButton.interactable = false;
        }
        else
        {
            startLevelButton.interactable = true;
        }
    }
}