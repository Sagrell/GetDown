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

    [Header("References")]
    public Animator anim;
    [Header("Power Ups:")]
    public float shieldPowerUpChance;
    public float magnetPowerUpChance;
    public float doubleCoinPowerUpChance;
    public float fastRunPowerUpChance;
    [Header("Coins:")]
    public float coinChance;

    [Space]
    [Header("Spike:")]
    public float spikeAfter;
    public float startSpikeChance;

    [Header("Broken:")]
    public float brokenAfter;
    [Header("Diamond:")]
    public float diamondAfter;
    public float startDiamondChance;
    [Space]
    [Header("Hard:")]
    public float hardAfter;
    public float startHardChance;
    [Space]
    [Header("Spawn points:")]
    public Transform[] spawnPoints;
    public Transform content;
    [Space]
    [Header("Patterns:")]
    public Pattern[] patterns;
    public GameObject learning;
    //PRIVATE
    bool isDoubleCoin;
    bool isFastRun;
    bool isDiamond;
    bool isCoin;
    bool isShieldPowerUp;
    bool isMagnetPowerUp;
    bool isDoubleCoinPowerUp;
    bool isFastRunPowerUp;
    bool isSpike;
    bool isHard;
    bool isLaserAttack;
    float diamondChance;
    float spikeChance;
    float hardChance;
    int laserAttackAfter;
    PlatformType[] currentWave;
    Transform _transform;
    Vector3 _localPosition;
    float spawnerDistance;

    float topCoordY;
    float spawnIn;
    int targetPlayerPosition;
    int spawnerPosition;
    ElementsPool objectPooler;
    UserData data;

    private void Start()
    {
        data = DataManager.Instance.GetUserData();
        isDoubleCoin = false;
        laserAttackAfter = Random.Range(80, 101);
        topCoordY = 20f;
        spawnIn = 40f;
        _transform = transform;
        _localPosition = _transform.localPosition;
        
        isCoin = false;
        isDiamond = true;
        isLaserAttack = false;
        isSpike = true;
        objectPooler = ElementsPool.Instance;
        if(!data.isFirstTime)
        {
            currentWave = new PlatformType[6] {
                PlatformType.Normal,
                PlatformType.Empty,
                PlatformType.Empty,
                PlatformType.Empty,
                PlatformType.Empty,
                PlatformType.Empty};
            objectPooler.PickFromPool("Normal", spawnPoints[0].position, spawnPoints[0].rotation, transform.parent);
        } else
        {
            GameObject learn = Instantiate(learning, transform.parent);
            foreach(Platform plat in learn.GetComponentsInChildren<Platform>())
            {
                plat.GetComponent<Renderer>().sharedMaterial = SkinManager.platformMat;
            }      
            _localPosition.y -= 1.5f*49f;
            _transform.localPosition = _localPosition;
            currentWave = new PlatformType[6] {
                PlatformType.Empty,
                PlatformType.Empty,
                PlatformType.Empty,
                PlatformType.Normal,
                PlatformType.Empty,
                PlatformType.Empty};
            objectPooler.PickFromPool("Normal", spawnPoints[3].position, spawnPoints[3].rotation, transform.parent);
        }

        _localPosition.y -= 1.5f;
        _transform.localPosition = _localPosition;
        spawnerDistance = Mathf.Abs(topCoordY - _localPosition.y);
        while (spawnerDistance <= spawnIn)
        {
            if (isFastRun)
            {
                currentWave = GenerateFastWaveFromPrevious(currentWave);
                SpawnWaveWithoutEnemy(currentWave, isDoubleCoin);
            }
            else
            {
                currentWave = GenerateNormalWaveFromPrevious(currentWave);
                SpawnWave(currentWave, isDoubleCoin);
            }


            //Move to next position
            _transform.localPosition = _localPosition;
            spawnerDistance = Mathf.Abs(topCoordY - _localPosition.y);
        }
        StartCoroutine("Spawn");
    }

    IEnumerator Spawn()
    {

        while(true)
        {
            spikeChance = startSpikeChance * GameState.currentSpeedFactor;
            diamondChance = startDiamondChance * GameState.currentSpeedFactor;
            hardChance = startHardChance * GameState.currentSpeedFactor;
            spawnerDistance = Mathf.Abs(topCoordY - _transform.position.y);
            if (spawnerDistance <= spawnIn)
            {
                if (isFastRun || isLaserAttack)
                {
                    currentWave = GenerateFastWaveFromPrevious(currentWave);
                    SpawnWaveWithoutEnemy(currentWave, isDoubleCoin);
                }
                else
                {
                    currentWave = GenerateNormalWaveFromPrevious(currentWave);
                    SpawnWave(currentWave, isDoubleCoin);
                }

                _transform.localPosition = _localPosition;
                if (GameState.score >= laserAttackAfter && !isLaserAttack)
                {
                    isLaserAttack = true;
                    spawnerPosition = (int)(_localPosition.y / -1.5f);
                    targetPlayerPosition = spawnerPosition;
                    StartCoroutine(LaserAttack());
                }
            }

            yield return new WaitForSeconds(.25f);
        }
    }
    IEnumerator LaserAttack()
    {
        while(GameState.playerPositionY < targetPlayerPosition-2)
        {
            yield return null;
        }
        anim.Play("LaserAttack");
        while (GameState.playerPositionY < targetPlayerPosition)
        {
            yield return null;
        }
        float laserCount = Random.Range(4, 9);
        while(laserCount>0)
        {
            Laser laser = objectPooler.PickFromPool("Laser", new Vector3(spawnPoints[GameState.playerPositionX].position.x, 0), Quaternion.identity, content).GetComponent<Laser>();
            while (laser.gameObject.activeSelf && !laser.isOver)
                yield return new WaitForSeconds(.1f);
            --laserCount;
            yield return null;
        }
        laserAttackAfter = GameState.score + Random.Range(80, 101);
        isLaserAttack = false;
    }
    public void Restart()
    {
        StopCoroutine("Spawn");
        objectPooler.RemovePlatformsFromPool(false);
        if(isLaserAttack)
        {
            StopAllCoroutines();
            isLaserAttack = false;
            laserAttackAfter = GameState.score + Random.Range(80, 101);
        }
        objectPooler.RemoveEnemyiesFromPool();
        MoveSpawnerTo(GameState.playerPositionY);
        currentWave = new PlatformType[6];
        currentWave[GameState.playerPositionX] = PlatformType.Normal;
        spawnerDistance = Mathf.Abs(topCoordY - _transform.position.y);
        while (spawnerDistance <= spawnIn)
        {
            currentWave = GenerateNormalWaveFromPrevious(currentWave);
            SpawnWaveWithoutEnemy(currentWave, isDoubleCoin);
            _transform.localPosition = _localPosition;
            spawnerDistance = Mathf.Abs(topCoordY - _transform.position.y);
        }
        StartCoroutine("Spawn");
    }
    public void ActivateDoubleCoins()
    {
        objectPooler.DoubleCoins();
        isDoubleCoin = true;
    }
    public void ActivateFastRun()
    {
        isFastRun = true;
        objectPooler.RemovePlatformsFromPool(true);
        objectPooler.RemoveEnemyiesFromPool();
        MoveSpawnerTo(GameState.playerPositionY);
    }
    public void DeactivateFastRun()
    {
        isFastRun = false;
    }
    public void DeactivateDoubleCoins()
    {
        objectPooler.NormalCoins();
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
                
                isSpike = Random.value <= spikeChance  && !isSpike ? true : false;  
                if (isSpike && GameState.score > spikeAfter)
                {
                    platform = objectPooler.PickFromPool("NormalWithSpike", spawnPoints[i].position, transform.parent);
                }
                if(!platform)
                    platform = objectPooler.PickFromPool("Normal", spawnPoints[i].position, spawnPoints[i].rotation, transform.parent);

                SpawnPowerUp(platform, isDoubleCoin);
            }
        }
        SpawnEnemies();
        _localPosition.y -= 1.5f;
        isHard = Random.value <= hardChance ? true : false;
        if (isHard && GameState.score > hardAfter && (wave[1]==PlatformType.Normal || wave[3] == PlatformType.Normal || wave[5] == PlatformType.Normal ) && !isLaserAttack)
        {
            SpawnHardPattern(wave);
        }
    }


    void SpawnHardPattern(PlatformType[] wave)
    {
        GameObject platform = null;
        wave = GenerateFastWaveFromPrevious(wave);
        _transform.localPosition = _localPosition;
        SpawnWaveWithoutEnemy(wave, isDoubleCoin);
        _transform.localPosition = _localPosition;
        if (wave[0] == PlatformType.Normal && wave[2] != PlatformType.Normal && wave[4] != PlatformType.Normal)
        {
            platform = objectPooler.PickFromPool("Normal", spawnPoints[1].position, spawnPoints[1].rotation, transform.parent);
            SpawnPowerUp(platform, isDoubleCoin);
        }
        else if (wave[0] != PlatformType.Normal && wave[2] != PlatformType.Normal && wave[4] == PlatformType.Normal)
        {
            platform = objectPooler.PickFromPool("Normal", spawnPoints[3].position, spawnPoints[3].rotation, transform.parent);
            SpawnPowerUp(platform, isDoubleCoin);
        }
        else
        {
            platform = objectPooler.PickFromPool("Normal", spawnPoints[1].position, spawnPoints[1].rotation, transform.parent);
            SpawnPowerUp(platform, isDoubleCoin);
            platform = objectPooler.PickFromPool("Normal", spawnPoints[3].position, spawnPoints[3].rotation, transform.parent);
            SpawnPowerUp(platform, isDoubleCoin);
        }
        _localPosition.y -= 1.5f;

        Pattern currentPattern = patterns[Random.Range(0, patterns.Length)];
        PatternItem[] patternItems = currentPattern.GetComponentsInChildren<PatternItem>();
        for (int i = 0; i < patternItems.Length; i++)
        {
            PatternItem item = patternItems[i];
            Vector3 pos = _localPosition + item.transform.localPosition;
            switch (item.type)
            {
                case "Normal":
                    platform = objectPooler.PickFromPool("Normal", transform.parent, pos, Quaternion.identity);
                    SpawnPowerUp(platform, isDoubleCoin);
                    break;
                case "Broken":
                    objectPooler.PickFromPool("Broken", transform.parent, pos, Quaternion.identity);
                    break;
                case "WithSpike":
                    platform = objectPooler.PickFromPool("NormalWithSpike", transform.parent, pos, Quaternion.identity);
                    SpawnPowerUp(platform, isDoubleCoin);
                    break;
                case "Diamond":
                    objectPooler.PickFromPool("Diamond", transform.parent, pos, Quaternion.identity);
                    break;
            }
        }
        _localPosition.y -= 1.5f * currentPattern.distance;
        currentWave = new PlatformType[6];
        currentWave[2] = PlatformType.Normal;
    }


    void SpawnWaveWithoutEnemy(PlatformType[] wave, bool isDoubleCoin)
    { 
        for (int i = 0; i < wave.Length; i++)
        {
            PlatformType type = wave[i];
            if (type == PlatformType.Empty)
            {
                continue;
            }
            else if (type == PlatformType.Broken && GameState.score > brokenAfter)
            {
                objectPooler.PickFromPool("Broken", spawnPoints[i].position, transform.parent);
            }
            else if (type == PlatformType.Normal)
            {

                GameObject platform = objectPooler.PickFromPool("Normal", spawnPoints[i].position, spawnPoints[i].rotation, transform.parent);
                if (isFastRun)
                {
                    GameObject pc = objectPooler.PickFromPool("PlatformChange", platform.transform.position, platform.transform.rotation, transform.parent);
                    if (pc != null)
                    {
                        pc.GetComponent<ParticleSystem>().Play();
                    }  
                }
                SpawnPowerUp(platform, isDoubleCoin);
            }
        }
        _localPosition.y -= 1.5f;
    }
    void SpawnPowerUp(GameObject platform, bool isDoubleCoin)
    {
        isCoin = Random.value <= coinChance ? true : false;
        if (isCoin)
           if (objectPooler.PickFromPool("Coin", platform.transform, Coin.startPosition, Coin.startRotation) != null)
               return;  
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

        if (isDiamond && GameState.score > diamondAfter)
            objectPooler.PickFromPool("Diamond", spawnPoints[6].position, spawnPoints[6].rotation, transform.parent);  
    }
}
