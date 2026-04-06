using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [Header("Persistent Objects/Data")]
    public GameObject[] persistentObjects; 

    public ItemManager itemManager;
    public TileManager tileManager;
    public UI_Manager uiManager;
    public TownBehaviour townBehaviour;

    public Player player;
    public List <BuildingData> buildingData;
    public static int currentDay = 1;
    public string sceneName;

    private void Awake()
    {
        if(instance != null && instance != this)
        {
            CleanUpAndDestroy();
            //Destroy(this.gameObject);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
            MarkPersistentObjects();
        }

        

        itemManager = GetComponent<ItemManager>();
        tileManager = GetComponent<TileManager>(); // May need to remove
        uiManager = GetComponent<UI_Manager>();
        townBehaviour = GetComponent<TownBehaviour>();

        // Might have to remove along with variable itself and redo the inventory system from scratch
        player = FindObjectOfType<Player>();
    }

    public void Update()
    {
        // Makes player invisible based on current scene and unusuable
        Scene currentScene = SceneManager.GetActiveScene();
        sceneName = currentScene.name;

        if (sceneName == "TownScene")
        {
            for (int i = 0; i < persistentObjects.Length -1; i++)
            {
                persistentObjects[i].SetActive(true);
            }
        }
        else if (sceneName == "EndDayEvent")
        {
            for (int i = 0; i < persistentObjects.Length - 1; i++)
            {
                persistentObjects[i].SetActive(false);
            }
                
        }
    }

    private void MarkPersistentObjects()
    {
        foreach (GameObject obj in persistentObjects) 
        {
            if(obj != null)
            {
                DontDestroyOnLoad(obj);
            }
        }
    }

    private void CleanUpAndDestroy()
    {
        foreach (GameObject obj in persistentObjects)
        {
            Destroy(obj);
        }

        Destroy(gameObject);
    }
}
