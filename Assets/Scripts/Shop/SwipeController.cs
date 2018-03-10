using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SwipeController : MonoBehaviour {

    public Image cubePlace;
    public enum SwipeDirection
    {
        Up,
        Down,
        Right,
        Left
    }
    private bool swiping = false;
    private Vector2 lastPosition, nextPosition, deltaPosition;



    private void Start()
    {
    }
    void Update()
    {
        
        if (Input.GetButtonDown("Fire1"))
        {
            if(cubePlace.Raycast(Input.mousePosition, Camera.main))
            {
                Debug.Log("true");
                swiping = true;
                lastPosition = Input.mousePosition;
            }         
        }
        if (Input.GetButtonUp("Fire1"))
        {
            swiping = false;
            Reset();
        }
        if(swiping)
        {
            nextPosition = Input.mousePosition;
            deltaPosition = nextPosition - lastPosition;
            if(deltaPosition.magnitude > 50f)
            {
                float x = deltaPosition.x;
                float y = deltaPosition.y;

                if(Mathf.Abs(x)>Mathf.Abs(y))
                {
                    //Right
                    if(x>0)
                    {
                        Debug.Log("Right!");
                    }//Left
                    else
                    {
                        Debug.Log("Left!");
                    }
                } else
                {
                    //Up
                    if (y > 0)
                    {
                        Debug.Log("Up!");
                    }//Down
                    else
                    {
                        Debug.Log("Down!");
                    }
                }
                Reset();
            }
            lastPosition = nextPosition;
        }
        
    }

    private void Reset()
    {
        lastPosition = deltaPosition = Vector2.zero;
        swiping = false;
    }
}
