using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimePickup : MonoBehaviour
{
    [SerializeField]
    private float time = 10f;

    /*
    [SerializeField]
    private GameObject pickupScreen;
    */

    [SerializeField]
    private Text text;
    
    private void Awake()
    {
        text.text = time.ToString("0");
    }

    // DEBUG DEBUG DEBUG
    private void OnMouseDown()
    {
        Pickup();
    }

    private void Pickup()
    {
        Inventory.instance.AddTimer(time);
        /*
        GameObject newPickupScreen = Instantiate(pickupScreen) as GameObject;
        newPickupScreen.GetComponent<TimePickupScreen>().Activate(time);
        */
        Destroy(gameObject);
    }
}