using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

[Serializable]
public class EventWeight
{
    public WheelEvent eventType;
    public float weight = 1f;
}

public enum WheelEvent
{
    Nothing,
    EventA, EventB, EventC
}

public class EDEManager : MonoBehaviour
{
    public List<EventWeight> events = new List<EventWeight>();

    ProbMenu<WheelEvent> eventMenu = new ProbMenu<WheelEvent>();

    // Variable for 
    public float totalChance = 1.0f;
    public float eventResult;

    public GameObject dialogueContainer;
    public Button choiceOne, choiceTwo, choiceThree;

    public TMP_Text event_NPC_Name;
    public TMP_Text event_NPC_Dialogue;
    public TMP_Text choice_1;
    public TMP_Text choice_2;
    public TMP_Text choice_3;

    // On scene start spin wheel
    void Start()
    {
        BuildMenu();

        WheelEvent result = SpinWheel();

        TriggerEvent(result);
    }

    // Used for testing purposes *Allows for respin of event wheel by pressing R
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            RunWheel();
        }
    }

    // Tester code
    void RunWheel()
    {
        WheelEvent result = SpinWheel();
        TriggerEvent(result);
    }

    void BuildMenu()
    {
        foreach (var e in events)
        {
            eventMenu.Add(e.eventType, e.weight);
        }
    }

    WheelEvent SpinWheel()
    {
        float r = Random.value * eventMenu.WeightSum;

        float cumulative = 0f;

        for(int i = 0; i < eventMenu.Count; i++)
        {
            cumulative += eventMenu[i].weight;

            if (r <= cumulative)
                return eventMenu[i].payload;
        }

        return eventMenu[eventMenu.Count - 1].payload;
    }

    void TriggerEvent(WheelEvent e)
    {
        switch (e)
        {
            case WheelEvent.Nothing:
                Debug.Log("Nothing happened. Next day.");
                dialogueContainer.SetActive(false);
                break;

            case WheelEvent.EventA:
                Debug.Log("Trigger Event A");
                TriggerEventA();
                break;

            case WheelEvent.EventB:
                Debug.Log("Trigger Event B");
                TriggerEventB();
                break;

            case WheelEvent.EventC:
                Debug.Log("Trigger Event C");
                TriggerEventC();
                break;
        }
    }

    void TriggerEventA()
    {
        // spawn enemies, show dialogue, etc
        dialogueContainer.SetActive(true);
        choice_1.text = "A1 Choice - Gain 30 Food";
        choiceOne.onClick.AddListener(EventAddFoodResource);
        choice_2.text = "A2 Choice";
        choice_3.text = "A3 Choice - Skip";

    }

    void TriggerEventB()
    {
        // different event logic
        dialogueContainer.SetActive(true);
        choice_1.text = "B1 Choice - Gain 30 Energy";
        choiceOne.onClick.AddListener(EventAddEnergyResource);
        choice_2.text = "B2 Choice";
        choice_3.text = "B3 Choice  - Skip";
    }

    void TriggerEventC()
    {
        // another event
        dialogueContainer.SetActive(true);
        choice_1.text = "C1 Choice - Gain 30 Defense";
        choiceOne.onClick.AddListener(EventAddDefenseResource);
        choice_2.text = "C2 Choice";
        choice_3.text = "C3 Choice  - Skip";
    }

    public void EventAddFoodResource()
    {
        TownBehaviour.total_FOOD += 30;
        Debug.Log("Current Food Total: " + TownBehaviour.total_FOOD);
    }

    public void EventAddEnergyResource()
    {
        TownBehaviour.total_ENG += 30;
        Debug.Log("Current Energy Total: " + TownBehaviour.total_ENG);
    }

    public void EventAddDefenseResource()
    {
        TownBehaviour.total_DEF += 30;
        Debug.Log("Current Defense Total: " + TownBehaviour.total_DEF);
    }
}