using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelPart : MonoBehaviour {

    float destroyAfter = 20f;
	void Update () {

        if (transform.position.y >= destroyAfter)
        {
            Destroy(gameObject);
        }
	}
}
