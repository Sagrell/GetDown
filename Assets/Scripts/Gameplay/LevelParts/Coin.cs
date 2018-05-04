using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour, IPooledObject {

    public GameObject effect;
    public float rotationSpeed = 100f;

    [Header("Materials")]
    public Texture normalCoinTex;
    public Texture doubleCoinTex;
    public Texture tripleCoinTex;
    public Texture quadCoinTex;
    public Texture sixthCoinTex;
    public Texture eightCoinTex;
    [Header("Particle colors")]
    public Color normalCoin;
    public Color doubleCoin;
    public Color tripleCoin;
    public Color quadCoin;
    public Color sixthCoin;
    public Color eightCoin;

    bool isDouble;
    public int defaultValue;
    [HideInInspector]
    public int currentValue;
    public int doubleAfter = 200;
    public int tripleAfter = 400;
    Transform _RBTransform;
    Renderer rendererCoin;
    ParticleSystem.MainModule mainPS;
    [HideInInspector]
    public static Vector3 startPosition;
    [HideInInspector]
    public static Quaternion startRotation;
    private void Awake()
    {
        isDouble = false;
        mainPS = effect.GetComponent<ParticleSystem>().main;
        rendererCoin = GetComponent<Renderer>();
        if(!PurchaseManager.isDoubleCoins)
        {
            defaultValue = 1;
            currentValue = 1;
        } else
        {
            defaultValue = 2;
            currentValue = 2;
        }
        
        rendererCoin.material.SetTexture("_MainTex", normalCoinTex);
        startPosition = new Vector3(0,0.6f,0);
        startRotation = Quaternion.identity;
        _RBTransform = GetComponent<Rigidbody>().transform;
        GetComponent<MagnetToPlayer>().enabled = false;
    }

    public void OnObjectSpawn()
    {
        rendererCoin = GetComponent<Renderer>();
        if (!PurchaseManager.isDoubleCoins)
        {

            defaultValue = 1;
        }
        else
        {
            defaultValue = 2;
        }
        if (GameState.score> doubleAfter)
        {
            if(GameState.score > tripleAfter)
            {
                ChangeToTriple();
            } else
            {
                ChangeToDouble();
            } 
        }  else
        {
            ChangeTexture(currentValue);
        }
        _RBTransform = GetComponent<Rigidbody>().transform;
        GetComponent<MagnetToPlayer>().enabled = false;
    }

    public void Double()
    {
        currentValue *= 2;
        ChangeTexture(currentValue);

    }
    public void Normal()
    {
        currentValue = defaultValue;
        ChangeTexture(currentValue);
    }
    public void ChangeToDouble()
    {
        defaultValue += 1;
        currentValue = isDouble ? defaultValue*2 : defaultValue;
        ChangeTexture(currentValue);
    }
    public void ChangeToTriple()
    {
        defaultValue += 2;
        currentValue = isDouble ? defaultValue * 2 : defaultValue;
        ChangeTexture(currentValue);
    }

    void ChangeTexture(int value)
    {
        switch (value)
        {
            case 1:
                rendererCoin.material.SetTexture("_MainTex", normalCoinTex);
                mainPS.startColor = normalCoin;
                break;
            case 2:
                rendererCoin.material.SetTexture("_MainTex", doubleCoinTex);
                mainPS.startColor = doubleCoin;
                break;
            case 3:
                rendererCoin.material.SetTexture("_MainTex", tripleCoinTex);
                mainPS.startColor = tripleCoin;
                break;
            case 4:
                rendererCoin.material.SetTexture("_MainTex", quadCoinTex);
                mainPS.startColor = quadCoin;
                break;
            case 6:
                rendererCoin.material.SetTexture("_MainTex", sixthCoinTex);
                mainPS.startColor = sixthCoin;
                break;
            case 8:
                rendererCoin.material.SetTexture("_MainTex", eightCoinTex);
                mainPS.startColor = eightCoin;
                break;
        }
    }
    void Update()
    {
        _RBTransform.Rotate(Vector3.up, rotationSpeed * GameManager.deltaTime);
    }
    public void DestroyCoin()
    {
        AudioCenter.Instance.PlaySound("CoinCollect");
        GameObject coinEffect = Instantiate(effect, transform.position, Quaternion.identity, transform.parent);
        Destroy(coinEffect, 2f);
        gameObject.transform.SetParent(null);
        gameObject.SetActive(false);
    }
}
