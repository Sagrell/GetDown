
using System.Collections;
using UnityEngine;

public class Spike : Enemy {

    public GameObject destroyed;
    public override void Destroy()
    {
        AudioCenter.Instance.PlaySound("DiamondDestroy");
        GameObject parts = Instantiate(destroyed, transform.position, transform.rotation);
        Destroy(parts, 3f);
        GetComponentInParent<Animator>().enabled = false;
        gameObject.SetActive(false);
    }
}
