using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

/*
 * Purpose: To manage all the behaviour/mechanics that are part of the town
 * building aspects of the game/scene.
 * 
 * Writer: Cristian Duque
 */

public class TownBehaviour : MonoBehaviour
{
    // Variables for updating text
    public TMP_Text def_Text;

    // Variables that track all values once they're calculated
    int total_DEF;
    int total_ENG;
    int total_FOOD;
    int total_REC;

    // Base score for the first Player building
    int segundo_Sol_DEF = 50;
    int segundo_Sol_ENG = 50;
    int segundo_Sol_FOOD = 50;
    int segundo_Sol_REC = 50;

    // Start is called before the first frame update
    void Start()
    {
        // Calculates total def
        def_Calculation();

        // Display current total resources
        print(" " + total_DEF);
        print(" " + total_ENG);
        print(" " + total_FOOD);
        print(" " + total_REC);
    }

    // Update is called once per frame
    void Update()
    {
        def_Text.text = "DEF:  " + total_DEF;
    }

    public void def_Calculation()
    {
        // Calculates total def
        total_DEF = total_DEF + segundo_Sol_DEF;
    }
}
