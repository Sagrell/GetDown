using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayer : MonoBehaviour {

    public float cameraSpeed = 10f;
    public Transform player;
    //Vector3 cameraPosition;
	void Awake () {
        //transform.position = new Vector3(0, player.position.y, -10f);
        //cameraPosition = transform.position;

    }
	
	// Update is called once per frame
	void Update () {
        //cameraPosition.y -= cameraSpeed * Time.deltaTime;
        //transform.position = cameraPosition;

    }
}
