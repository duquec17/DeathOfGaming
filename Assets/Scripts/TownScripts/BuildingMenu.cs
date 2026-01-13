using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BuildingMenu : MonoBehaviour
{
    [SerializeField] private BuildingDatabase database;
    [SerializeField] private GameObject buttonPrefab;
    [SerializeField] private Transform container; // The Layout Group object

    private RuinInteraction activeRuin;

    public void OpenMenu(RuinInteraction ruin)
    {
        activeRuin = ruin;
        gameObject.SetActive(true);
        Cursor.lockState = CursorLockMode.None; // Show mouse

        // Clear old buttons
        foreach (Transform child in container) Destroy(child.gameObject);

        // Generate a button for each building in the database
        foreach (BuildingData building in database.allBuildings)
        {
            GameObject btnObj = Instantiate(buttonPrefab, container);
            btnObj.GetComponentInChildren<TextMeshProUGUI>().text = building.buildingName;

            // Add click listener
            btnObj.GetComponent<Button>().onClick.AddListener(() => SelectBuilding(building));
        }
    }

    private void SelectBuilding(BuildingData data)
    {
        activeRuin.ConvertRuin(data); // Pass choice back to the ruin
        CloseMenu();
    }

    public void CloseMenu()
    {
        gameObject.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
    }
}
