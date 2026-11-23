using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class CombatUI : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private Transform cursorVisual;
    [SerializeField] private Vector3 cursorOffset = new Vector3(0f, 1.5f, 0f);

    [Header("Input system actions")]
    [SerializeField] private InputActionReference navigateAction;
    [SerializeField] private InputActionReference submitAction;
    [SerializeField] private InputActionReference cancelAction;

    private List<Unit> validTargets = new List<Unit>();
    private int currentIndex = 0;
    private bool isSelecting = false;

    private Action<Unit> onTargetConfirmed;
    private Action onSelectionCanceled;

    private void OnEnable()
    {
        // Setup input system
        navigateAction.action.performed += OnNavigatePerformed;

        navigateAction.action.Enable();
    }

    private void OnDisable()
    {
        navigateAction.action.performed -= OnNavigatePerformed;
    }

    public void StartTargetSelection(List<Unit> enemies, Action<Unit> onConfirm, Action onCancel = null)
    {
        validTargets.Clear();
        foreach (Unit enemy in enemies)
        {
            if (enemy != null && enemy.currentHP > 0)
            {
                validTargets.Add(enemy);
            }
        }

        if (validTargets.Count == 0) return;

        if (EventSystem.current != null)
        {
            EventSystem.current.SetSelectedGameObject(null);
        }

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
}
