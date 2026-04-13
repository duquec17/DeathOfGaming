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

    // Int Variable 
    public float totalChance = 1.0f;
    public float eventResult;

    // Determine highest and lowest resource
    int highestRes = Math.Max(TownBehaviour.total_DEF, Math.Max(TownBehaviour.total_ENG, TownBehaviour.total_FOOD));
    int lowestRes = Math.Min(TownBehaviour.total_DEF, Math.Min(TownBehaviour.total_ENG, TownBehaviour.total_FOOD));

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
                event_NPC_Name.text = "A Sunny Day";
                event_NPC_Dialogue.text = "An uneventful, but peaceful day. Should turn for the night.";
                dialogueContainer.SetActive(false);
                break;

            case WheelEvent.EventA:
                event_NPC_Name.text = "A Gardener's Respite";
                event_NPC_Dialogue.text = "Hello there, my name is Mariana. A simple lady that wants to see all flowers";
                TriggerEventA();
                break;

            case WheelEvent.EventB:
                event_NPC_Name.text = "A Thief's Opportunity";
                event_NPC_Dialogue.text = "An uneventful, but peaceful day. Should turn for the night.";
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
        
        choice_2.text = "A2 Choice - Exchange 20 highest resource for 10 lowest resource";
        choiceTwo.onClick.AddListener(ExchangeResource);

        choice_3.text = "A3 Choice - Skip";
        choiceThree.onClick.AddListener(SkipDay);

    }

    void TriggerEventB()
    {
        // different event logic
        dialogueContainer.SetActive(true);
        choice_1.text = "B1 Choice - Gain 30 Energy";
        choiceOne.onClick.AddListener(EventAddEnergyResource);
        
        choice_2.text = "B2 Choice";
        
        choice_3.text = "B3 Choice - Skip";
        choiceThree.onClick.AddListener(SkipDay);
    }

    void TriggerEventC()
    {
        // another event
        dialogueContainer.SetActive(true);
        choice_1.text = "C1 Choice - Gain 30 Defense";
        choiceOne.onClick.AddListener(EventAddDefenseResource);
        
        choice_2.text = "C2 Choice";

        choice_3.text = "C3 Choice - Skip";
        choiceThree.onClick.AddListener(SkipDay);
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

    public void SkipDay()
    {
        dialogueContainer.SetActive(false);
    }

    public void ExchangeResource()
    {
        // Takeaway from highest resource
        if (highestRes == TownBehaviour.total_DEF)
        {
            TownBehaviour.total_DEF = TownBehaviour.total_DEF - 20;
            Debug.Log("Exchanged def");
        } 
        else if(highestRes == TownBehaviour.total_FOOD)
        {
            TownBehaviour.total_FOOD = TownBehaviour.total_FOOD - 20;
            Debug.Log("Exchanged food");
        }
        else if(highestRes == TownBehaviour.total_ENG)
        {
            TownBehaviour.total_ENG = TownBehaviour.total_ENG - 20;
            Debug.Log("Exchanged ENG");
        }

        // Add same value taken from highest resource to lower
        if (lowestRes == TownBehaviour.total_DEF)
        {
            TownBehaviour.total_DEF = TownBehaviour.total_DEF + 10;
            Debug.Log("Exchanged def");
        }
        else if (lowestRes == TownBehaviour.total_FOOD)
        {
            TownBehaviour.total_FOOD = TownBehaviour.total_FOOD + 10;
            Debug.Log("Exchanged food");
        }
        else if (lowestRes == TownBehaviour.total_ENG)
        {
            TownBehaviour.total_ENG = TownBehaviour.total_ENG + 10;
            Debug.Log("Exchanged ENG");

        }


    }


}