using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shield : MonoBehaviour, IPooledObject {


    public GameObject shieldEffect;

    Animator shieldHitAnim;
    float countShield;
    CameraShakeInstance _shakeInstance;

    int shieldHitSoundId;
    // Use this for initialization
    void Start () {
        shieldHitSoundId = AudioCenter.loadSound("ShieldHit");
        shieldHitAnim = GetComponent<Animator>();
        shieldHitAnim.Play("ShieldAppearance");
        countShield = 1f;
        _shakeInstance = CameraShaker.Instance.StartShake(3, 10, 0, new Vector3(0.4f,0.2f), new Vector3(0, 0, 0));
        _shakeInstance.StartFadeOut(0);
        _shakeInstance.DeleteOnInactive = false;
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
            AudioCenter.playSound(shieldHitSoundId);
            _shakeInstance.StartFadeIn(0);
            _shakeInstance.StartFadeOut(0.5f);
            countShield--;
            other.GetComponent<Diamond>().destroyDiamond();
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
