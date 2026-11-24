using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Linq;

public enum CombatState { START, PLAYER_PHASE, ENEMY_PHASE, EXECUTION_PHASE, WON, LOST}

public class CombatSystem : MonoBehaviour
{
    public CombatState currentState;
    public CombatUI combatUI;

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
    public TMP_Text[] dialogueStats;

    // Used to know where to spawn units
    public Transform playerBattleStation;
    public Transform enemyBattleStation;

    // Track number of player actions
    private List<CombatAction> actionQueue = new List<CombatAction>();
    public int currentPlayerIndex;

    // Start is called before the first frame update
    void Start()
    {
        currentState = CombatState.START;
        StartCoroutine(SetupBattle());
    }

    // Sets up area but units and their info are independent from combat system
    IEnumerator SetupBattle()
    {
        // Remove old data
        playerTeam.Clear();
        enemyTeam.Clear();

        // Spawn Player Units at locations
        for (int i = 0; i < playerPrefabs.Count; i++)
        {
            if (i >= playerSpawnPoints.Length) break;

            // Spawns party member while gettings their stats
            GameObject spawnedUnit = Instantiate(playerPrefabs[i], playerSpawnPoints[i].position, Quaternion.identity);
            Unit unitInfo = spawnedUnit.GetComponent<Unit>();

            if (unitInfo != null)
            {
                playerTeam.Add(unitInfo);
            }

            dialogueText[i].text = unitInfo.unitName;
            dialogueStats[i].text = $"HP: {unitInfo.currentHP}/{unitInfo.maxHP} MP: {unitInfo.currentMP}/{unitInfo.maxMP}";
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

        yield return new WaitForSeconds(2f);

        ChangeState(CombatState.PLAYER_PHASE);
    }

    public void ChangeState(CombatState newState)
    {
        currentState = newState;

        switch (currentState)
        {
            case CombatState.PLAYER_PHASE:
                StartPlayerPhase();
                break;

            case CombatState.ENEMY_PHASE:
                StartEnemyPhase();
                break;

            case CombatState.EXECUTION_PHASE:
                StartCoroutine(ExecutionPhase());
                break;

            case CombatState.WON:
                WonPhase();
                break;

            case CombatState.LOST:
                LostPhase();
                break;
        }
    }

    void StartPlayerPhase()
    {
        actionQueue.Clear();
        currentPlayerIndex = 0;

        // Rest player flages for having acted
        foreach (Unit p in playerTeam) p.ResetTurn();
        foreach (Unit e in enemyTeam) e.ResetTurn();

        NextPlayer();
    }

    void NextPlayer()
    {
        combatUI.actionMenuPanel.SetActive(true);

        while (currentPlayerIndex < playerTeam.Count)
        {
            Unit unit = playerTeam[currentPlayerIndex];

            if (unit != null && !unit.isDead)
                break;

            currentPlayerIndex++;
        }

        if (currentPlayerIndex >= playerTeam.Count)
        {
            combatUI.actionMenuPanel.SetActive(false);
            ChangeState(CombatState.ENEMY_PHASE);
            return;
        }

        Unit activePlayer = playerTeam[currentPlayerIndex];

        Debug.Log($"Player Phase: Select action for {activePlayer.name}");
    }

    public void OnplayerActionChosen(string actionType, Unit target)
    {
        if (currentState != CombatState.PLAYER_PHASE) return;

        if (currentPlayerIndex >= playerTeam.Count) return;

        Unit activePlayer = playerTeam[currentPlayerIndex];

        if (activePlayer == null || activePlayer.isDead || activePlayer.hasActed) return;

        if (target == null || target.isDead) return;

        actionQueue.Add(new CombatAction
        {
            actor = activePlayer,
            target = target,
            actionType = actionType,
            speedPriority = activePlayer.speed
        });
        
        activePlayer.hasActed = true;

        currentPlayerIndex++;
        NextPlayer();
    }

    /// <summary>
    /// End of Player Phase
    /// </summary>

    void StartEnemyPhase()
    {
        List<Unit> livingPlayers = playerTeam.Where(p => !p.isDead).ToList();

        foreach (Unit enemy in enemyTeam)
        {
            if (enemy.isDead || livingPlayers.Count == 0) continue;

            Unit randomTarget = livingPlayers[UnityEngine.Random.Range(0, livingPlayers.Count)];
            enemy.hasActed = true;

            actionQueue.Add(new CombatAction
            {
                actor = enemy,
                target = randomTarget,
                actionType = "Attack",
                speedPriority = enemy.speed
            });
        }

        Debug.Log("Enemies acted");
        ChangeState(CombatState.EXECUTION_PHASE);
    }
    
    /// <summary>
    /// End of Enemy Phase
    /// </summary>
 
    IEnumerator ExecutionPhase()
    {
        // Sorted using speedPriority
        actionQueue = actionQueue.OrderByDescending(a => a.speedPriority).ToList();

        foreach (CombatAction action in actionQueue)
        {
            if (action.actor.isDead) continue;

            if (action.target == null || action.target.isDead)
            {
                List<Unit> fallbackTeam = action.target.isPlayerTeam ? playerTeam : enemyTeam;
                action.target = fallbackTeam.FirstOrDefault(u => u != null && !u.isDead);

                if (action.target == null) continue;
            }

            Debug.Log($"{action.actor.unitName} (Speed {action.speedPriority}) uses {action.actionType} on {action.target.unitName}!");

            // Perform current character action
            PerformAction(action);

            yield return new WaitForSeconds(1.5f);

            if (CheckForBattleEnd()) yield break;
        }

        // Returns to player combat after all calculations done
        ChangeState(CombatState.PLAYER_PHASE);
    }

    private void PerformAction(CombatAction action)
    {
        switch (action.actionType)
        {
            case "Attack":
                if (action.target.DodgeChance(action.target.speed))
                {
                    Debug.Log($"{action.target.unitName} dodged {action.actor.unitName}");
                }
                else
                {
                    action.target.Damaged(action.actor.attackPower);
                }
                
                break;

            case "Heal":
                action.target.Heal(action.actor.magicPower);
                break;

            case "DefBuff":
                action.target.DefBuff(action.actor.magicPower);
                break;
        }
    }

    bool CheckForBattleEnd()
    {
        if (playerTeam.All(u => u.isDead))
        {
            ChangeState(CombatState.LOST); 
            return true;
        }

        if (enemyTeam.All(u => u.isDead))
        {
            ChangeState(CombatState.WON);
            return true;
        }

        return false;
    }

    private void LostPhase()
    {
        Debug.Log("Battle lost");
    }

    private void WonPhase()
    {
        Debug.Log("Battle won");
    }
}
