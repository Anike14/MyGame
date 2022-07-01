using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.Events;

public class UnitBase : MonoBehaviour
{
    [SerializeField]
    private UnityEvent OnDeactivateMovable;
    [SerializeField]
    private UnityEvent OnActivateMovable;
    [SerializeField]
    public string _unitType;

    private bool _canMove = true;

    public bool isMovable() {
        return _canMove;
    }

    public void activateMovable() {
        Debug.Log("activating movable....");
        _canMove = true;
        OnActivateMovable?.Invoke();
    }

    public void deactivateMovable() {
        Debug.Log("deactivating movable....");
        _canMove = false;
        OnDeactivateMovable?.Invoke();
    }
}
