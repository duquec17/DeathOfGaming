using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildResource : MonoBehaviour
{
    // Variables for buildings to increase 
    public int energyIncrease;
    public int defenseIncrease;
    public int foodIncrease;
    public int recordIncrease;

    // Variable to access all script functions connected to Gamemanager
    private GameManager gmTownBehaviour;

    private void Start()
    {
        gmTownBehaviour = FindObjectOfType<GameManager>();
        if (gmTownBehaviour != null)
        {
            Debug.Log("found gamemanager");
        }
        else
        {
            Debug.Log("no manager");
        }
    }

    private void Update()
    {
        gmTownBehaviour.townBehaviour.total_ENG += energyIncrease;
    }
}
