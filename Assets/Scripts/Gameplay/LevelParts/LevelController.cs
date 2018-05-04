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
    void FixedUpdate () {
        playerScreenPos = GameManager.player.playerScreenPos;
        if(!GameState.isLearning)
        {
            if (playerScreenPos.y < Screen.height * 0.45f)
            {
                if (startLevelSpeed < maxLevelSpeed)
                    startLevelSpeed = Mathf.MoveTowards(startLevelSpeed, startPlayerSpeed, .04f);
            }
            else
            {
                startLevelSpeed = Mathf.MoveTowards(startLevelSpeed, 1f, .04f);
            }

        } else
        {
            startLevelSpeed = 2.2f;
        }
        
        levelSpeed = startLevelSpeed * Mathf.Min(GameState.currentSpeedFactor, 2);

            

        stepUp = levelSpeed * GameManager.fixedDeltaTime;
        levelPosition.y += stepUp;
        transform.position = levelPosition;
    }
}
