﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour, IPooledObject {

    Animator laserAnimation;

    public bool isOver;
    bool isShooting;
    // Use this for initialization
    int layer_mask = (1 << 9) | (1 << 10);
    public void OnObjectSpawn()
    {
        isOver = false;
        isShooting = false;
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
                hit.collider.gameObject.GetComponent<PlayerController>().DestroyPlayer(false);
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
        float soundDuration = AudioCenter.Instance.GetSoundDuration("LaserStart");
        yield return new WaitForSeconds(soundDuration*3/4);
        isOver = true;
        yield return new WaitForSeconds(soundDuration*1/4);
        laserAnimation.SetBool("LaserOn", true);
        isShooting = true;
        AudioCenter.Instance.PlaySound("LaserShot");
        yield return new WaitForSeconds(AudioCenter.Instance.GetSoundDuration("LaserShot"));
        laserAnimation.SetBool("LaserOn", false);
        isShooting = false;
        yield return new WaitForSeconds(.25f);
       
        gameObject.SetActive(false);
    }

    
}
