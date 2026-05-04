using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.VersionControl;
using UnityEngine;
using UnityEngine.UIElements;

public class Ruin_Converter : MonoBehaviour
{

    public GameObject buildMenuPanel;
    [SerializeField] private BuildMenuController menu;

    [SerializeField] private Transform parentObjectTransform;

    [SerializeField] private GameObject cloneBuilding;
    [SerializeField] private GameObject energyBuilding;
    [SerializeField] private GameObject foodBuilding;
    [SerializeField] private GameObject defenseBuilding;
    [SerializeField] private GameObject recordBuilding;

    // Start is called before the first frame update
    void Start()
    {
        
        //menu = GetComponent<BuildMenuController>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // When building clicked on will display information
    private void OnMouseDown()
    {
        // Opens menu
        menu.Open(this);
        /*if (buildMenuPanel != null)
        {
            if (!buildMenuPanel.activeSelf)
            {
                buildMenuPanel.SetActive(true);
            }
            else
            {
                buildMenuPanel.SetActive(false);
            }
        }
        */
    }

    // Causes build menu to disappear after done using it
    private void HideInfo()
    {
        buildMenuPanel.SetActive(false);
    }

    public void SelectedBuilding(int index)
    {
        switch (index)
        {
            case 0: // Should do nothing i.e. Select building button
                Debug.Log("Wait for player to select building");
                break;

            case 1: // Will create ENERGY building to replace ruin location costs Food
                if (TownBehaviour.total_FOOD >= 25)
                {
                    TownBehaviour.total_FOOD -= 25;
                    cloneBuilding = Instantiate(energyBuilding, new Vector3Int((int)transform.position.x, (int)transform.position.y, 0), Quaternion.identity);
                    Destroy(gameObject);
                    cloneBuilding.transform.parent = parentObjectTransform; // Places the new building into the container so it's saved
                }
                
                menu.Close();
                Debug.Log("making energy building");
                break;
            
            case 2: // Will create defense building to destroy ruin location costs ENG
                
                if (TownBehaviour.total_ENG >= 25)
                {
                    TownBehaviour.total_ENG -= 25;
                    cloneBuilding = Instantiate(defenseBuilding, new Vector3Int((int)transform.position.x, (int)transform.position.y, 0), Quaternion.identity);
                    Destroy(gameObject);
                    cloneBuilding.transform.parent = parentObjectTransform;
                    
                }
                menu.Close();
                Debug.Log("making defense building");
                break;

            case 3: // Will create food building to replace ruin location costs def

                if (TownBehaviour.total_DEF >= 25)
                {
                    TownBehaviour.total_DEF -= 25;
                    cloneBuilding = Instantiate(foodBuilding, new Vector3Int((int)transform.position.x, (int)transform.position.y, 0), Quaternion.identity);
                    Destroy(gameObject);
                    cloneBuilding.transform.parent = parentObjectTransform;
                }
                menu.Close();
                Debug.Log("making food building");
                break;

            case 4: // Will create record building to replace ruin location costs all three resources
                
                if (TownBehaviour.total_ENG >= 25 && TownBehaviour.total_DEF >= 25 && TownBehaviour.total_FOOD >= 25)
                {
                    // Record building payment cost
                    TownBehaviour.total_DEF -= 25;
                    TownBehaviour.total_FOOD -= 25;
                    TownBehaviour.total_ENG -= 25;

                    cloneBuilding = Instantiate(recordBuilding, new Vector3Int((int)transform.position.x, (int)transform.position.y, 0), Quaternion.identity);
                    Destroy(gameObject);
                    cloneBuilding.transform.parent = parentObjectTransform;
                }
                
                menu.Close();
                Debug.Log("making record building");
                break;
        }
    }
}
