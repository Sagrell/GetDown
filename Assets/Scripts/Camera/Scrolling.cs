using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scrolling : MonoBehaviour {
    public float speed = 5f;
    Renderer _renderer;
    Vector2 offset;
	// Use this for initialization
	void Start () {
        _renderer = GetComponent<Renderer>();
        offset = Vector2.zero;
    }
	
	// Update is called once per frame
	void Update () {
        offset.y += Time.deltaTime * speed;
        _renderer.material.mainTextureOffset = offset;
    }
}
