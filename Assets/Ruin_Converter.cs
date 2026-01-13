using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.VersionControl;
using UnityEngine;

public class Ruin_Converter : MonoBehaviour
{
    public GameObject buildMenuPanel;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // When building clicked on will display information
    private void OnMouseDown()
    {
        if (buildMenuPanel != null)
        {
            if (!buildMenuPanel.activeSelf)
            {
                buildMenuPanel.SetActive(true);
            }
            else
            {
                buildMenuPanel.SetActive(false);
            }
        }
    }

   private void HideInfo()
    {
        buildMenuPanel.SetActive(false);
    }
}
