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
    
    // selected unit
	private GameObject selectedObject;
    private UnitBase selectedUnit;
    private FlashFeedback flashFeedback;


    // moving related
    private Vector2 lastClickedPos;

    public void HandleUpdate() {
        if (selectedObject != null && (Vector2)selectedObject.transform.position != lastClickedPos) {
            selectedObject.transform.position = Vector2.MoveTowards(selectedObject.transform.position, lastClickedPos, 10f * Time.deltaTime);
        }
    }

    public void HandleSelection(GameObject selection) {
        if (selection != null) {
            this.selectedObject = selection;
            lastClickedPos = _tilemap.GetCellCenterWorld(_tilemap.WorldToCell(selectedObject.transform.position));
            selectedUnit = selectedObject.transform.GetChild(0).GetComponent<UnitBase>();
            flashFeedback = selectedObject.GetComponent<FlashFeedback>();
            flashFeedback.PlayerFeedback();
        }
	}

	public void HandleMovement(Vector3 mouseInput) {
        Debug.Log("-----------------");
        if (selectedObject == null) return;
        flashFeedback.StopFeedBack();
        Vector3Int targetPosition = _tilemap.WorldToCell(mouseInput),
                currentPosition = _tilemap.WorldToCell(selectedObject.transform.position);
        int distance = Mathf.Abs(targetPosition.x - currentPosition.x) + Mathf.Abs(targetPosition.y - currentPosition.y);
        if (selectedUnit._unitType == "Tank") {
            Tank tank = (Tank)selectedUnit;
            if (tank._maximumMovement < distance) {
                deselect();
            } else {
                lastClickedPos = _tilemap.GetCellCenterWorld(targetPosition);
            }
        }
        Debug.Log(lastClickedPos);
    }

    private void deselect() {
        flashFeedback.StopFeedBack();
        selectedUnit = null;
        selectedObject = null;
        OnSelectedObjectDeselect?.Invoke();
    }
}
