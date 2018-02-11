using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shield : MonoBehaviour {


    public GameObject shieldEffect;
    public float countShield = 2f;

    Animator shieldHitAnim;
    // Use this for initialization
    void Start () {
        shieldHitAnim = GetComponent<Animator>();
    }
	

    private void OnTriggerEnter(Collider other)
    {

        if (other.tag == "Enemy")
        {
            countShield--;
            other.GetComponent<Diamond>().destroyDiamond();
            Destroy(other.gameObject);
            shieldHitAnim.Play("ShieldHit");
            if (countShield <= 0f)
            {
                DestroyShield();
            }
        }
    }

    public void DestroyShield()
    {
        GameObject effect = Instantiate(shieldEffect, transform.position, transform.rotation);
        effect.transform.SetParent(transform.parent.parent);
        Destroy(gameObject);
    }

}
