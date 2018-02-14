using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Diamond : MonoBehaviour, IPooledObject
{

    public GameObject destroyVersion;
    public float speed = 5f;
    public float rotationSpeed = 10f;

    Rigidbody rb;
    Transform _RBTransform;
    Vector3 currPosition;
    Vector3 rightPosition;
    Vector3 leftPosition;
    Vector3 nextPosition;
    // Use this for initialization
    void Start () {
        rb = GetComponent<Rigidbody>();
        _RBTransform = rb.transform;
        currPosition = _RBTransform.localPosition;
        leftPosition = new Vector3(-5.25f, currPosition.y);
        rightPosition = new Vector3(5.25f, currPosition.y);
        nextPosition = rightPosition;
    }

    public void OnObjectSpawn()
    {
        //currPosition = _RBTransform.localPosition;
        nextPosition = rightPosition;
    }

    // Update is called once per frame
    void Update () {
        _RBTransform.localPosition = currPosition;
        Move();
	}

    void Move()
    {
        if(Vector3.Distance(currPosition, nextPosition) < 0.1f)
        {
            switchNextPosition();
        }
        currPosition = Vector3.MoveTowards(currPosition, nextPosition, speed * Time.deltaTime);
        _RBTransform.Rotate(Vector3.up, rotationSpeed* Time.deltaTime);
    }
    void switchNextPosition()
    {
        nextPosition = nextPosition != leftPosition ? leftPosition : rightPosition;
    }

    public void destroyDiamond()
    {
        GameObject destroyV = Instantiate(destroyVersion, transform.position, transform.rotation);
        Destroy(destroyV, 2f);
        Destroy(gameObject);
    }

}
