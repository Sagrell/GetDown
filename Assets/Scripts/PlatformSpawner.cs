using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformSpawner : MonoBehaviour {

    public Transform[] spawnPoints;
    public GameObject platform;
    public GameObject broken;
    public GameObject platforms;
    public GameObject coin;
    public GameObject diamond;

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
                GameObject newPlatform = Instantiate(broken, spawnPoints[i].position, broken.transform.rotation);
                newPlatform.transform.parent = platforms.transform;
            } else
            {
                GameObject newPlatform = Instantiate(platform, spawnPoints[i].position, spawnPoints[i].rotation);
                newPlatform.transform.parent = platforms.transform;

                bool isCoin = type == PlatformType.WithCoin ? true : false;
                if (isCoin)
                {
                    Instantiate(coin, newPlatform.transform);
                }
            }
            
        }
        if(isDiamond)
        {
            GameObject diamondObj = Instantiate(diamond, spawnPoints[8].position, spawnPoints[8].rotation);
            diamondObj.transform.parent = platforms.transform;
        }
       
    }
}
