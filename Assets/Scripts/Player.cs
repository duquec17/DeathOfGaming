using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public InventoryManager inventory;
    private TileManager tileManager;

    private void Start()
    {
        tileManager = GameManager.instance.tileManager;
    }

    private void Awake()
    {
        // Need to change these values to match however many slots are given to the player
        inventory = GetComponent<InventoryManager>();
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space)) 
        {
            if (tileManager != null)
            {
                Vector3Int position = new Vector3Int((int)transform.position.x, (int)transform.position.y, 0);

                string tileName = tileManager.GetTileName(position);

                if (!string.IsNullOrWhiteSpace(tileName))
                {
                    if (tileName == "Interactable" && inventory.toolbar.selectedSlot.itemName == "Pickaxe") // Name needs to match exactly or won't work
                    {
                        tileManager.SetInteracted(position);
                    }
                }
            }
        }
    }

    public void DropItem(Item item)
    {
        Vector2 spawnLocation = transform.position;

        Vector2 spawnOffset = Random.insideUnitCircle * 1.75f;

        Item droppedItem = Instantiate(item, spawnLocation + spawnOffset *2f, 
            Quaternion.identity);

        droppedItem.rb2d.AddForce(spawnOffset * 0.2f, ForceMode2D.Impulse);
    }

    //Overload version that overwrites the past function
    public void DropItem(Item item, int numToDrop)
    {
        for(int i = 0; i < numToDrop; i++)
        {
            DropItem(item);
        }
    }
}
