using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlatformSpawner : MonoBehaviour {

    public Transform[] spawnPoints;
    public Transform player;

    public float coinChance = 0.1f;
    public float diamondChance = 0.05f;
    public float doubleNormalChance = 0.2f;

    float addChance = 0.30f;

    bool isDiamond;
    bool isCoin;

    PlatformType[] prevWave;
    Transform _transform;
    Vector3 _localPosition;
    float playerSpawnerDistance;


    ElementsPool objectPooler;
    private void Start()
    {
        _transform = transform;
        _localPosition = _transform.localPosition;
        playerSpawnerDistance = Mathf.Abs(_localPosition.y - player.localPosition.y);
        prevWave = new PlatformType[8] {
        PlatformType.Normal,
        PlatformType.Empty,
        PlatformType.Empty,
        PlatformType.Empty,
        PlatformType.Empty,
        PlatformType.Empty,
        PlatformType.Empty,
        PlatformType.Empty};
        isCoin = false;
        isDiamond = true;

        objectPooler = ElementsPool.Instance;

        while (playerSpawnerDistance <= 30f)
        {
            SpawnNextWave();
            _localPosition.y -= 1.5f;
            _transform.localPosition = _localPosition;
            playerSpawnerDistance = Mathf.Abs(_localPosition.y - player.localPosition.y);
        }
    }

    void Update()
    {
        playerSpawnerDistance = Mathf.Abs(_localPosition.y - player.localPosition.y);
        if (playerSpawnerDistance <= 30f)
        {         
            SpawnNextWave();
            _localPosition.y -= 1.5f;
            _transform.localPosition = _localPosition;
        }
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
        SpawnWave(nextWave, isDiamond);
        prevWave = nextWave;
    }


    public void SpawnWave(PlatformType[] wave, bool isDiamond)
    {
        for (int i = 0; i < wave.Length; i++)
        {
            PlatformType type = wave[i];
            if(type == PlatformType.Empty)
            {
                continue;
            }
            if(type == PlatformType.Broken)
            {
                objectPooler.pickFromPool("Broken", spawnPoints[i].position, transform.parent);
            } else if(type == PlatformType.WithCoin)
            {
                objectPooler.pickFromPool("NormalWithCoin", spawnPoints[i].position, spawnPoints[i].rotation, transform.parent);
            } else if(type == PlatformType.Normal)
            {
                objectPooler.pickFromPool("Normal", spawnPoints[i].position, spawnPoints[i].rotation, transform.parent);
            }
            
        }
        if(isDiamond)
        {
            objectPooler.pickFromPool("Diamond", spawnPoints[8].position, spawnPoints[8].rotation, transform.parent);
        }
       
    }
}
