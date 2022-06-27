using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitBase : MonoBehaviour
{

    [SerializeField]
    public string _unitType;

    private bool _canMove = true;

    public bool isMovable() {
        return _canMove;
    }

    public void activateMovable() {
        _canMove = true;
    }

    public void deactivateMovable() {
        _canMove = false;
    }
}
