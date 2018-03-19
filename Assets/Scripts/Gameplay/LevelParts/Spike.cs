
using UnityEngine;

public class Spike : Enemy {
    

    public override void Destroy()
    {
        AudioCenter.PlaySound("DiamondDestroy");
        ElementsPool.Instance.PickFromPool("DestroyedSpike", transform.position, transform.rotation);    
        GetComponentInParent<Animator>().enabled = false;
        gameObject.SetActive(false);
    }
}
