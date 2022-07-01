using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.Events;

public class TurnBasedSystem : MonoBehaviour
{
    [SerializeField]
    private UnityEvent OnBlockPlayerInput;
    [SerializeField]
    private UnityEvent OnUnblockPlayerInput;

    public void NextTurn() {
        Debug.Log("waiting for your turn.....");
        OnBlockPlayerInput?.Invoke();
        foreach(UnitBase unit in FindObjectsOfType<UnitBase>()) {
            unit.activateMovable();
        }
        Debug.Log("Your turn!");
        OnUnblockPlayerInput?.Invoke();
    }
}
