using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    public float _panSpeed = 20f;
    private float _leftLimit, _rightLimit, _toppLimit, _bottomLimit;
    // Start is called before the first frame update
    void Start()
    {
        _leftLimit = -43.42f;
        _rightLimit = 46.62f;
        _toppLimit = 20.22f;
        _bottomLimit = -16.4f;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 position = transform.position;
        if (Input.GetKey("w")) {
            position.y += _panSpeed * Time.deltaTime;
        } 
        if (Input.GetKey("s")) {
            position.y -= _panSpeed * Time.deltaTime;
        } 
        if (Input.GetKey("d")) {
            position.x += _panSpeed * Time.deltaTime;
        }
        if (Input.GetKey("a")) {
            position.x -= _panSpeed * Time.deltaTime;
        }
        position.x = Mathf.Clamp(position.x, _leftLimit, _rightLimit);
        position.y = Mathf.Clamp(position.y, _bottomLimit, _toppLimit);
        transform.position = position;
    }
}
