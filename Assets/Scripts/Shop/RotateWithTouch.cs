using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateWithTouch : MonoBehaviour {

    public float speed = 20f;
    Transform _transform;
	// Use this for initialization
	void Start () {
        _transform = transform;
    }

    void OnMouseDrag()
    {
        float x = Input.GetAxis("Mouse X") * speed;
        _transform.Rotate(Vector3.down, x);
    }
}
