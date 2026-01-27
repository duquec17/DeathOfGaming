using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class BuildMenuController : MonoBehaviour
{
    [SerializeField] private TMP_Dropdown dropDown;

    // Meant to be filled when you click on the ruin
    private Ruin_Converter activeRuin;

    private void Awake()
    {
        dropDown.onValueChanged.RemoveAllListeners();
        dropDown.onValueChanged.AddListener(OnDropDownChanged);
    }

    public void Open(Ruin_Converter ruin)
    {
        activeRuin = ruin;
        dropDown.value = 0; // Resets back to select building
        gameObject.SetActive(true);
    }

    public void Close()
    {
        activeRuin = null;
        gameObject.SetActive(false);
    }

    private void OnDropDownChanged(int index)
    {
        if(activeRuin == null)
        {
            Debug.Log("no active ruin");
            return;
        }

        activeRuin.SelectedBuilding(index);
    }
}
