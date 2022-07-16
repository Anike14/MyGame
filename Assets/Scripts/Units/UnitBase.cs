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
    private bool _destroyed = false;

    public bool IsMovable() {
        return !_destroyed && _canMove;
    }

    public bool IsActable() {
        return !_destroyed && _canAct;
    }

    public bool IsDestroyed() {
        return _destroyed;
    }

    public void ActivateDestroyed() {
        _destroyed = true;
    }

    public void ActivateMovable() {
        if (_destroyed) return;
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
