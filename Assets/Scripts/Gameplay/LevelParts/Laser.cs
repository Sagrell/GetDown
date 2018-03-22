using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour, IPooledObject {

    Animator laserAnimation;


    bool isShooting;
    // Use this for initialization
    int layer_mask = (1 << 9) | (1 << 10);
    public void OnObjectSpawn()
    {
        //GetComponentInChildren<ParticleSystem>().Play();
        laserAnimation = GetComponent<Animator>();
        StartCoroutine(LaserShot());
    }
    // Update is called once per frame
    void FixedUpdate () {

        RaycastHit hit;
        if (Physics.Raycast(new Vector3(transform.position.x, 12), Vector3.down, out hit, 24f, layer_mask) && isShooting)
        {
            if (hit.collider.CompareTag("Player"))
            {
                hit.collider.gameObject.GetComponent<PlayerController>().DestroyPlayer();
            }
            if (hit.collider.CompareTag("Shield"))
            {
                hit.collider.gameObject.GetComponent<Shield>().HitShield();
            }
        }
    }

    IEnumerator LaserShot()
    {
        AudioCenter.Instance.PlaySound("LaserStart");
        yield return new WaitForSeconds(2f);
        laserAnimation.SetBool("LaserOn", true);
        isShooting = true;

        AudioCenter.Instance.PlaySound("LaserShot");
        yield return new WaitForSeconds(1.7f);
        laserAnimation.SetBool("LaserOn", false);
        isShooting = false;
        yield return new WaitForSeconds(.25f);
        gameObject.SetActive(false);
    }

    
}
