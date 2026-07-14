using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ItemDragHandler : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerClickHandler
{
    // Goal: Handles when items are dragged between inventory boxes and when dragged into other space by sending it back to original location

    Transform originalParent;
    CanvasGroup canvasGroup;

    public float minDropDistance = 2f;
    public float maxDropDistance = 3f;

    private InventoryController inventoryController;

    // Start is called before the first frame update
    void Start()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        inventoryController = InventoryController.Instance;
        Debug.Log("original Parent: " + originalParent + "canvas group: " + canvasGroup);       
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    public void OnBeginDrag(PointerEventData eventData)
    {
        originalParent = transform.parent; // Save OG canvas parent 
        transform.SetParent(transform.root); // Above other canvas
        canvasGroup.blocksRaycasts = false;
        canvasGroup.alpha = 0.6f; // Becomes semi-transparent during drag
    }

    public void OnDrag(PointerEventData eventData)
    {
        transform.position = eventData.position; // Item follows the mouse
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        canvasGroup.blocksRaycasts = true; // Enables raycasts to interact with UI again
        canvasGroup.alpha = 1.0f; // No longer transparent

        Slot dropSlot = eventData.pointerEnter?.GetComponent<Slot>(); // Slot where item is dropped
        if(dropSlot == null)
        {
            GameObject dropItem = eventData.pointerEnter;
            if(dropItem != null)
            {
                dropSlot = dropItem.GetComponentInParent<Slot>();
            }
        }

        Slot originalSlot = originalParent.GetComponent<Slot>();

        // Prevents bug when placing item back in same slot you grabbed from
        if(dropSlot == originalSlot)
        {
            transform.SetParent(dropSlot.transform);
            GetComponent<RectTransform>().anchoredPosition = Vector2.zero; // Centers item
            return;
        }

        if(dropSlot != null)
        {
            // Is a slot under drop point
            if(dropSlot.currentItem != null)
            {
                ItemVer2 draggedItem = GetComponent<ItemVer2>();
                ItemVer2 targetItem = dropSlot.currentItem.GetComponent<ItemVer2>();

                if (draggedItem.ID == targetItem.ID) 
                {
                    targetItem.AddToStack(draggedItem.quantity);
                    originalSlot.currentItem = null;
                    Destroy(gameObject);
                }
                else
                {
                    //Slot has an item - causes swapping of items
                    dropSlot.currentItem.transform.SetParent(originalSlot.transform);
                    originalSlot.currentItem = dropSlot.currentItem;
                    dropSlot.currentItem.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;

                    transform.SetParent(dropSlot.transform);
                    dropSlot.currentItem = gameObject;
                    GetComponent<RectTransform>().anchoredPosition = Vector2.zero; // Centers item
                }

            }
            else
            {
                originalSlot.currentItem = null;
                transform.SetParent(dropSlot.transform);
                dropSlot.currentItem = gameObject;
                GetComponent<RectTransform>().anchoredPosition = Vector2.zero; // Centers item

            }

        }
        else
        {
            // If where we're dropping is not within the inventory (no slot)
            if(!IsWithinInventory(eventData.position))
            {
                // Drop our item
                DropItem(originalSlot);
            }
            else
            {
                // Snaps back to original spot
                transform.SetParent(originalParent);
                GetComponent<RectTransform>().anchoredPosition = Vector2.zero; // Centers item
            }   
        }
    }

    bool IsWithinInventory(Vector2 mousePosition)
    {
        RectTransform inventoryRect = originalParent.parent.GetComponent<RectTransform>();
        return RectTransformUtility.RectangleContainsScreenPoint(inventoryRect, mousePosition);
    }

    void DropItem(Slot originalSlot)
    {
        ItemVer2 item = GetComponent<ItemVer2>();
        int quantity = item.quantity;

        if (quantity > 1) 
        {
            item.RemoveFromStack();

            transform.SetParent(originalParent);
            GetComponent<RectTransform>().anchoredPosition = Vector2.zero;

            quantity = 1;
        }
        else
        {
            originalSlot.currentItem = null;
        }

        // Find player
        Transform playerTransform = GameObject.FindGameObjectWithTag("Player")?.transform;
        if(playerTransform == null)
        {
            Debug.LogError("Missing 'Player' tag");
            return;
        }

        // Select random drop position
        Vector2 dropOffset = Random.insideUnitCircle.normalized * Random.Range(minDropDistance, maxDropDistance);
        Vector2 dropPosition = (Vector2)playerTransform.position + dropOffset;

        // Instantiate drop item
        GameObject dropItem = Instantiate(gameObject, dropPosition, Quaternion.identity);
        ItemVer2 droppedItem = dropItem.GetComponent<ItemVer2>();
        droppedItem.quantity = 1;
        dropItem.GetComponent<BounceEffect>().StartBounce();

        // Destroy the UI item
        if(quantity <= 1 && originalSlot.currentItem == null)
        {
            Destroy(gameObject);
        }
        
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if(eventData.button == PointerEventData.InputButton.Right) // Press right click
        {
            //Split stack
            SplitStack();
            Debug.Log("Called split stack");
        }
    }

    private void SplitStack()
    {
        ItemVer2 item = GetComponent<ItemVer2>();
        if (item == null || item.quantity <= 1) return;

        int splitAmount = item.quantity / 2;
        if (splitAmount <= 0) return;

        item.RemoveFromStack(splitAmount);
        
        GameObject newItem = item.CloneItem(splitAmount);

        if (inventoryController == null || newItem == null) return;

        foreach(Transform slotTransform in inventoryController.inventoryPanel.transform)
        {
            Slot slot = slotTransform.GetComponent<Slot>();
            if (slot != null && slot.currentItem == null)
            {
                slot.currentItem = newItem;
                newItem.transform.SetParent(slot.transform);
                newItem.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
                return;
            }
        }

        // No empty slot - return to stack
        item.AddToStack(splitAmount);
        Destroy(newItem);

        Debug.Log("Split stack was ran");
    }
}
