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
    private MapManager mapManager;
    [SerializeField]
    private MovementRangeHighlight movementRangeHighlight;
    
    // selected unit
	private GameObject selectedObject;
    private UnitBase selectedUnit;

    // moving related
    private List<Vector2Int> movableRange;
    private Vector2 lastClickedPos;
    private Vector3 originalPosition;

    public void HandleUpdate() {
        if (selectedObject != null && (Vector2)selectedObject.transform.position != lastClickedPos) {
            selectedObject.transform.position = Vector2.MoveTowards(selectedObject.transform.position, lastClickedPos, 10f * Time.deltaTime);
        }
    }

    public void HandleSelection(GameObject selection) {
        if (this.selectedObject != null)
            this.selectedObject.transform.position = originalPosition;
        this.selectedObject = selection;
        lastClickedPos = _tilemap.GetCellCenterWorld(_tilemap.WorldToCell(selectedObject.transform.position));
        selectedUnit = selectedObject.transform.GetChild(0).GetComponent<UnitBase>();
        if (selectedUnit._unitType == "Tank") {
            Tank tank = (Tank)selectedUnit;
            movableRange = mapManager.GetMovementRange(
                _tilemap.WorldToCell(selectedObject.transform.position), 
                tank._maximumMovement, tank._stepConsumption, 1000);
            movementRangeHighlight.PaintTileForMovable(movableRange);
            originalPosition = selectedObject.transform.position;
        }
	}

	public void HandleMovement(Vector3 mouseInput) {
        if (selectedObject == null) return;
        Vector3Int targetPosition = _tilemap.WorldToCell(mouseInput),
                currentPosition = _tilemap.WorldToCell(originalPosition);
        if (movableRange.Contains((Vector2Int)targetPosition)) {
            lastClickedPos = _tilemap.GetCellCenterWorld(targetPosition);
        } else deselect();
    }

    private void deselect() {
        selectedObject.transform.position = originalPosition;
        movementRangeHighlight.ClearMovable();
        selectedUnit = null;
        selectedObject = null;
        OnSelectedObjectDeselect?.Invoke();
    }
}
