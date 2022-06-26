using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Tilemaps;
using UnityEngine.Events;

public class UnitMovement : MonoBehaviour
{
    [SerializeField]
    public Tilemap _tilemap;
    [SerializeField]
    private UnityEvent OnSelectedObjectDeselect;
    [SerializeField]
    private MapManager_Land mapManager_land;
    [SerializeField]
    private MovementRangeHighlight movementRangeHighlight;
    
    // selected unit
	private GameObject selectedObject;
    private UnitBase selectedUnit;

    // moving related
    private List<List<Vector2Int>> movableRange;
    private Stack<List<Vector2Int>> movingTowards;
    private Vector3 originalPosition;
    private Vector3 movingPosition;

    public void HandleUpdate() {
        if (selectedObject != null && selectedObject.transform.position != movingPosition) {
            if (movingPosition.x > selectedObject.transform.position.x && selectedObject.transform.localScale.x > 0) flipUnit();
            selectedObject.transform.position = Vector2.MoveTowards(selectedObject.transform.position, movingPosition, 10f * Time.deltaTime);
        } else if (selectedObject != null && movingTowards != null && selectedObject.transform.position == movingPosition) {
            if (movingTowards.Count > 0) {
                movingPosition = MapManager_Land._tilemap.GetCellCenterWorld((Vector3Int)movingTowards.Pop()[0]);
            if (movingPosition.x < selectedObject.transform.position.x && selectedObject.transform.localScale.x < 0) flipUnit();
            selectedObject.transform.position = Vector2.MoveTowards(selectedObject.transform.position, movingPosition, 10f * Time.deltaTime);
            } else MovingDone();
        }
    }

    public void HandleSelection(GameObject selection) {
        if (this.selectedObject != null)
            this.selectedObject.transform.position = originalPosition;
        this.selectedObject = selection;
        movingPosition = this.selectedObject.transform.position;
        selectedUnit = selectedObject.transform.GetChild(0).GetComponent<UnitBase>();

        
        if (selectedUnit._unitType == Constants._unitType_Tank) {
            Tank tank = (Tank)selectedUnit;
            movableRange = mapManager_land.GetMovementRange(tank,
                _tilemap.WorldToCell(selectedObject.transform.position), 8f);
            movementRangeHighlight.PaintTileForMovable(movableRange);
            originalPosition = selectedObject.transform.position;
        }
	}

	public void HandleMovement(Vector3 mouseInput) {
        if (selectedObject == null) return;
        movingTowards = new Stack<List<Vector2Int>>();
        Vector2Int targetPosition = (Vector2Int)_tilemap.WorldToCell(mouseInput);
        List<Vector2Int> targetTowards = movableRange.Find(target => target[0] == targetPosition);
        if (targetTowards != null) {
            movingTowards.Push(targetTowards);
            targetTowards = movableRange.Find(target => target[0] == targetPosition);
            while (targetTowards[1] != (Vector2Int)MapManager_Land._tilemap.WorldToCell(originalPosition)) {
                targetTowards = movableRange.Find(target => target[0] == targetTowards[1]);
                movingTowards.Push(targetTowards);
            }
        } else Deselect();
    }

    private void flipUnit() {
        if (selectedObject != null) {
            Vector3 newScale = 
                selectedObject.transform.localScale;
            newScale.x *= -1; 
            selectedObject.transform.localScale = newScale;
        }
    }

    private void Deselect() {
        selectedObject.transform.position = originalPosition;
        movementRangeHighlight.ClearMovable();
        movingTowards = null;
        selectedUnit = null;
        selectedObject = null;
        OnSelectedObjectDeselect?.Invoke();
    }

    private void MovingDone() {
        originalPosition = selectedObject.transform.position;
        // For now
        Deselect();
    }
}
