using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DownPhaseManager : MonoBehaviour {

    public Transform level;
    public GameObject player;
    public Transform playerPosition;

    public float levelSpeed = 1f;
    public float maxLevelSpeed = 8f;

    PlayerController playerController;
    Vector3 playerScreenPos;
    float startPlayerSpeed;
    Vector3 levelPosition;
    float startLevelSpeed;
    float stepUp;
    void Start()
    {
        playerController = Instantiate(player, playerPosition.position, playerPosition.rotation, playerPosition.parent).GetComponent<PlayerController>(); 
        levelPosition = level.position;
        startLevelSpeed = levelSpeed;
        startPlayerSpeed = playerController.speed;
    }
    
    void FixedUpdate()
    {
        playerScreenPos = playerController.playerScreenPos;
        if (playerScreenPos.y <= Screen.height * 0.25f)
        {
            startLevelSpeed = Mathf.MoveTowards(startLevelSpeed, startPlayerSpeed, .05f);
        } else if((playerScreenPos.y > Screen.height * 0.25f)&&(playerScreenPos.y < Screen.height * 0.7f))
        {
            startLevelSpeed = Mathf.MoveTowards(startLevelSpeed, 2f, .05f);
        } else
        {
            startLevelSpeed = Mathf.MoveTowards(startLevelSpeed, 0f, .05f);
        }
        levelSpeed = startLevelSpeed * GameState.currentSpeedFactor;
        stepUp = levelSpeed * GameManager.fixedDeltaTime;
        levelPosition.y += stepUp;
        level.position = levelPosition;
    }


}
