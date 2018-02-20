using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SpiralPlatformType
{
    Empty,
    Broken,
    Normal,
    WithCoin
}
public class SpiralPhaseManager : MonoBehaviour {

    /*public Transform level;
    public GameObject player;
    public PlatformSpawner spawner;
    public float levelSpeed = 1;

    PlayerController playerController;
    float nextSpawnIn = 0f;
    PlatformType[] prevWave;
    Vector3 levelPosition;
    void Start()
    {
        playerController = player.GetComponent<PlayerController>();
        levelPosition = level.position;
    }


    void Update()
    {

        float stepUp = levelSpeed * Time.deltaTime;

        nextSpawnIn -= stepUp;
        if (nextSpawnIn <= 0f)
        {
            SpawnNextPlatform();
            nextSpawnIn = 1.5f;
        }
        //levelPosition.y += stepUp;
        //level.position = levelPosition;

    }

    void SpawnNextPlatform()
    {

    }*/
}
