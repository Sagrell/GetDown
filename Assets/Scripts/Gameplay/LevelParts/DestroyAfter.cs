using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyAfter : MonoBehaviour {

    float destroyAfter;

    Transform _transform;
	void Start () {
        destroyAfter = 12f;
        _transform = transform;
    }
	
	// Update is called once per frame
	void Update () {
		if(_transform.position.y>destroyAfter && gameObject.activeSelf)
        {
            gameObject.SetActive(false);
        }
	}
}
