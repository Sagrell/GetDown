using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PlatformType
{
    Empty,
    Broken,
    Normal
}
public class PlatformSpawner : MonoBehaviour {

    public Transform[] spawnPoints;
    public Transform content;
    public float coinChance = 0.1f;
    public float doubleCoinChance = 0.1f;
    public float diamondChance = 0.05f;
    public float shieldPowerUpChance = 0.05f;
    public float magnetPowerUpChance = 0.05f;
    public float doubleCoinPowerUpChance = 0.05f;
    public float fastRunPowerUpChance = 0.05f;
    public float spikeChance = 0.08f;
    public float laserChance = 0.05f;
    public float brokenAfter = 20f;
    public float spikeAfter = 40f;
    public float diamondAfter = 80f;
    public float laserAfter= 100f;

    bool isDoubleCoin;
    bool isFastRun;

    bool isDiamond;
    bool isCoin;
    bool isShieldPowerUp;
    bool isMagnetPowerUp;
    bool isDoubleCoinPowerUp;
    bool isFastRunPowerUp;
    bool isLaser;
    bool isSpike;

    PlatformType[] currentWave;
    Transform _transform;
    Vector3 _localPosition;
    float spawnerDistance;

    float topCoordY;
    float spawnIn;
    ElementsPool objectPooler;
    UserData data;
    private void Start()
    {
        data = DataManager.Instance.GetUserData();
        isDoubleCoin = false;
        topCoordY = 20f;
        spawnIn = 40f;
        _transform = transform;
        _localPosition = _transform.localPosition;

        currentWave = new PlatformType[6] {
        PlatformType.Normal,
        PlatformType.Empty,
        PlatformType.Empty,
        PlatformType.Empty,
        PlatformType.Empty,
        PlatformType.Empty};
        isCoin = false;
        isDiamond = true;
        isLaser = true;
        isSpike = true;
        objectPooler = ElementsPool.Instance;

        objectPooler.PickFromPool("Normal", spawnPoints[0].position, spawnPoints[0].rotation, transform.parent);
        _localPosition.y -= 1.5f;
        spawnerDistance = Mathf.Abs(topCoordY - _transform.position.y);
        _transform.localPosition = _localPosition;
        while (spawnerDistance <= spawnIn)
        {
            if (isFastRun)
                currentWave = GenerateFastWaveFromPrevious(currentWave);
            else 
                currentWave = GenerateNormalWaveFromPrevious(currentWave);
            SpawnWave(currentWave, isDoubleCoin);
            //Move to next position
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
            if (isFastRun)
                currentWave = GenerateFastWaveFromPrevious(currentWave);
            else
                currentWave = GenerateNormalWaveFromPrevious(currentWave);
            SpawnWave(currentWave, isDoubleCoin);
            _localPosition.y -= 1.5f;
            _transform.localPosition = _localPosition;
        }
    }
    public void ActivateDoubleCoins()
    {
        objectPooler.ChangeCoins();
        isDoubleCoin = true;
    }
    public void ActivateFastRun()
    {
        isFastRun = true;
        objectPooler.RemovePlatformsFromPool();
        MoveSpawnerTo(GameState.playerPositionY);
    }
    public void DeactivateFastRun()
    {
        isFastRun = false;
    }
    public void DeactivateDoubleCoins()
    {
        objectPooler.ChangeCoins();
        isDoubleCoin = false;
    }
    public void MoveSpawnerTo(int height)
    {
        int playerX = 0;
        for (int i = 0; i < currentWave.Length; i++)
        {
            if (i == GameState.playerPositionX)
            {
                playerX = i;
                currentWave[i] = PlatformType.Normal;
            }      
            else
                currentWave[i] = PlatformType.Empty;
        }
        _localPosition.y = height * -1.5f;
        _transform.localPosition = _localPosition;
        objectPooler.PickFromPool("Normal", spawnPoints[playerX].position, spawnPoints[playerX].rotation, transform.parent);
        _localPosition.y -= 1.5f;
        _transform.localPosition = _localPosition;

    }
    PlatformType[] GenerateFastWaveFromPrevious(PlatformType[] prevWave)
    {
        //For each previous platform create new "wave"
        PlatformType[] wave = new PlatformType[6];

        for (int i = 0; i < prevWave.Length; i++)
        {
            PlatformType prevPlatform = prevWave[i];
            //If previous platform is normal
            if (prevPlatform.Equals(PlatformType.Normal))
            {
                //Check for bounds
                if (i == 0)
                    wave[i + 1] = PlatformType.Normal;
                else if (i == (prevWave.Length - 1))
                    wave[i - 1] = PlatformType.Normal;
                else
                {
                    wave[i - 1] = PlatformType.Normal;
                    wave[i + 1] = PlatformType.Normal;
                }
            }
        }
        return wave;
    }

