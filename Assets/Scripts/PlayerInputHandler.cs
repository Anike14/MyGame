using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.Events;

public class PlayerInputHandler : MonoBehaviour
{
	public Camera _currentCamera;
	public LayerMask _layerMask;
	public float _threshold = 0.5f;
    
	private UnityEvent<GameObject> HandleMouseClickEvent;
    private UnityEvent<Vector3> HandleMovementEvent;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0)) {
			HandleMouseClick();
        }

        if (Input.GetMouseButtonUp(0)) {
			HandleMouseUp();
        }
    }

    void HandleMouseClick() {
		Vector3 mouseInput = _currentCamera.ScreenToWorldPoint(Input.mousePosition);
		mouseInput.z = 0f;
		Collider2D collider = Physics2D.OverlapPoint(mouseInput, _layerMask);
		GameObject selectedObject = collider == null ? null : collider.gameObject;
	}

	void HandleMouseUp () {
	}
}
