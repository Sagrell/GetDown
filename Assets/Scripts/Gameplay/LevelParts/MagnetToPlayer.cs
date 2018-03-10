using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagnetToPlayer : MonoBehaviour {
    public float speed = 1f;
    Transform _transform;
    Transform playerTransform;
    Vector3 currPosition;
    Vector3 playerPosition;
    float startDistance;
    float currDistance;
    // Use this for initialization
    void Start () {
        _transform = transform;
        currPosition = transform.position;
        playerTransform = GameManager.player.transform;
        playerPosition = playerTransform.position;
        startDistance = Vector3.Distance(currPosition, playerPosition);
        currDistance = startDistance;
    }
    
	// Update is called once per frame
	void FixedUpdate () {
        playerPosition = playerTransform.position;
        currDistance = Vector3.Distance(_transform.position, playerPosition);
        _transform.position = Vector3.MoveTowards(_transform.position, playerPosition, speed/(currDistance / startDistance));
    }
}
