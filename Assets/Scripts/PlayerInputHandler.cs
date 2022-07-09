using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.Events;

public class PlayerInputHandler : MonoBehaviour
{
    [SerializeField]
	public Camera _currentCamera;
    [SerializeField]
	public LayerMask _myLayerMask;
    [SerializeField]
	public LayerMask _enemyLayerMask;
    [SerializeField]
	private UnityEvent OnHandleUpdate;
    [SerializeField]
	private UnityEvent<GameObject, LayerMask, LayerMask> OnHandleSelection;
    [SerializeField]
    private UnityEvent<Vector3> OnHandleMovement;
    
	private GameObject selectedObject;

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
            else HandleMove();
        }

        OnHandleUpdate?.Invoke();
    }


    private void HandleSelection() {
		Vector3 mouseInput = _currentCamera.ScreenToWorldPoint(Input.mousePosition);
		mouseInput.z = 0f;
		Collider2D collider = Physics2D.OverlapPoint(mouseInput, _myLayerMask);
		selectedObject = collider == null ? null : collider.gameObject;
        if (selectedObject != null) {
            OnHandleSelection?.Invoke(selectedObject, _myLayerMask, _enemyLayerMask);
        }
	}

	private void HandleMove() {
        Vector3 mouseInput = _currentCamera.ScreenToWorldPoint(Input.mousePosition);
		mouseInput.z = 0f;
        
		Collider2D collider = Physics2D.OverlapPoint(mouseInput, _myLayerMask);
        if (collider != null) {
            HandleSelection();
            return;
        }
        
        UnitBase selectedUnit = selectedObject.transform.GetChild(0).GetComponent<UnitBase>();
        if (selectedUnit != null) {
            if (selectedUnit.isMovable()) 
                OnHandleMovement?.Invoke(mouseInput);
        }
	}

    public void handleDeselect() {
        this.selectedObject = null;
    }
}
