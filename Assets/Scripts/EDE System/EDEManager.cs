using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EDEManager : MonoBehaviour
{
    // Variable for 
    public float eventMiss = 0.8f;
    public float eventTriggered = 0.2f;
    public float totalChance = 1.0f;
    public float eventResult;


    // On scene start spin wheel
    public void Awake()
    {
        EventWheelSpin();
        if(eventResult <= eventTriggered)
        {
            EventTriggered();
        }
        else
        {
            Debug.Log("Will skip event");
            return;
        }
    }

    public void EventWheelSpin()
    {
        Debug.Log("variables: " + eventMiss + " " + eventTriggered + " " + totalChance);
        eventResult = Random.Range(0.1f, totalChance);
        Debug.Log(eventResult);
    }

    public void EventTriggered()
    {
        Debug.Log("Will run event");
    }
}
