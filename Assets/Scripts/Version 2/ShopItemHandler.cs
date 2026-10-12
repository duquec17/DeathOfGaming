using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ShopItemHandler : MonoBehaviour, IPointerClickHandler
{
    private bool isShopItem;
    public Slot originalInventorySlot;

    public void Initialise(bool shopItem) => isShopItem = shopItem;

    public void OnPointerClick(PointerEventData eventdata)
    {
        if(eventdata.button == PointerEventData.InputButton.Right)
        {
            if (isShopItem) BuyItem();
            else SellItem();
        }
    }

    private void BuyItem()
    {
        ItemVer2 item = GetComponent<ItemVer2>();
        ShopSlot slot = GetComponentInParent<ShopSlot>();
        if (!item || !slot) return;

        if(CurrencyController.Instance.GetGold() < slot.itemPrice)
        {
            // message to say not enough gold
            Debug.Log("Not enough gold");
            return;
        }

        GameObject itemPrefab = FindObjectOfType<ItemDictionary>().GetItemPrefab(item.ID);
        if (InventoryController.Instance.AddItem(itemPrefab))
        {
            CurrencyController.Instance.SpendGold(slot.itemPrice);
            ShopController.Instance.RefreshPlayerInventoryDisplay();
            ShopController.Instance.RemoveItemFromShop(item.ID, 1);
        }
        else
        {
            Debug.Log("Inventory full");
            return;
        }
    }

    private void SellItem()
    {
        ItemVer2 item = GetComponent<ItemVer2>();
        ShopSlot slot = GetComponentInParent<ShopSlot>();
        if (!item || !slot || !originalInventorySlot) return;

        ItemVer2 invItem = originalInventorySlot.currentItem?.GetComponent<ItemVer2>();
        if (!invItem) return;

        if (invItem.quantity > 1) invItem.RemoveFromStack(1);
        else
        {
            Destroy(originalInventorySlot.currentItem);
            originalInventorySlot.currentItem = null;
        }

        InventoryController.Instance.ReBuildItemsCounts();
        CurrencyController.Instance.AddGold(slot.itemPrice);
        ShopController.Instance.RefreshPlayerInventoryDisplay();
        ShopController.Instance.AddItemToShop(item.ID, 1);

    }
}
