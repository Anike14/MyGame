using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.Events;

public class UnitMovement : MonoBehaviour
{
    [SerializeField]
    private UnityEvent OnSelectedObjectDeselect;
    
    [SerializeField]
    private UnityEvent OnDeactivateAimingMode;

    [SerializeField]
    private UnityEvent OnSelectedObjectFirable;

    [SerializeField]
    private UnityEvent<GameObject> OnTargetObjectDestroyed;

    [SerializeField]
    private MovementRangeHighlight movementRangeHighlight;

    [SerializeField]
    private FirableRangeHighlight firableRangeHighlight;

    [SerializeField]
	private GameObject _selectionPanel;
    
    // selected unit
	private GameObject selectedObject;
    private UnitBase selectedUnit;

	private GameObject selectedEnemyObject;
    private UnitBase selectedEnemyUnit;

    // moving related
    private List<List<Vector2Int>> movableRange;
    private Stack<List<Vector2Int>> movingTowards;
    private List<List<Vector2Int>> firableRange;
    private Vector3 originalPosition;
    private Vector3 movingPosition;

    public void HandleUpdate(LayerMask myLayer, LayerMask enemyLayer) {
        if (selectedObject != null && selectedObject.transform.position != movingPosition) {
            if (movingPosition.x > selectedObject.transform.position.x && selectedObject.transform.localScale.x > 0) FlipUnit();
            selectedObject.transform.position = Vector2.MoveTowards(selectedObject.transform.position, movingPosition, 10f * Time.deltaTime);
        } else if (selectedObject != null && movingTowards != null && selectedObject.transform.position == movingPosition) {
            if (selectedUnit._unitType == Constants._unitType_Tank) {
                // using BFS for scouting enemy
                if (MapManager_Land.ScoutFromMyPosition((Tank)selectedUnit, 
                    (Vector2Int)MapManager_Land._tilemap.WorldToCell(selectedObject.transform.position), enemyLayer)) {
                    MovingDone();
                    return;
                }
            }
            if (movingTowards.Count > 0) {
                movingPosition = MapManager_Land._tilemap.GetCellCenterWorld((Vector3Int)movingTowards.Pop()[0]);
            if (movingPosition.x < selectedObject.transform.position.x && selectedObject.transform.localScale.x < 0) FlipUnit();
            selectedObject.transform.position = Vector2.MoveTowards(selectedObject.transform.position, movingPosition, 10f * Time.deltaTime);
            } else MovingDone();
        }
    }

    public void HandleSelection(GameObject selection, LayerMask myLayerMask, LayerMask enemyLayerMask) {
        if (selection == null) { 
            Deselect(); return; }
        if (this.selectedObject != null && this.selectedObject != selection) { 
            if (movingTowards != null) return;
            else Deselect(true);
        }
        this.selectedObject = selection;
        movingPosition = this.selectedObject.transform.position;
        selectedUnit = selectedObject.transform.GetChild(0).GetComponent<UnitBase>();
        if (selectedUnit.IsDestroyed()) {
            Deselect();
            return;
        }
        if (selectedUnit.IsActable()) _selectionPanel.SetActive(true);
        selectedUnit.PlaySelectedEffect();
        if (selectedUnit._unitType == Constants._unitType_Tank) {
            Tank tank = (Tank)selectedUnit;
            if (tank.IsMovable()) {
                movableRange = MapManager_Land.GetMovementRange(tank,
                    MapManager_Land._tilemap.WorldToCell(selectedObject.transform.position), 20f, myLayerMask, enemyLayerMask);
                movementRangeHighlight.PaintTileForMovable(movableRange);
            }
            originalPosition = selectedObject.transform.position;
        }
	}

	public void HandleMovement(Vector3 mouseInput) {
        if (selectedObject == null) return;
        if (selectedUnit != null && selectedUnit.IsMoving()) return;
        if (!selectedUnit.IsMovable()) { Deselect(); return; }
        movingTowards = new Stack<List<Vector2Int>>();
        Vector2Int targetPosition = (Vector2Int)MapManager_Land._tilemap.WorldToCell(mouseInput);
        List<Vector2Int> targetTowards = movableRange.Find(target => target[0] == targetPosition);
        if (targetTowards != null) {
            movingTowards.Push(targetTowards);
            targetTowards = movableRange.Find(target => target[0] == targetPosition);
            while (targetTowards[1] != (Vector2Int)MapManager_Land._tilemap.WorldToCell(originalPosition)) {
                targetTowards = movableRange.Find(target => target[0] == targetTowards[1]);
                movingTowards.Push(targetTowards);
            }
            selectedUnit.PlayMovingEffect();
        } else Deselect();
    }

