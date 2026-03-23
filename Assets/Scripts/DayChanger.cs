using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DayChanger : MonoBehaviour
{
    public Player player;

    public void Awake()
    {
        player = FindObjectOfType<Player>();
    }

    public void Update()
    {
        
    }

    public void DayShift()
    {
        Debug.Log("DayShift CALLED");

        SceneManager.LoadScene("EndDayEvent");
    }
    
    public void MoveToNextDay()
    {
        Debug.Log("TownScene CALLED");

        SceneManager.LoadScene("TownScene");

        //GameManager.instance.gameObject.SetActive(false);
        //player.gameObject.SetActive(true);
    }
}
