using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrokenPlatform : MonoBehaviour {

    public GameObject partPlatform;
    // Use this for initialization
    void Start() {

    }

    // Update is called once per frame
    void Update() {

    }
    public void destroyPlatform()
    {
        GameObject coinEffect = Instantiate(partPlatform, transform.position, transform.rotation);
        Destroy(coinEffect, 2f);
        Destroy(gameObject);
    }
}
