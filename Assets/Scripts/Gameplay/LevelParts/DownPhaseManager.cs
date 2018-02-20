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

    void Start()
    {
        levelPosition = level.position;
        startLevelSpeed = levelSpeed;
        startPlayerSpeed = player.speed;
    }
    float stepUp;
    void FixedUpdate()
    {
        playerScreenPos = player.playerScreenPos;
        if (playerScreenPos.y <= Screen.height * 0.25f)
        {
            StopAllCoroutines();
            StartCoroutine(SpeedTo(true, startPlayerSpeed));
            //startLevelSpeed = player.startSpeed;
        } else
        {
            StopAllCoroutines();
            StartCoroutine(SpeedTo(false, 2f));
        }
        levelSpeed = startLevelSpeed * GameState.currentSpeedFactor;
        stepUp = levelSpeed * GameManager.fixedDeltaTime;
        levelPosition.y += stepUp;
        level.position = levelPosition;
    }

    IEnumerator SpeedTo(bool up, float speed)
    {
        if(up)
        {
            while(startLevelSpeed< speed)
            {
                startLevelSpeed += .02f;
                yield return new WaitForSeconds(.1f);
            }
            startLevelSpeed = speed;
        } else
        {
            while (startLevelSpeed > speed)
            {
                startLevelSpeed -= .02f;
                yield return new WaitForSeconds(.1f);
            }
            startLevelSpeed = speed;
        }
        
        
    }

}
