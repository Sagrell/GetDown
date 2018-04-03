using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraBehaviour : MonoBehaviour {
    const float startAspect = 0.5625f;
    const float startSize = 8.2f;
    const float ratio = 19.5f;
    Camera cam;
    float aspectRatio;
	// Use this for initialization
	void Start () {
        cam = GetComponentInChildren<Camera>();
        #if UNITY_ANDROID || UNITY_EDITOR
        aspectRatio = cam.aspect;
        cam.orthographicSize = startAspect*startSize/ aspectRatio;
        #endif
    }
}
