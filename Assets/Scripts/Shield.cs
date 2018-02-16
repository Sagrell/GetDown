using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shield : MonoBehaviour, IPooledObject {


    public GameObject shieldEffect;

    Animator shieldHitAnim;
    float countShield;

    GameState GS;
    // Use this for initialization
    void Start () {
        shieldHitAnim = GetComponent<Animator>();
        shieldHitAnim.Play("ShieldAppearance");
        countShield = 1f;
        GS = GameState.Instance;
    }

    public void OnObjectSpawn()
    {
        shieldHitAnim = GetComponent<Animator>();
        shieldHitAnim.Play("ShieldAppearance");
        countShield = 1f;
        GS = GameState.Instance;
    }
    private void OnTriggerEnter(Collider other)
    {

        if (other.tag == "Enemy")
        {
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
        GS.TurnOffShield();
    }
    IEnumerator BlinkingShield()
    {
        shieldHitAnim.Play("ShieldBlink");
        yield return new WaitForSecondsRealtime(1.5f);
        gameObject.SetActive(false);
        Instantiate(shieldEffect, transform.position, transform.rotation, transform.parent.parent);
        GS.TurnOffShield();
    }
    
}
