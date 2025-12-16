using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Item))]
public class Collectable : MonoBehaviour
{
    /* - Player wwalks into collectable
       - addd collectable to player, counter, or inventory
       - Delete collectable from the screen
    */

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Player player = collision.gameObject.GetComponent<Player>();

        if (player)
        {
            Item item = GetComponent<Item>();

            if (item != null)
            {
                player.inventory.Add("Backpack", item);
                Destroy(this.gameObject);
            }
        }
    }
}
