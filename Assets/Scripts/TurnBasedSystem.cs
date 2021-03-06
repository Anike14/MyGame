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

    // Start is called before the first frame update
    void Start()
    {
        NextTurn();
    }

    public void NextTurn() {
        if (currentTurn % 2 == 0)
            OnEvenTurnEnd?.Invoke();
        else OnOddTurnEnd?.Invoke();
        currentTurn++;
    }
}
