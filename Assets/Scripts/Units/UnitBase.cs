using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.Events;

public class UnitBase : MonoBehaviour
{
    [SerializeField]
    private UnityEvent OnActivateActable;
    [SerializeField]
    private UnityEvent OnDeactivateActable;
    [SerializeField]
    public string _unitType;

    private bool _canMove = true;
    private bool _canAct = true;

    public bool IsMovable() {
        return _canMove;
    }

    public bool IsActable() {
        return _canAct;
    }

    public void ActivateMovable() {
        Debug.Log("activating actable....");
        _canMove = true;
        _canAct = true;
        OnActivateActable?.Invoke();
    }

    public void DeactivateMovable() {
        Debug.Log("deactivating movable....");
        _canMove = false;
    }

    public void DeactivateActable() {
        Debug.Log("deactivating actable....");
        _canAct = false;
        OnDeactivateActable?.Invoke();
    }
}
