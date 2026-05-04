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
        Debug.Log("DayShift CALLED "+ GameManager.currentDay);

        SceneManager.LoadScene("EndDayEvent");
    }

    public void MoveToNextDay()
    {
        Debug.Log("TownScene CALLED");

        GameManager.currentDay += 1;

        // Ends game is specified stat is 0 or lower.
        if (TownBehaviour.total_FOOD <= 0)
        {
            Debug.Log("Game over, died from starvation");
            SceneManager.LoadScene("GameOver");
        }
        else if (GameManager.currentDay >= 8 && TownBehaviour.total_REC < 100)
        {
            Debug.Log("Failed to obtain record");
            SceneManager.LoadScene("GameOver");
        }
        else if (GameManager.currentDay >= 8)
        {
            // Switches to Game End/Win if survived til last day
            SceneManager.LoadScene("GameEnd");
        }
        else // Switches back to town whenever ending a day, surviving, and not the last day
            SceneManager.LoadScene("TownScene");
    }
}