    public void HandleAiming(LayerMask myLayerMask, LayerMask enemyLayerMask) {
        if (selectedUnit._unitType == Constants._unitType_Tank) {
            Tank tank = (Tank)selectedUnit;
            firableRange = MapManager_Land.GetFirePowerRange(tank, 
                MapManager_Land._tilemap.WorldToCell(originalPosition), myLayerMask, enemyLayerMask);
            firableRangeHighlight.PaintTileForFirable(firableRange);
        }
    }

    public void HandleScouting() {
        selectedUnit.Scouting();
        ActingDone();
    }

    public void HandleHolding() {
        if (selectedUnit._unitType == Constants._unitType_Tank) {
            Tank tank = (Tank)selectedUnit;
        }
        ActingDone();
    }

    public void HandleHiding() {
        selectedUnit.hiding();
        ActingDone();
    }

    public void HandleEnemySelection(GameObject selection) {
        if (selection == null) { DeactivateAimingMode(); return; }
        Vector2Int targetPosition = (Vector2Int)MapManager_Land._tilemap.WorldToCell(selection.transform.position);
        if (firableRange.Find(target => target[0] == targetPosition) == null) { DeactivateAimingMode(); return; }
        this.selectedEnemyObject = selection;
        selectedEnemyUnit = selectedEnemyObject.transform.GetChild(0).GetComponent<UnitBase>();
        if (selectedEnemyUnit.IsDestroyed() || selectedEnemyUnit.IsConcealed()) return;
        if (selectedEnemyUnit._unitType == Constants._unitType_Tank) {
            Tank enemyTank = (Tank)selectedEnemyUnit;
            // show enemyTank on the sidebar
            // add enemyTank selection feedback
            OnSelectedObjectFirable?.Invoke();
        }
    }

    public void Fire() {
        MovingDone();
        firableRangeHighlight.ClearFirable();
        if (selectedUnit._unitType == Constants._unitType_Tank) {
            Tank tank = (Tank)selectedUnit;
            Tank enemyTank = (Tank)selectedEnemyUnit;
            if (enemyTank.transform.position.x > tank.transform.position.x
                && selectedObject.transform.localScale.x > 0) FlipUnit();
            else if (enemyTank.transform.position.x < tank.transform.position.x
                && selectedObject.transform.localScale.x < 0) FlipUnit();
            tank.FireAt(enemyTank);
        }
        ActingDone();
    }

    private void FlipUnit() {
        if (selectedObject != null) {
            Vector3 newScale = 
                selectedObject.transform.localScale;
            newScale.x *= -1; 
            selectedObject.transform.localScale = newScale;
        }
    }

    private void MovingDone() {
        originalPosition = selectedObject.transform.position;
        selectedUnit.DeactivateMovable();
        movementRangeHighlight.ClearMovable();
        movingTowards = null;
    }

    private void ActingDone() {
        originalPosition = selectedObject.transform.position;
        selectedUnit.DeactivateActable();
        firableRangeHighlight.ClearFirable();
        Deselect();
    }

    private void DeactivateAimingMode() {
        selectedEnemyUnit = null;
        selectedEnemyObject = null;
        firableRangeHighlight.ClearFirable();
        if (selectedUnit.IsMovable())
            movementRangeHighlight.PaintTileForMovable(movableRange);
        else Deselect();
        OnDeactivateAimingMode?.Invoke();
    }

    public void Deselect() { 
        this.Deselect(false); 
    }

    private void Deselect(bool Silience) {
        if (!selectedUnit.IsDestroyed()) {
            if (selectedObject != null)
                selectedObject.transform.position = originalPosition;
            if (selectedUnit != null)
                selectedUnit.PlayDeselectedEffect();
            movementRangeHighlight.ClearMovable();
        }
        movingTowards = null;
        selectedUnit = null;
        selectedObject = null;
        _selectionPanel.SetActive(false);
        if (!Silience) OnSelectedObjectDeselect?.Invoke();
    }
}
