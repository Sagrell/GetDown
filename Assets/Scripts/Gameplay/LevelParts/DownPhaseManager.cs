using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DownPhaseManager : MonoBehaviour {

    public Transform level;
    public PlayerController player;

    public float levelSpeed = 1f;
    public float maxLevelSpeed = 8f;

    Vector3 playerScreenPos;
    float startPlayerSpeed;
    Vector3 levelPosition;
    float startLevelSpeed;
    float stepUp;
    void Start()
    {
        levelPosition = level.position;
        startLevelSpeed = levelSpeed;
        startPlayerSpeed = player.speed;
    }
    
    void FixedUpdate()
    {
        playerScreenPos = player.playerScreenPos;
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
