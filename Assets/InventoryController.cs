using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryController : MonoBehaviour
{
    // NOTE: Whenever using item class has to be written as ItemVer2 as the original (undeveloped) version still exists
    private ItemDictionary itemDictionary;

    public GameObject inventoryPanel;
    public GameObject slotPrefab;
    public int slotCount;
    public GameObject[] itemPrefabs;

    public static InventoryController Instance { get; private set; }
    Dictionary<int, int> itemsCountCache = new();
    public event Action OnInventoryChanged; // event to notify quest system (or any other system that needs to know!)

    private void Awake()
    {
        if(Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        itemDictionary = FindObjectOfType<ItemDictionary>();
        ReBuildItemsCounts();
        //for (int i = 0; i <slotCount; i++)
        //{
        //    Slot slot = Instantiate(slotPrefab, inventoryPanel.transform).GetComponent<Slot>();
        //    if (i < itemPrefabs.Length)
        //    {
        //        GameObject Item = Instantiate(itemPrefabs[i], slot.transform);
        //        Item.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
        //        slot.currentItem = Item;
        //    }
        //}
    }

    public void ReBuildItemsCounts()
    {
        itemsCountCache.Clear();

        foreach (Transform slotTranform in inventoryPanel.transform)
        {
            Slot slot = slotTranform.GetComponent<Slot>();
            if(slot.currentItem != null)
            {
                ItemVer2 item = slot.currentItem.GetComponent<ItemVer2>();
                if(item != null)
                {
                    itemsCountCache[item.ID] = itemsCountCache.GetValueOrDefault(item.ID, 0) + item.quantity;
                }
            }
        }

        OnInventoryChanged?.Invoke();
    }

    public Dictionary<int, int> GetItemCounts() => itemsCountCache;

    public bool AddItem(GameObject itemPrefab)
    {
        ItemVer2 itemToAdd = itemPrefab.GetComponent<ItemVer2>();
        if (itemToAdd == null) return false;

        // Check if we have this item type in inventory
        foreach(Transform slotTranform in inventoryPanel.transform)
        {
            Slot slot = slotTranform.GetComponent<Slot>();
            if(slot != null && slot.currentItem != null)
            {
                ItemVer2 slotItem = slot.currentItem.GetComponent<ItemVer2>();
                if(slotItem != null && slotItem.ID == itemToAdd.ID)
                {
                    // Same item, stack them
                    slotItem.AddToStack();
                    ReBuildItemsCounts();
                    return true;
                }
            }
        }

        // Looks for empty slot
        foreach (Transform slotTranform in inventoryPanel.transform)
        {
            Slot slot = slotTranform.GetComponent<Slot>();
            if (slot != null && slot.currentItem == null)
            {
                GameObject newItem = Instantiate(itemPrefab, slotTranform);
                newItem.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
                slot.currentItem = newItem;
                ReBuildItemsCounts();
                return true;
            }
        }

        Debug.Log("Inventory is full!");
        return false;
    }

    public List<InventorySaveData> GetInventoryItems()
    {
        List<InventorySaveData> invData = new List<InventorySaveData>();
        foreach(Transform slotTranform in inventoryPanel.transform)
        {
            Slot slot = slotTranform.GetComponent<Slot>();
            if(slot.currentItem != null)
            {
                ItemVer2 item = slot.currentItem.GetComponent<ItemVer2>();
                invData.Add(new InventorySaveData
                {
                    itemID = item.ID, 
                    slotIndex = slotTranform.GetSiblingIndex(), 
                    quantity = item.quantity
                });
            }
        }
        return invData;
    }

    public void SetInventoryItems(List<InventorySaveData> inventorySaveData)
    {
        // Clear inventory panel - avoid duplicates
        foreach(Transform child in inventoryPanel.transform)
        {
            Destroy(child.gameObject);
        }

        // Create new slots
        for(int i = 0; i < slotCount; i++)
        {
            Instantiate(slotPrefab, inventoryPanel.transform);
        }

        // Populate slots with saved items
        foreach(InventorySaveData data in inventorySaveData)
        {
            if(data.slotIndex < slotCount)
            {
                Slot slot = inventoryPanel.transform.GetChild(data.slotIndex).GetComponent<Slot>();
                GameObject itemPrefab = itemDictionary.GetItemPrefab(data.itemID);
                if(itemPrefab != null)
                {
                    GameObject item = Instantiate(itemPrefab, slot.transform);
                    item.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
                    
                    ItemVer2 itemComponent = item.GetComponent<ItemVer2>();
                    if(itemComponent != null && data.quantity > 1)
                    {
                        itemComponent.quantity = data.quantity;
                        itemComponent.UpdateQuantityDisplay();
                    }
                    
                    slot.currentItem = item;
                }
            }
        }

        ReBuildItemsCounts();
    }
}
