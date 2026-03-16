using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        tileManager = GetComponent<TileManager>();
        uiManager = GetComponent<UI_Manager>();
        townBehaviour = GetComponent<TownBehaviour>();

        player = FindObjectOfType<Player>();
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
