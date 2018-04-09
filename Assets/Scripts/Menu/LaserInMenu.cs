using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserInMenu : MonoBehaviour {

    Animator laserAnimation;

    float minX;
    float maxX;

    Transform _transform;
    Vector3 newPosition;
    MeshRenderer rend;
    // Use this for initialization
    public void Start()
    {
        minX = -1.7f;
        maxX = 1.7f;
        _transform = transform;
        rend = GetComponent<MeshRenderer>();
        //GetComponentInChildren<ParticleSystem>().Play();
        laserAnimation = GetComponent<Animator>();
        newPosition = new Vector3(Random.Range(minX,maxX),0f, 1f);
        _transform.position = newPosition;
        StartCoroutine(LaserShot());
        
    }

    IEnumerator LaserShot()
    {
        while(true)
        {
            yield return new WaitForSeconds(Random.Range(0f, 5f));
            rend.enabled = true;
            AudioCenter.Instance.PlaySound("LaserStartMenu");
            yield return new WaitForSeconds(AudioCenter.Instance.GetSoundDuration("LaserStartMenu"));
            laserAnimation.SetBool("LaserOn", true);
            AudioCenter.Instance.PlaySound("LaserShotMenu");
            yield return new WaitForSeconds(AudioCenter.Instance.GetSoundDuration("LaserShotMenu"));
            laserAnimation.SetBool("LaserOn", false);
            yield return new WaitForSeconds(.25f);
            rend.enabled = false;
            newPosition.x = Random.Range(minX, maxX);
            _transform.position = newPosition;
        }   
    }
}
