using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class QuestUI : MonoBehaviour
{
    public Transform questListContent;
    public GameObject questEntryPrefab;
    public GameObject objectiveTextPrefab;

    /* Can be swapped back for test code whenever
    public Quest testQuest;
    public int testQuestAmount;
    private List<QuestProgress> testQuests = new();
    */

    // Start is called before the first frame update
    void Start()
    {
        /* Can be swapped back for test code whenever
        for(int i = 0; i < testQuestAmount; i++)
        {
            testQuests.Add(new QuestProgress(testQuest));
        }
        */

        UpdateQuestUI();
    }

    public void UpdateQuestUI()
    {
        // Destroy existing quest entries
        foreach(Transform child in questListContent)
        {
            Destroy(child.gameObject);
        }

        // Recreates quest entries based on latest info
        foreach(var quest in QuestController.Instance.activateQuests)
        {
            GameObject entry = Instantiate(questEntryPrefab, questListContent);
            TMP_Text questNameText = entry.transform.Find("QuestNameText").GetComponent<TMP_Text>();
            Transform objectiveList = entry.transform.Find("ObjectiveList");

            questNameText.text = quest.quest.name;

            foreach(var objective in quest.objectives)
            {
                GameObject objTextGO = Instantiate(objectiveTextPrefab, objectiveList);
                TMP_Text objText = objTextGO.GetComponent<TMP_Text>();
                objText.text = $"{objective.description} ({objective.currentAmount}/{objective.requiredAmount})"; // Collect 5 minor records (0/5)
            }
        }
    }
}
