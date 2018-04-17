using System.Collections;
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
    public static Vector3 levelPosition;
    // Use this for initialization

    void Start()
    {
        levelPosition = transform.position;
        startLevelSpeed = levelSpeed;
        startPlayerSpeed = GameManager.Instance.playerSpeed;
    }
    // Update is called once per frame
    void Update () {
        playerScreenPos = GameManager.player.playerScreenPos;
        if (playerScreenPos.y < Screen.height * 0.45f)
        {
            if (startLevelSpeed < maxLevelSpeed)
                startLevelSpeed = Mathf.MoveTowards(startLevelSpeed, startPlayerSpeed, .03f);
        }
        /*else if ((playerScreenPos.y > Screen.height * 0.25f) && (playerScreenPos.y < Screen.height * 0.65f))
        {
            startLevelSpeed = Mathf.MoveTowards(startLevelSpeed, 2f, .03f);
        }*/
        else
        {
            startLevelSpeed = Mathf.MoveTowards(startLevelSpeed, 1f, .03f);
        }
        if (GameState.currentSpeedFactor <= 2)
        {
            levelSpeed = startLevelSpeed * GameState.currentSpeedFactor;
        }else
        {
            levelSpeed = startLevelSpeed * 2;
        }
        
        if (levelSpeed > maxLevelSpeed)
            levelSpeed = maxLevelSpeed;

        stepUp = levelSpeed * GameManager.deltaTime;
        levelPosition.y += stepUp;
        transform.position = levelPosition;
    }
}
