using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ItemDragHandler : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    // Goal: Handles when items are dragged between inventory boxes and when dragged into other space by sending it back to original location

    Transform originalParent;
    CanvasGroup canvasGroup;

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
            // No slot under drop point
            transform.SetParent(originalParent);
        }

        GetComponent<RectTransform>().anchoredPosition = Vector2.zero; // Centers item
    }
}
