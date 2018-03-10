﻿using System.Collections;
using UnityEngine;

public class PowerUpManager : MonoBehaviour {

    #region Singleton
        public static PowerUpManager Instance;

	    void Awake () {
            Instance = this;
        }
    #endregion

    public static bool isShield;
    public static bool isMagnet;
    public static bool isDoubleCoin;
    public static bool isFastRun;
    [Header("Platform spawner:")]
    public PlatformSpawner spawner;
    [Space]
    [Header("Magnet:")]
    public float magnetTimer = 20f;
    [Space]
    [Header("Double coins:")]
    public float doubleCoinTimer = 20f;
    [Space]
    [Header("Shield:")]
    public float shieldCount = 1f;
    public float shieldTimer = 50f;
    [Space]
    [Header("Fast run:")]
    public float fastRunTimer = 50f;
    PlayerController player;
    //Player transform
    Transform _PTransform;

    //Objects
    ElementsPool objectPooler;
    GameObject magnet;
    GameObject shield;
    private void Start()
    {
        player = GameManager.player;
        _PTransform = player.transform;
        objectPooler = ElementsPool.Instance;
    }
   
    public void Activate(string powerUp)
    {
        switch (powerUp)
        {
            case "FastRun":
                spawner.ActivateFastRun();
                isFastRun = true;
                InGameGUI.Instance.StartPowerUp(powerUp, fastRunTimer);
                break;
            case "DoubleCoins":
                spawner.ActivateDoubleCoins();
                isDoubleCoin = true;
                InGameGUI.Instance.StartPowerUp(powerUp, doubleCoinTimer);
                break;
            case "Shield":
                shield = objectPooler.PickFromPool("Shield", _PTransform);
                isShield = true;
                InGameGUI.Instance.StartPowerUp(powerUp, shieldTimer);
                break;
            case "Magnet":
                magnet = objectPooler.PickFromPool("Magnet", _PTransform.position, Quaternion.identity, _PTransform.parent);
                isMagnet = true;
                InGameGUI.Instance.StartPowerUp(powerUp, magnetTimer);
                break;

            default:
                Debug.Log("Wrong name of power up!");
                break;
        }
    }
    public void Deactivate(string powerUp)
    {
        switch (powerUp)
        {
            case "FastRun":
                spawner.DeactivateFastRun();
                isFastRun = false;
                InGameGUI.Instance.StopPowerUp(powerUp);
                break;
            case "DoubleCoins":
                spawner.DeactivateDoubleCoins();
                isDoubleCoin = false;
                InGameGUI.Instance.StopPowerUp(powerUp);
                break;
            case "Shield":
                shield.GetComponent<Shield>().DestroyShieldImmediately();
                isShield = false;
                InGameGUI.Instance.StopPowerUp(powerUp);
                break;
            case "Magnet":
                magnet.GetComponent<Magnet>().DestroyMagnet();
                isMagnet = false;
                InGameGUI.Instance.StopPowerUp(powerUp);
                break;

            default:
                Debug.Log("Wrong name of power up!");
                break;
        }
    }

    public void DeactivateAll()
    {
        if (isMagnet)
        {
            Deactivate("Magnet");
        }
        if (isShield)
        {
            Deactivate("Shield");
        }
        if (isDoubleCoin)
        {
            Deactivate("DoubleCoins");
        }
        if (isFastRun)
        {
            Deactivate("FastRun");
        }
    }

}
