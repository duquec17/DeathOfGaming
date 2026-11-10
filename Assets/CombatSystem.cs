using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public enum CombatState { START, PLAYER_PHASE, ENEMY_PHASE, EXECUTION_PHASE, WON, LOST}

public class CombatSystem : MonoBehaviour
{
    public CombatState currentState;

    [Header("Spawn Battle Stations")]
    public Transform[] playerSpawnPoints;
    public Transform[] enemySpawnPoints;

    [Header("Team Prefabs (Data Sources")]
    public List<GameObject> playerPrefabs;
    public List<GameObject> enemyPrefabs;

    [Header("RunTime Active Teams")]
    [HideInInspector] public List<Unit> playerTeam = new List<Unit>();
    [HideInInspector] public List<Unit> enemyTeam = new List<Unit>();

    // Character Team Names
    public TMP_Text[] dialogueText;

    // Used to know where to spawn units
    public Transform playerBattleStation;
    public Transform enemyBattleStation;

    

    // Start is called before the first frame update
    void Start()
    {
        currentState = CombatState.START;
        StartCoroutine(SetupBattle());
    }

    IEnumerator SetupBattle()
    {
        // Remove old data
        playerTeam.Clear();
        enemyTeam.Clear();

        // Spawn Player Units at locations
        for (int i = 0; i < playerPrefabs.Count; i++) 
        {
            if (i >= playerSpawnPoints.Length) break;

            GameObject spawnedUnit = Instantiate(playerPrefabs[i], playerSpawnPoints[i].position, Quaternion.identity);

            Unit unitInfo = spawnedUnit.GetComponent<Unit>();

            if (unitInfo != null)
            {
                playerTeam.Add(unitInfo);
            }

            dialogueText[i].text = unitInfo.unitName;
        }

        // Spawn Enemy Units at locations
        for (int i = 0; i < enemyPrefabs.Count; i++)
        {
            if (i >= enemySpawnPoints.Length) break;

            GameObject spawnedUnit = Instantiate(enemyPrefabs[i], enemySpawnPoints[i].position, Quaternion.identity);

            Unit unitInfo = spawnedUnit.GetComponent<Unit>();

            if (unitInfo != null)
            {
                enemyTeam.Add(unitInfo);
            }
        }

        yield return new WaitForSeconds(1f);
        StartRoundSelection();
    }

    // ======================
    // Player Selection Phase
    // ======================

    void StartRoundSelection()
    {

    }
}
