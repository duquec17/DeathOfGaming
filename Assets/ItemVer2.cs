using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemVer2 : MonoBehaviour
{
    public int ID;
    public string Name;
    public int quantity = 1;

    private TMP_Text quantityText;

    // Shop fields
    public int buyPrice = 10; // Buy from shop
    [Range(0f, 1f)]
    public float sellPriceMultiplier = 0.5f; // Sell for 50% buy price

    private void Awake()
    {
        quantityText = GetComponentInChildren<TMP_Text>();
        UpdateQuantityDisplay();
    }

    public int GetSellPrice()
    {
        return Mathf.RoundToInt(buyPrice * sellPriceMultiplier);
    }

    public void UpdateQuantityDisplay()
    {
        if(quantityText != null)
        {
            quantityText.text = quantity > 1 ? quantity.ToString(): "";
        }
        
    }

    public void AddToStack(int amount = 1)
    {
        quantity += amount;
        UpdateQuantityDisplay();
    }

    public int RemoveFromStack(int amount = 1)
    {
        int removed = Mathf.Min(amount, quantity);
        quantity -= removed;
        UpdateQuantityDisplay();
        return removed;
    }

    public GameObject CloneItem(int newQuantity)
    {
        GameObject clone = Instantiate(gameObject);
        ItemVer2 cloneItem = clone.GetComponent<ItemVer2>(); // Remember to use ItemVer2 and replacement for any coding that would normally use Item script
        cloneItem.quantity = newQuantity;
        cloneItem.UpdateQuantityDisplay();
        return clone;
    }

    public virtual void UseItem()
    {
        Debug.Log("Using item " + Name);
    }

    public virtual void ShowPopUp()
    {
        Sprite itemIcon = GetComponent<Image>().sprite;
        if(ItemPickupUIController.Instance != null)
        {
            ItemPickupUIController.Instance.ShowItemPickup(Name, itemIcon);
        }
    }
}
