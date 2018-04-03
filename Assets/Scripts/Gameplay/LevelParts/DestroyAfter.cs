using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyAfter : MonoBehaviour {

    float destroyAfter;

    Transform _transform;
    ElementsPool pool;
	void Start () {
        destroyAfter = 9f;
        _transform = transform;
        pool = ElementsPool.Instance;
    }
	
	// Update is called once per frame
	void Update () {
		if(_transform.position.y>destroyAfter && gameObject.activeSelf)
        {
            GameObject ps = pool.PickFromPool("PlatformBlowEffect", _transform.position, _transform.rotation, _transform.parent);
            if(ps)
            {
                ps.GetComponent<ParticleSystem>().Play();
            }
            gameObject.SetActive(false);
        }
	}
}
