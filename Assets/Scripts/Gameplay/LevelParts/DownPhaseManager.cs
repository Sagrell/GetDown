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
            StopAllCoroutines();
            StartCoroutine(SpeedTo(startPlayerSpeed));
            //startLevelSpeed = player.startSpeed;
        } else if((playerScreenPos.y > Screen.height * 0.25f)&&(playerScreenPos.y < Screen.height * 0.7f))
        {
            StopAllCoroutines();
            StartCoroutine(SpeedTo(2f));
        } else
        {
            StartCoroutine(SpeedTo(0f));
        }
        levelSpeed = startLevelSpeed * GameState.currentSpeedFactor;
        stepUp = levelSpeed * GameManager.fixedDeltaTime;
        levelPosition.y += stepUp;
        level.position = levelPosition;
    }

    IEnumerator SpeedTo(float speed)
    {
        while(startLevelSpeed< speed)
        {
            startLevelSpeed += .02f;
            yield return new WaitForSeconds(.1f);
        }
        while (startLevelSpeed > speed)
        {
            startLevelSpeed -= .02f;
            yield return new WaitForSeconds(.1f);
        }
        startLevelSpeed = speed;       
    }

}
