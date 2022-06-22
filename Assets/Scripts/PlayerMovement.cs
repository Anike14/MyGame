using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    float _speed = 10f;
    bool _moveable = false;
    Vector2 lastClickedPos;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0)) {
            lastClickedPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            _moveable = true;
        }

        if (_moveable && (Vector2)transform.position != lastClickedPos) {
            float step = _speed * Time.deltaTime;
            transform.position = Vector2.MoveTowards(transform.position, lastClickedPos, step);
        } else _moveable = false;
    }
}
