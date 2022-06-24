using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Tilemaps;

public class UnitMovement : MonoBehaviour
{
    [SerializeField]
	public Camera _currentCamera;
    [SerializeField]
	public LayerMask _layerMask;
    [SerializeField]
    public Tilemap _tilemap;
    
    // selected unit
	private GameObject selectedObject;
    private UnitBase selectedUnit;
    private FlashFeedback flashFeedback;

    // moving related
    private bool moving = false;
    private Vector2 lastClickedPos;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0)) {
			if (selectedObject == null)
                HandleSelection();
            else if (!moving) HandleMovement();
        }

        if (moving && (Vector2)selectedObject.transform.position != lastClickedPos) {
            selectedObject.transform.position = Vector2.MoveTowards(selectedObject.transform.position, lastClickedPos, 10f * Time.deltaTime);
        } else moving = false;
    }

    void HandleSelection() {
		Vector3 mouseInput = _currentCamera.ScreenToWorldPoint(Input.mousePosition);
		mouseInput.z = 0f;

		Collider2D collider = Physics2D.OverlapPoint(mouseInput, _layerMask);
		selectedObject = collider == null ? null : collider.gameObject;
        lastClickedPos = _tilemap.GetCellCenterWorld(_tilemap.WorldToCell(selectedObject.transform.position));

        if (selectedObject != null) {
            selectedUnit = selectedObject.transform.GetChild(0).GetComponent<UnitBase>();
            flashFeedback = selectedObject.GetComponent<FlashFeedback>();
            flashFeedback.PlayerFeedback();
        }
	}

	void HandleMovement() {
        if (selectedObject == null) return;

        flashFeedback.StopFeedBack();
        Vector3Int targetPosition = _tilemap.WorldToCell(_currentCamera.ScreenToWorldPoint(Input.mousePosition)),
                currentPosition = _tilemap.WorldToCell(selectedObject.transform.position);
        int distance = Mathf.Abs(targetPosition.x - currentPosition.x) + Mathf.Abs(targetPosition.y - currentPosition.y);
        if (selectedUnit._unitType == "Tank") {
            Tank tank = (Tank)selectedUnit;
            Debug.Log("tank._maximumMovement: " + tank._maximumMovement);
            Debug.Log("distance: " + distance);
            if (tank._maximumMovement < distance) deselect();
            else {
                lastClickedPos = _tilemap.GetCellCenterWorld(targetPosition);
                moving = true;
            }
        }
    }

    void deselect() {
        flashFeedback.StopFeedBack();
        selectedUnit = null;
        selectedObject = null;
    }
}
