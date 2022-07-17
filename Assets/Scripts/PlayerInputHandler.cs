using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class PlayerInputHandler : MonoBehaviour
{
    [SerializeField]
	public GameObject _me;

    [SerializeField]
    private List<GameObject> _vitoryConditions;

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
    
    [SerializeField]
	private UnityEvent<GameObject> OnHandleEnemySelection;
    
    [SerializeField]
	private UnityEvent OnVictory;

    [SerializeField]
    private UnityEvent<LayerMask, LayerMask> OnHandleAiming;
    
	private GameObject selectedObject;
    
    private bool aimingMode = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0) 
            && !EventSystem.current.IsPointerOverGameObject()) {
			if (this.aimingMode || selectedObject == null)
                HandleSelection();
            else HandleMove();
        }
        OnHandleUpdate?.Invoke();
    }

    private void HandleSelection() {
		Vector3 mouseInput = _currentCamera.ScreenToWorldPoint(Input.mousePosition);
		mouseInput.z = 0f;
		Collider2D collider = Physics2D.OverlapPoint(mouseInput, this.aimingMode ? _enemyLayerMask : _myLayerMask);
        if (this.aimingMode)
            OnHandleEnemySelection?.Invoke(collider == null ? null : collider.gameObject);
        else {
		    selectedObject = collider == null ? null : collider.gameObject;
            OnHandleSelection?.Invoke(selectedObject, _myLayerMask, _enemyLayerMask);
        }
	}

	private void HandleMove() {
        // we're not clicking on a UI object, so do your normal movement stuff here
        Vector3 mouseInput = _currentCamera.ScreenToWorldPoint(Input.mousePosition);
        mouseInput.z = 0f;
        
        Collider2D collider = Physics2D.OverlapPoint(mouseInput, _myLayerMask);
        if (collider != null) {
            HandleSelection();
            return;
        }
        
        UnitBase selectedUnit = selectedObject.transform.GetChild(0).GetComponent<UnitBase>();
        if (selectedUnit != null) {
            OnHandleMovement?.Invoke(mouseInput);
        }
	}

    public void HandleAiming() {
        if (_me.activeSelf == false) return;
        if (selectedObject == null) return; // should never go here;
        this.aimingMode = true;
        UnitBase selectedUnit = selectedObject.transform.GetChild(0).GetComponent<UnitBase>();
        if (selectedUnit != null) {
            if (selectedUnit.IsActable()) 
                OnHandleAiming?.Invoke(_myLayerMask, _enemyLayerMask);
        }
    }

    public void HandleDeactivateAimingMode() {
        this.aimingMode = false;
    }

    public void HandleUnitDestroyed(GameObject destroyedTarget) {
        if (_vitoryConditions.Contains(destroyedTarget)) {
            _vitoryConditions.Remove(destroyedTarget);
        }
        if (_vitoryConditions.Count == 0) {
            OnVictory?.Invoke();
        }
    }

    public void HandleDeselect() {
        this.selectedObject = null;
        this.aimingMode = false;
    }
}
