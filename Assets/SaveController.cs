using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

public class SaveController : MonoBehaviour
{
    private string saveLocation;
    private InventoryController inventoryController;
    private HotbarController hotbarController;
    private Chest[] chests;

    // Start is called before the first frame update
    void Start()
    {
        InitializeComponents();
        LoadGame();
        Debug.Log("File is saving to: " + saveLocation);
    }

    private void InitializeComponents()
    {
        // Defines save location
        saveLocation = Path.Combine(Application.persistentDataPath, "saveData.json");
        inventoryController = FindObjectOfType<InventoryController>();
        hotbarController = FindObjectOfType<HotbarController>();
        chests = FindObjectsOfType<Chest>();
    }

    public void SaveGame()
    {
        SaveData saveData = new SaveData()
        {
            playerPosition = GameObject.FindGameObjectWithTag("Player").transform.position,
            mapBoundary = FindObjectOfType<CinemachineConfiner>().m_BoundingShape2D.gameObject.name,
            inventorySaveData = inventoryController.GetInventoryItems(),
            hotbarSaveData = hotbarController.GetHotbarItems(),
            chestSaveData = GetChestsState(),
            questProgressData = QuestController.Instance.activateQuests,
            handinQuestIDs = QuestController.Instance.handinQuestIDs

            // Need to save individual item that are on map if want to remove
        };

        File.WriteAllText(saveLocation, JsonUtility.ToJson(saveData));
    }

    private List<ChestSaveData> GetChestsState()
    {
        List<ChestSaveData> chestStates = new List<ChestSaveData>();

        foreach(Chest chest in chests)
        {
            ChestSaveData chestSaveData = new ChestSaveData
            {
                chestID = chest.ChestID,
                isOpened = chest.IsOpened
            };
            chestStates.Add(chestSaveData);
        }

        return chestStates;
    }

    public void LoadGame()
    {
        if (File.Exists(saveLocation))
        {
            SaveData saveData = JsonUtility.FromJson<SaveData>(File.ReadAllText(saveLocation));

            GameObject.FindGameObjectWithTag("Player").transform.position = saveData.playerPosition;

            PolygonCollider2D savedMapBoundary = GameObject.Find(saveData.mapBoundary).GetComponent<PolygonCollider2D>();
            FindObjectOfType<CinemachineConfiner>().m_BoundingShape2D = savedMapBoundary;

            MapController_Manual.Instance?.HighlightArea(saveData.mapBoundary);
            MapController_Dynamic.Instance?.GenerateMap(savedMapBoundary);

            inventoryController.SetInventoryItems(saveData.inventorySaveData);
            hotbarController.SetHotbarItems(saveData.hotbarSaveData);

            //loadcheststate
            LoadChestStates(saveData.chestSaveData);

            // Need to load final state of individual items that are on map if I want to not have respawn on next load/play

            QuestController.Instance.LoadQuestProgress(saveData.questProgressData);
            QuestController.Instance.handinQuestIDs = saveData.handinQuestIDs;
        }
        else
        {
            SaveGame();

            inventoryController.SetInventoryItems(new List<InventorySaveData>());
            hotbarController.SetHotbarItems(new List<InventorySaveData>());

            MapController_Dynamic.Instance?.GenerateMap();
        }
    }

    private void LoadChestStates(List<ChestSaveData> chestStates)
    {
        foreach(Chest chest in chests)
        {
            ChestSaveData chestSaveData = chestStates.FirstOrDefault(c => c.chestID == chest.ChestID);

            if (chestSaveData != null)
            {
                chest.SetOpened(chestSaveData.isOpened);
            }
        }
    }
}
