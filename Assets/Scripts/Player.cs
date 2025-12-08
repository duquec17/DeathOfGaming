using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public Inventory inventory;

    private void Awake()
    {
        inventory = new Inventory(21);
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space)) 
        {
            Vector3Int position = new Vector3Int((int)transform.position.x, 
                (int)transform.position.y, 0);

            if (GameManager.instance.tileManager.isInteractable(position))
            {
                Debug.Log("Tile is interactable");
                GameManager.instance.tileManager.SetInteracted(position);
            }
        }
    }

    public void DropItem(Collectable item)
    {
        Vector2 spawnLocation = transform.position;

        Vector2 spawnOffset = Random.insideUnitCircle * 1.75f;

        Collectable droppedItem = Instantiate(item, spawnLocation + spawnOffset, 
            Quaternion.identity);

        droppedItem.rb2d.AddForce(spawnOffset * 0.2f, ForceMode2D.Impulse);
    }
}
