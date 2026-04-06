using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DayChanger : MonoBehaviour
{
    public Player player;
    public TMP_Text currentDay;

    public void Awake()
    {
        player = FindObjectOfType<Player>();
    }

    public void Update()
    {
        currentDay.text = "End Day " + GameManager.currentDay;
    }

    public void DayShift()
    {
        Debug.Log("DayShift CALLED");

        SceneManager.LoadScene("EndDayEvent");

        Debug.Log("Day = " + GameManager.currentDay);
    }

    public void MoveToNextDay()
    {
        Debug.Log("TownScene CALLED");

        GameManager.currentDay += 1;

        // Switch to end game scene if reach the eight day
        if (GameManager.currentDay >= 8)
        {
            //
        }

        SceneManager.LoadScene("TownScene");

        //GameManager.instance.gameObject.SetActive(false);
        //player.gameObject.SetActive(true);
    }
}
