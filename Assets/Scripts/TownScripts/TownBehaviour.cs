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

    // Base score for the Player building
    // Variables that track all values once they're calculated
    public static int total_DEF = 25;
    public static int total_ENG = 25;
    public static int total_FOOD = 25;
    public static int total_REC;

    // Start is called before the first frame update
    void Start()
    {
        // Display initial value of resources when loading scene
        /*
        total_DEF = total_DEF + segundo_Sol_DEF;
        total_ENG = total_ENG + segundo_Sol_ENG;
        total_FOOD = total_FOOD + segundo_Sol_FOOD;
        total_REC = total_REC + segundo_Sol_REC;
        */

        def_Text.text = "DEF:  " + total_DEF;
        eng_Text.text = "ENG:  " + total_ENG;
        food_Text.text = "FOOD:  " + total_FOOD;
        rec_Text.text = "REC:  " + total_REC;
    }

    private void Update()
    {
        def_Text.text = "DEF:  " + total_DEF;
        eng_Text.text = "ENG:  " + total_ENG;
        food_Text.text = "FOOD:  " + total_FOOD;
        rec_Text.text = "REC:  " + total_REC;
    }

    // Adds resource listed in buildresource prefab of each building
    public void AddResources(int eng, int def, int food, int rec)
    {
        total_ENG += eng;
        total_DEF += def;
        total_FOOD += food;
        total_REC += rec;

        UpdateUI();
    }

    private void UpdateUI()
    {
        eng_Text.text = "ENG: " + total_ENG;
        def_Text.text = "DEF: " + total_DEF;
        food_Text.text = "FOOD: " + total_FOOD;
        rec_Text.text = "REC: " + total_REC;
    }
}
