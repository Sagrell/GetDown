﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelController : MonoBehaviour {

    
    [Space]
    [Header("Settings:")]
    public float levelSpeed = 2f;
    public float maxLevelSpeed = 6f;

   // public PlatformSpawner spawner;
    Vector3 playerScreenPos;
    float startPlayerSpeed;
    float startLevelSpeed;
    float stepUp;
    Vector3 levelPosition;
    // Use this for initialization

    void Start()
    {
        levelPosition = transform.position;
        startLevelSpeed = levelSpeed;
        startPlayerSpeed = GameManager.Instance.playerSpeed;
    }
	// Update is called once per frame
	void FixedUpdate () {
        playerScreenPos = GameManager.player.playerScreenPos;
        if (playerScreenPos.y <= Screen.height * 0.35f)
        {
            if (startLevelSpeed < maxLevelSpeed)
                startLevelSpeed = Mathf.MoveTowards(startLevelSpeed, startPlayerSpeed, .04f);
        }
        else if ((playerScreenPos.y > Screen.height * 0.25f) && (playerScreenPos.y < Screen.height * 0.65f))
        {
            startLevelSpeed = Mathf.MoveTowards(startLevelSpeed, 2f, .04f);
        }
        else
        {
            startLevelSpeed = Mathf.MoveTowards(startLevelSpeed, 1f, .04f);
        }
        levelSpeed = startLevelSpeed * GameState.currentSpeedFactor;
        if (levelSpeed < maxLevelSpeed)
            stepUp = levelSpeed * GameManager.fixedDeltaTime;
        levelPosition.y += stepUp;
        transform.position = levelPosition;
    }
}