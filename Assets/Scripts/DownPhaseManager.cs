using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum PlatformType
{
    Empty,
    Broken,
    Normal,
    WithCoin
}
public class DownPhaseManager : MonoBehaviour {

    public Transform level;
    public PlayerController player;

    public float levelSpeed = 1.5f;
    public float maxLevelSpeed = 8f;

    Vector3 levelPosition;
    void Start()
    {
        levelPosition = level.position;
    }

    private void FixedUpdate()
    {
        if (levelSpeed < maxLevelSpeed)
        {
            levelSpeed *= GameManager.difficultyIncrease;
        }
    }
    void Update()
    {
        float stepUp = levelSpeed * GameManager.deltaTime;
        levelPosition.y += stepUp;
        level.position = levelPosition;
    }
  
}
