using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ItemDragHandler : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    // Goal: Handles when items are dragged between inventory boxes and when dragged into other space by sending it back to original location

    Transform originalParent;
    CanvasGroup canvasGroup;

    public float minDropDistance = 2f;
    public float maxDropDistance = 3f;

    // Start is called before the first frame update
    void Start()
    {
        canvasGroup = GetComponent<CanvasGroup>();
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

        if(dropSlot != null)
        {
            if(dropSlot.currentItem != null)
            {
                //Slot has an item - causes swapping of items
                dropSlot.currentItem.transform.SetParent(originalSlot.transform);
                originalSlot.currentItem = dropSlot.currentItem;
                dropSlot.currentItem.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;

            }
            else
            {
                originalSlot.currentItem = null;
            }

            // Move Item into drop slot
            transform.SetParent(dropSlot.transform);
            dropSlot.currentItem = gameObject;
        }
        else
        {
            // If where we're dropping is not within the inventory
            if(!IsWithinInventory(eventData.position))
            {
                // Drop our item
                DropItem(originalSlot);
            }
            else
            {
                // Snaps back to original spot
                transform.SetParent(originalParent);
            }   
        }

        GetComponent<RectTransform>().anchoredPosition = Vector2.zero; // Centers item
    }

    bool IsWithinInventory(Vector2 mousePosition)
    {
        RectTransform inventoryRect = originalParent.parent.GetComponent<RectTransform>();
        return RectTransformUtility.RectangleContainsScreenPoint(inventoryRect, mousePosition);
    }

    void DropItem(Slot originalSlot)
    {
        originalSlot.currentItem = null;

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
        dropItem.GetComponent<BounceEffect>().StartBounce();

        // Destroy the UI item
        Destroy(gameObject);
    }
}
