using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.Events;

public class TurnBasedSystem : MonoBehaviour
{
    [SerializeField]
    private UnityEvent OnEvenTurnEnd;
    [SerializeField]
    private UnityEvent OnOddTurnEnd;

    private int currentTurn = 0;

    public void NextTurn() {
        if (currentTurn % 2 == 0)
            OnEvenTurnEnd?.Invoke();
        else OnOddTurnEnd?.Invoke();
        foreach(UnitBase unit in FindObjectsOfType<UnitBase>()) {
            unit.activateMovable();
        }
        currentTurn++;
    }
}