    float doubleNormalChance = 0.4f;
    float addChance = 0.38f;
    PlatformType[]  GenerateNormalWaveFromPrevious(PlatformType[] prevWave)
    {
        //For each previous platform create new "wave"
        PlatformType[] wave = new PlatformType[6];

        for (int i = 0; i < prevWave.Length; i++)
        {
            PlatformType prevPlatform = prevWave[i];
            //If previous platform is normal
            if (prevPlatform.Equals(PlatformType.Normal))
            {
                //Check for bounds
                if (i == 0)
                    wave[i + 1] = PlatformType.Normal;
                else if (i == (prevWave.Length - 1))
                    wave[i - 1] = PlatformType.Normal;
                else
                {
                    //If exists only on the left (on the right isnt possible)
                    if (wave[i - 1].Equals(PlatformType.Normal))
                    {
                        wave[i + 1] = (PlatformType)Random.Range(0, 2);
                        wave[i + 1] = Random.value <= doubleNormalChance ? PlatformType.Normal : wave[i + 1];
                    }  
                    //If doesn't exists, then create 1-2 
                    else
                    {
                        //Random create left platform
                        wave[i - 1] = (PlatformType)Random.Range(0, 3);
                        wave[i - 1] = Random.value <= addChance ? PlatformType.Normal : wave[i - 1];
                        //if left platform is normal
                        if (wave[i - 1] == PlatformType.Normal)
                        {
                            wave[i + 1] = (PlatformType)Random.Range(0, 2);
                            wave[i + 1] = Random.value <= doubleNormalChance ? PlatformType.Normal : wave[i + 1];
                        }
                        else
                        {
                           wave[i + 1] = PlatformType.Normal;
                        }
                    }
                }
            }
        }
        return wave;
    }

    void SpawnWave(PlatformType[] wave, bool isDoubleCoin)
    {
        
        for (int i = 0; i < wave.Length; i++)
        {
            PlatformType type = wave[i];
            if(type == PlatformType.Empty)
            {
                continue;
            }
            else if (type == PlatformType.Broken && GameState.score > brokenAfter)
            {
                 objectPooler.PickFromPool("Broken", spawnPoints[i].position, transform.parent); 
            }
            else if (type == PlatformType.Normal)
            {
                GameObject platform = null;
                
                isSpike = Random.value <= spikeChance ? true : false;
                if (isSpike && GameState.score > spikeAfter)
                    platform = objectPooler.PickFromPool("NormalWithSpike", spawnPoints[i].position, transform.parent);
                else
                    platform = objectPooler.PickFromPool("Normal", spawnPoints[i].position, spawnPoints[i].rotation, transform.parent);

                SpawnPowerUp(platform, isDoubleCoin);
            }
        }

        SpawnEnemies();
    }

    void SpawnPowerUp(GameObject platform, bool isDoubleCoin)
    {
        if(isDoubleCoin)
        {
            isCoin = Random.value <= doubleCoinChance ? true : false;
            if (isCoin)
                if (objectPooler.PickFromPool("DoubleCoin", platform.transform, DoubleCoin.startPosition, DoubleCoin.startRotation) != null)
                    return;
        } else
        {
            isCoin = Random.value <= coinChance ? true : false;
            if (isCoin)
                if (objectPooler.PickFromPool("Coin", platform.transform, Coin.startPosition, Coin.startRotation) != null)
                    return;
        }    
        isShieldPowerUp = Random.value <= shieldPowerUpChance ? true : false;
        if (isShieldPowerUp && !PowerUpManager.isShield && data.Shield[0]>0)
            if (objectPooler.PickFromPool("ShieldPowerUp", platform.transform, ShieldPowerUp.startPosition, ShieldPowerUp.startRotation) != null)
                return;
        isMagnetPowerUp = Random.value <= magnetPowerUpChance ? true : false;
        if (isMagnetPowerUp && !PowerUpManager.isMagnet && data.Magnet[0] > 0)
            if (objectPooler.PickFromPool("MagnetPowerUp", platform.transform, MagnetPowerUp.startPosition, MagnetPowerUp.startRotation) != null)
                return;
        isDoubleCoinPowerUp = Random.value <= doubleCoinPowerUpChance ? true : false;
        if (isDoubleCoinPowerUp && !PowerUpManager.isDoubleCoin && data.DoubleCoin[0] > 0)
            if (objectPooler.PickFromPool("DoubleCoinPowerUp", platform.transform, DoubleCoinPowerUp.startPosition, DoubleCoinPowerUp.startRotation) != null)
                return;

        isFastRunPowerUp = Random.value <= fastRunPowerUpChance ? true : false;
        if (isFastRunPowerUp && !PowerUpManager.isFastRun && data.FastRun[0] > 0)
            if (objectPooler.PickFromPool("FastRunPowerUp", platform.transform, FastRunPowerUp.startPosition, FastRunPowerUp.startRotation) != null)
                return;
    }

    void SpawnEnemies()
    {
        isDiamond = Random.value <= diamondChance && !isDiamond ? true : false;
        isLaser = Random.value <= laserChance && !isLaser ? true : false;

        if (isDiamond && GameState.score > diamondAfter)
            objectPooler.PickFromPool("Diamond", spawnPoints[6].position, spawnPoints[6].rotation, transform.parent);
        if (isLaser && GameState.score > laserAfter)
            objectPooler.PickFromPool("Laser", new Vector3(spawnPoints[GameState.playerPositionX].position.x, 0), Quaternion.identity, content);
    }
}
