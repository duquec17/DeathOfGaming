using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class CombatUI : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private Transform cursorVisual;
    [SerializeField] private Vector3 cursorOffset = new Vector3(0f, 1.5f, 0f);

    [Header("System References")]
    [SerializeField] private CombatSystem combatSystem;
    [SerializeField] public GameObject actionMenuPanel;

    [Header("Input system actions")]
    [SerializeField] private InputActionReference navigateAction;
    [SerializeField] private InputActionReference submitAction;
    [SerializeField] private InputActionReference cancelAction;

    private List<Unit> validTargets = new List<Unit>();
    private int currentIndex = 0;
    private bool isSelecting = false;

    [SerializeField] private Action<Unit> onTargetConfirmed;
    [SerializeField] private Action onSelectionCanceled;

    

    public void OnAttackButtonPressed()
    {
        if (combatSystem.currentState != CombatState.PLAYER_PHASE)
        {
            Debug.Log($"combatSystem.currentState");
            return;
        }

        actionMenuPanel.SetActive(false);

        StartTargetSelection
        (
            combatSystem.enemyTeam,
            onConfirm: (selectedEnemy) =>
            {
                combatSystem.OnplayerActionChosen("Attack", selectedEnemy);
                if (combatSystem.currentState == CombatState.PLAYER_PHASE)
                {
                    actionMenuPanel.SetActive(true);
                }
            },
            onCancel: () =>
            {
                actionMenuPanel.SetActive(true);
            }
        );
    }

    private void OnEnable()
    {
        // Setup input system
        navigateAction.action.performed += OnNavigatePerformed;
        submitAction.action.performed += OnSubmitPerformed;
        cancelAction.action.performed += OnCancelPerformed;

        navigateAction.action.Enable();
        submitAction.action.Enable();
        cancelAction.action.Enable();
    }

    private void OnDisable()
    {
        navigateAction.action.performed -= OnNavigatePerformed;
        submitAction.action.performed -= OnSubmitPerformed;
        cancelAction.action.performed -= OnCancelPerformed;
    }

    public void StartTargetSelection(List<Unit> enemies, Action<Unit> onConfirm, Action onCancel = null)
    {
        validTargets.Clear();
        foreach (Unit enemy in enemies)
        {
            if (enemy != null && !enemy.isDead && enemy.currentHP > 0)
            {
                validTargets.Add(enemy);
            }
        }

        if (validTargets.Count == 0)
        {
            actionMenuPanel.SetActive(true);
            return;
        }

        if (EventSystem.current != null)
        {
            EventSystem.current.SetSelectedGameObject(null);
        }

        onTargetConfirmed = onConfirm;
        onSelectionCanceled = onCancel;
        currentIndex = 0;
        isSelecting = true;

        cursorVisual.gameObject.SetActive(true);
        UpdateCursorPosition();
    }

    private void OnNavigatePerformed(InputAction.CallbackContext context)
    {
        if (!isSelecting || validTargets.Count == 0) return;

        Vector2 input = context.ReadValue<Vector2>();

        // Cycle forward on right or down
        if (input.x > 0.5f || input.y < -0.5f)
        {
            CycleTarget(1);
        }
        else if (input.x < -0.5f || input.y > 0.5f)
        {
            CycleTarget(-1);
        }
    }

    private void CycleTarget(int direction)
    {
        currentIndex = (currentIndex + direction + validTargets.Count) % validTargets.Count;
        UpdateCursorPosition();
    }

    private void UpdateCursorPosition()
    {
        Unit selectedEnemy = validTargets[currentIndex];
        cursorVisual.position = selectedEnemy.transform.position + cursorOffset;
    }

    private void OnSubmitPerformed(InputAction.CallbackContext context)
    {
        if (!isSelecting || validTargets.Count == 0) return;

        isSelecting = false;
        cursorVisual.gameObject.SetActive(false);
        onTargetConfirmed?.Invoke(validTargets[currentIndex]);
        Debug.Log("Submit Performed");
    }

    private void OnCancelPerformed(InputAction.CallbackContext context)
    {
        if (!isSelecting) return;

        isSelecting = false;
        cursorVisual.gameObject.SetActive(false);
        onSelectionCanceled?.Invoke();
        Debug.Log("Cancel Performed");
    }
}
