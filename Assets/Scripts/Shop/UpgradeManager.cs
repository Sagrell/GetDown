using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeManager : MonoBehaviour {

    public static UpgradeManager Instance;

    private void Awake()
    {
        Instance = this;
    }
    private void OnDestroy()
    {
        Instance = null;
    }


    public ShieldUpgrade shield;
    public MagnetUpgrade magnet;
    public DoubleCoinsUpgrade doubleCoins;
    public FastRunUpgrade fastRun;

    public GameObject point;
    public GameObject emptyPoint;

    public void Initialize()
    {
        shield.point = point;
        shield.emptyPoint = emptyPoint;
        shield.Initialize();
        magnet.Initialize();
        doubleCoins.Initialize();
        fastRun.Initialize();
    }
    // Use this for initialization
    
}
