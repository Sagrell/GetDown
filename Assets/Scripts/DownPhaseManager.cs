using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DownPhaseManager : MonoBehaviour {

    public Transform level;
    public PlayerController player;
    public Diamond diamond;

    public float levelSpeed = 1f;
    public float maxLevelSpeed = 8f;
    public float maxPlayerSpeed = 8f;
    public float maxDiamondSpeed = 20f;

    Vector3 playerScreenPos;
    float playerSpeed;
    float startPlayerSpeed;
    float diamondSpeed;
    float startDiamondSpeed;
    Vector3 levelPosition;
    float startLevelSpeed;

    Camera mainCamera;
    GameState GS;

    //Cache
    Transform _PlayerRBTransform;

    void Start()
    {
        _PlayerRBTransform = player._RBTransform;
        levelPosition = level.position;
        startLevelSpeed = levelSpeed;
        playerSpeed = player.speed;
        startPlayerSpeed = playerSpeed;
        diamondSpeed = diamond.speed;
        startDiamondSpeed = diamondSpeed;
        GS = GameState.Instance;
        mainCamera = Camera.main;
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
        levelSpeed = startLevelSpeed * GS.getSpeedFactor();
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
