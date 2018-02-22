﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shield : MonoBehaviour, IPooledObject {


    public GameObject shieldEffect;

    Animator shieldHitAnim;
    float countShield;

    // Use this for initialization
    void Awake () {
        shieldHitAnim = GetComponent<Animator>();
        shieldHitAnim.Play("ShieldAppearance");
        countShield = 1f;
    }

    public void OnObjectSpawn()
    {
        shieldHitAnim = GetComponent<Animator>();
        shieldHitAnim.Play("ShieldAppearance");
        countShield = 1f;
    }
    private void OnTriggerEnter(Collider other)
    {

        if (other.CompareTag("Enemy"))
        {
            AudioCenter.PlaySound(AudioCenter.shieldHitSoundId);

            countShield--;
            other.GetComponent<Diamond>().DestroyDiamond();
            if (countShield <= 0f)
            {
                DestroyShield();
            } else
            {
                shieldHitAnim.Play("ShieldHit");
            }
        }
    }

    public void DestroyShield()
    {
        StartCoroutine(BlinkingShield());
    }
    public void DestroyShieldimmediately()
    {
        gameObject.SetActive(false);
        Instantiate(shieldEffect, transform.position, transform.rotation, transform.parent.parent);
        GameState.isShield = false;
    }
    IEnumerator BlinkingShield()
    {
        shieldHitAnim.Play("ShieldBlink");
        yield return new WaitForSecondsRealtime(1.5f);
        gameObject.SetActive(false);
        Instantiate(shieldEffect, transform.position, transform.rotation, transform.parent.parent);
        GameState.isShield = false;
    }
    
}