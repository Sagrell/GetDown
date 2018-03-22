using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoubleCoin : MonoBehaviour, IPooledObject {

    public GameObject effect;
    public float rotationSpeed = 100f;

    Transform _RBTransform;

    [HideInInspector]
    public static Vector3 startPosition;
    [HideInInspector]
    public static Quaternion startRotation;
    private void Awake()
    {
        startPosition = transform.position;
        startRotation = transform.rotation;
    }
    public void OnObjectSpawn()
    {
        _RBTransform = GetComponent<Rigidbody>().transform;
        GetComponent<MagnetToPlayer>().enabled = false;
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
