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
    public PlatformSpawner spawner;

    public float levelSpeed = 1.5f;
    public float difficultyIncrease = 0.01f;
    public float maxLevelSpeed = 8f;

    public float coinChance = 0.1f;
    public float diamondChance = 0.05f;
    public float doubleNormalChance = 0.2f;

    float nextSpawnIn;
    PlatformType[] prevWave;
    Vector3 levelPosition;
    float addChance = 0.30f;
    bool isDiamond = true;
    void Start()
    {

        levelPosition = level.position;
        nextSpawnIn = 0.0f;
        prevWave = new PlatformType[8] {
        PlatformType.Empty,
        PlatformType.Empty,
        PlatformType.Empty,
        PlatformType.Empty,
        PlatformType.Normal,
        PlatformType.Empty,
        PlatformType.Empty,
        PlatformType.Empty};
    }


    void FixedUpdate()
    {
        if (levelSpeed < maxLevelSpeed)
        {
            levelSpeed += difficultyIncrease * GameManager.fixedDeltaTime;
        }
        float stepUp = levelSpeed * GameManager.fixedDeltaTime;

        nextSpawnIn -= stepUp;
        if (nextSpawnIn <= 0f)
        {
            SpawnNextWave();
            nextSpawnIn = 1.5f;
        }
        levelPosition.y += stepUp;
        level.position = levelPosition;
    }
    void SpawnNextWave()
    {

        //For each previous platform create new "wave"
        PlatformType[] nextWave = new PlatformType[8];
        //string log = "";

        for (int i = 0; i < prevWave.Length; i++)
        {
            PlatformType prevPlatform = prevWave[i];
            //Check next wave for existing platforms
            //If previous platform isnt broken or empty
            if (!prevPlatform.Equals(PlatformType.Empty) && !prevPlatform.Equals(PlatformType.Broken))
            {
                //Check for bounds
                if (i == 0)
                {
                    nextWave[i + 1] = PlatformType.Normal;
                }
                else if (i == (prevWave.Length - 1))
                {
                    nextWave[i - 1] = PlatformType.Normal;
                }
                else
                {
                    PlatformType left = nextWave[i - 1];
                    //If exists only on the left (on the right isnt possible)
                    if (left != PlatformType.Empty)
                    {
                        //If its normal platform, then create new normal/broken/empty on the right
                        if (left == PlatformType.Normal)
                        {
                            nextWave[i + 1] = (PlatformType)Random.Range(0, 2);
                            nextWave[i + 1] = Random.value <= doubleNormalChance ? PlatformType.Normal : nextWave[i + 1];
                        } //else create normal 
                        else
                        {
                            nextWave[i + 1] = PlatformType.Normal;
                        }
                    } //If doesn't exists, then create 1-2 
                    else
                    {
                        //Random create left platform
                        nextWave[i - 1] = (PlatformType)Random.Range(0, 3);
                        nextWave[i - 1] = Random.value <= addChance ? PlatformType.Normal : nextWave[i - 1];

                        //if left platform is normal
                        if (nextWave[i - 1] == PlatformType.Normal)
                        {
                            nextWave[i + 1] = (PlatformType)Random.Range(0, 2);
                            nextWave[i + 1] = Random.value <= doubleNormalChance ? PlatformType.Normal : nextWave[i + 1];
                        }
                        else
                        {
                            nextWave[i + 1] = PlatformType.Normal;
                        }
                    }
                }

            }
        }
        for (int i = 0; i < nextWave.Length; i++)
        {
            if (nextWave[i] == PlatformType.Normal)
            {
                bool isCoin = Random.value <= coinChance ? true : false;
                /*if (i < 4)
                {
                    leftCount++;
                }
                else
                {
                    rightCount++;
                }*/
                nextWave[i] = isCoin ? PlatformType.WithCoin : PlatformType.Normal;
            }
        }
        //Debug.Log("left: "+leftCount+" right: "+ rightCount);*/
        isDiamond = Random.value <= diamondChance && !isDiamond ? true : false;
        spawner.SpawnWave(nextWave, isDiamond);
        prevWave = nextWave;
    }
}
