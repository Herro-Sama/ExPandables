using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeSelection : MonoBehaviour
{
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
}