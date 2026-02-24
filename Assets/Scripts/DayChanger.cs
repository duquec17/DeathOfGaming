using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DayChanger : MonoBehaviour
{
    public void DayShift()
    {
        Debug.Log("DayShift CALLED");

        SceneManager.LoadScene("EndDayEvent");
    }
    
    public void MoveToNextDay()
    {
        SceneManager.LoadScene("TownScene");
    }
}
