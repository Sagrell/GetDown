using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shield : MonoBehaviour, IPooledObject {


    public GameObject shieldEffect;

    Animator shieldHitAnim;
    public float countShield;

    public void OnObjectSpawn()
    {
        shieldHitAnim = GetComponent<Animator>();
        shieldHitAnim.Play("ShieldAppearance");
        countShield = PowerUpManager.Instance.shieldCount;
    }
    private void OnTriggerEnter(Collider other)
    {

        if (other.CompareTag("Enemy"))
        {
            other.GetComponent<Diamond>().DestroyDiamond();
            HitShield();
        }
    }
    public void HitShield()
    {
        AudioCenter.PlaySound(AudioCenter.shieldHitSoundId);
        ShakeManager.ShakeAfterShieldHit();
        countShield--;
        if (countShield <= 0f)
        {
            DestroyShield();
        }
        else
        {
            shieldHitAnim.Play("ShieldHit");
        }
    }
    public void DestroyShield()
    { 
        StartCoroutine(BlinkingShield());
    }
    public void DestroyShieldImmediately()
    {
        gameObject.SetActive(false);
        Instantiate(shieldEffect, transform.position, transform.rotation, transform.parent.parent);
    }
    IEnumerator BlinkingShield()
    {
        shieldHitAnim.Play("ShieldBlink");
        yield return new WaitForSecondsRealtime(1.5f);
        PowerUpManager.Instance.Deactivate("Shield");
    }
}
