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
    public TMP_Text eng_Text;
    public TMP_Text food_Text;
    public TMP_Text rec_Text;

    // Variables that track all values once they're calculated
    public int total_DEF;
    public int total_ENG;
    public int total_FOOD;
    public int total_REC;

    // Base score for the first Player building
    int segundo_Sol_DEF = 50;
    int segundo_Sol_ENG = 50;
    int segundo_Sol_FOOD = 50;
    int segundo_Sol_REC = 50;

    // Start is called before the first frame update
    void Start()
    {
        // Display initial value of resources when loading scene
        def_Text.text = "DEF:  " + total_DEF + segundo_Sol_DEF;
        eng_Text.text = "ENG:  " + total_ENG + segundo_Sol_ENG;
        food_Text.text = "FOOD:  " + total_FOOD + segundo_Sol_FOOD;
        rec_Text.text = "REC:  " + total_REC + segundo_Sol_REC;

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
