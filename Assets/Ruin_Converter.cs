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

   private void HideInfo()
    {
        buildMenuPanel.SetActive(false);
    }

    public void SelectedBuilding(int index)
    {
        switch (index)
        {
            case 0: // Should do nothing
                Debug.Log("Wait for player to select building");
                break;

            case 1: // Will create energy building to replace ruin location
                //Destroy(gameObject);
                Instantiate(energyBuilding, new Vector3Int((int)transform.position.x, (int)transform.position.y, 0), Quaternion.identity);
                Destroy(gameObject);
                menu.Close();
                Debug.Log("making energy building");
                break;
        }
    }
}
