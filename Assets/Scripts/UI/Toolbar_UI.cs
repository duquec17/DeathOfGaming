using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Toolbar_UI : MonoBehaviour
{
    [SerializeField] private List<Slot_UI> toolbarSlots = new List<Slot_UI>();

    private Slot_UI selectedSlot;

    private void Start()
    {
        SelectSlot(0);
    }

    private void Update()
    {
        CheckAlphaNumericKeys();
    }

    public void SelectSlot(Slot_UI slot)
    {
        SelectSlot(slot.slotID);
    }

    public void SelectSlot(int index)
    {
        if(toolbarSlots.Count == 3)
        {
            if (selectedSlot != null)
            {
                selectedSlot.SetHighlight(false);
            }
            selectedSlot = toolbarSlots[index];
            selectedSlot.SetHighlight(true);
            Debug.Log("Selected Slot: " + selectedSlot.name);

            GameManager.instance.player.inventory.toolbar.SelectSlot(index);


        }
    }

    // Will have player select item that is in matching slot number of tool bar (Ex: Press 1 and select item 1)
    private void CheckAlphaNumericKeys()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            SelectSlot(0);
        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            SelectSlot(1);
        }
        
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            SelectSlot(2);
        }

        // Below repeat with increasing values, needed for additional keys past number 1-3
        
    }
}
