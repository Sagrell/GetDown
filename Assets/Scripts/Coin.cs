using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour {

    public GameObject effect;
    public float rotationSpeed = 30f;

	void Update () {
        transform.Rotate(Vector3.up, rotationSpeed*GameManager.deltaTime);
	}
    public void destroyCoin()
    {
        GameObject coinEffect = Instantiate(effect, transform.parent);
        Destroy(coinEffect, 2f);
        Destroy(gameObject);
    }
}
