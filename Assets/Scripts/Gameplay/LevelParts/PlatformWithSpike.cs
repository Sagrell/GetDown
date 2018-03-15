using System.Collections;
using UnityEngine;

public class PlatformWithSpike : MonoBehaviour, IPooledObject {
    public float interval = 5f;

    Animator anim;
    public void OnObjectSpawn()
    {
        anim = GetComponent<Animator>();
        StartCoroutine(ActivateSpikes(interval));
    }
    private void Start()
    {
        anim = GetComponent<Animator>();
        StartCoroutine(ActivateSpikes(interval));
    }
    public Transform spikes;

    IEnumerator ActivateSpikes(float interval)
    {
        bool isActive = true;
        while (true)
        {
            yield return  new WaitForSeconds(interval);
            isActive = !isActive;
            anim.SetBool("isActive", isActive);
        }
        
    }
        
}
