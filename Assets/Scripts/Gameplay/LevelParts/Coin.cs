using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour, IPooledObject {

    public GameObject effect;
    public float rotationSpeed = 30f;

    Rigidbody rb;
    Transform _RBTransform;

    void Start()
    {  
        rb = GetComponent<Rigidbody>();
        _RBTransform = rb.transform;
    }
    public void OnObjectSpawn()
    {
    }

    void Update () {
        _RBTransform.Rotate(Vector3.up, rotationSpeed*GameManager.deltaTime);
	}
    public void destroyCoin()
    {
        AudioCenter.PlaySound(AudioCenter.coinCollectsoundId);
        GameObject coinEffect = Instantiate(effect, transform.parent);
        Destroy(coinEffect, 2f);
        gameObject.SetActive(false);
    }
}
