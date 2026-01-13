using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RuinInteraction : MonoBehaviour
{
    private bool playerInRange = false;
    private BuildingMenu menu;

    private void Start() => menu = FindFirstObjectByType<BuildingMenu>();

    private void Update()
    {
        if (playerInRange && Input.GetKeyDown(KeyCode.I))
        {
            menu.OpenMenu(this); // Send this ruin to the menu
        }
    }

    // This is now called by the UI button
    public void ConvertRuin(BuildingData selectedData)
    {
        Instantiate(selectedData.prefab, transform.position, transform.rotation);
        Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider other) { if (other.CompareTag("Player")) playerInRange = true; }
    private void OnTriggerExit(Collider other) { if (other.CompareTag("Player")) playerInRange = false; }
}
