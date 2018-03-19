using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrokenPlatform : MonoBehaviour, IPooledObject {

    public GameObject partPlatform;
    Texture tex;
    // Use this for initialization
    void Start() {
        tex = SkinManager.platformMat.GetTexture("_MainTex");
        Renderer[] platformParts = partPlatform.GetComponentsInChildren<Renderer>();
        for (int i = 0; i < platformParts.Length; ++i)
        {
            platformParts[i].sharedMaterial.SetTexture("_MainTex", tex);
        }
    }
    public void OnObjectSpawn()
    {
        
    }

    // Update is called once per frame
    void Update() {

    }
    public void destroyPlatform()
    {
        GameObject coinEffect = Instantiate(partPlatform, transform.position, transform.rotation);
        Destroy(coinEffect, 2f);
        gameObject.SetActive(false);
    }
}
