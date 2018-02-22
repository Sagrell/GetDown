using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraBehaviour : MonoBehaviour {
    const float startAspect = 0.5625f;
    const float startSize = 11f;
    const float ratio = 19.5f;
    Camera cam;
    float aspectRatio;
	// Use this for initialization
	void Start () {
        cam = GetComponentInChildren<Camera>();
        aspectRatio = cam.aspect;
        cam.orthographicSize = startAspect*startSize/ aspectRatio;
    }
}
