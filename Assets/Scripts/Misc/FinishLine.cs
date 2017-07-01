using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinishLine : MonoBehaviour
{
    // DEBUG DEBUG DEBUG
    private void OnMouseDown()
    {
        FinishLevel();
    }

    private void FinishLevel()
    {
        GameManager.instance.FinishLevel();
    }
}
