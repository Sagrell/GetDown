using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldCoin : MonoBehaviour, IPooledObject {

    public float rotationSpeed = 30f;

    Transform _RBTransform;

    void Start()
    {
        _RBTransform = GetComponent<Rigidbody>().transform;
    }
    public void OnObjectSpawn()
    {
       
    }

    void Update () {
        _RBTransform.Rotate(Vector3.up, rotationSpeed*GameManager.deltaTime);
	}
    public void DestroyShieldCoin()
    {
        AudioCenter.PlaySound(AudioCenter.shieldUpSoundId);
        gameObject.SetActive(false);
    }
}
