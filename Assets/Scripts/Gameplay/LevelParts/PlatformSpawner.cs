using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PlatformType
{
    Empty,
    Broken,
    Normal,
    WithCoin,
    WithShield
}
public class PlatformSpawner : MonoBehaviour {

    public Transform[] spawnPoints;

    public float coinChance = 0.1f;
    public float diamondChance = 0.05f;
    public float shieldCoinChance = 0.05f;
    public float doubleNormalChance = 0.2f;

    float addChance = 0.30f;

    bool isDiamond;
    bool isCoin;
    bool isShieldCoin;
    PlatformType[] prevWave;
    Transform _transform;
    Vector3 _localPosition;
    float spawnerDistance;

    float topCoordY;
    float spawnIn;
    ElementsPool objectPooler;
    private void Start()
    {
        topCoordY = 20f;
        spawnIn = 40f;
        _transform = transform;
        _localPosition = _transform.localPosition;
        
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

        objectPooler.pickFromPool("Normal", spawnPoints[0].position, spawnPoints[0].rotation, transform.parent);
        _localPosition.y -= 1.5f;
        spawnerDistance = Mathf.Abs(topCoordY - _transform.position.y);
        _transform.localPosition = _localPosition;
        while (spawnerDistance <= spawnIn)
        {
            SpawnNextWave();
            _localPosition.y -= 1.5f;
            _transform.localPosition = _localPosition;
            spawnerDistance = Mathf.Abs(topCoordY - _transform.position.y);
        }

        
    }

    void Update()
    {
        spawnerDistance = Mathf.Abs(topCoordY - _transform.position.y);
        if (spawnerDistance <= spawnIn)
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
            PlatformType type = nextWave[i];
            if (type == PlatformType.Empty)
            {
                continue;
            } else if (type == PlatformType.Broken)
            {
                objectPooler.pickFromPool("Broken", spawnPoints[i].position, transform.parent);
            }
            else if (type == PlatformType.Normal)
            {
                isCoin = Random.value <= coinChance ? true : false;
                isShieldCoin = Random.value <= 1 ? true : false;
                if (isCoin)
                {
                    if (objectPooler.pickFromPool("NormalWithCoin", spawnPoints[i].position, spawnPoints[i].rotation, transform.parent) != null)
                    {
                        continue;
                    }
                }
                if (isShieldCoin)
                {
                    if(!GameState.isShield)
                    {
                        if(objectPooler.pickFromPool("NormalWithShield", spawnPoints[i].position, spawnPoints[i].rotation, transform.parent)!=null)
                        {
                            continue;
                        }
                    }
                        
                }
                objectPooler.pickFromPool("Normal", spawnPoints[i].position, spawnPoints[i].rotation, transform.parent);
            }
        }
        isDiamond = Random.value <= diamondChance && !isDiamond ? true : false;
        if (isDiamond)
        {
            objectPooler.pickFromPool("Diamond", spawnPoints[8].position, spawnPoints[8].rotation, transform.parent);
        }
        prevWave = nextWave;
    }

}
