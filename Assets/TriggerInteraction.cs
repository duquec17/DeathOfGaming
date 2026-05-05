using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TriggerInteraction : MonoBehaviour
{
    public GameObject Player;
    public GameObject popUpBox;
    int currentScene = 0;
    bool popOnScreen;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.L) && popOnScreen == true && currentScene == 0)
        {
            SceneManager.LoadScene("StoneCometLevel");
            currentScene = 5;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("Entered Trigger area");
        if (currentScene == 0)
        {
            PopUpStart();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        Debug.Log("Left Trigger area");
        PopUpEnd();
    }

    public void PopUpStart()
    {
        popUpBox.SetActive(true);
        popOnScreen = true;
    }

    public void PopUpEnd()
    {
        popUpBox.SetActive(false);
        popOnScreen = false;
    }
}
