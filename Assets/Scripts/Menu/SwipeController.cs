using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SwipeController : MonoBehaviour {

    public enum SwipeDirection
    {
        Up,
        Down,
        Right,
        Left
    }
    private bool swiping = false;
    private Vector2 lastPosition, nextPosition, deltaPosition;
    public UIManager manager;
    public static bool isAnimating;
    private void Start()
    {
        isAnimating = false;
    }
    void Update()
    {
        
        if (Input.GetButtonDown("Fire1"))
        {
           swiping = true;
           lastPosition = Input.mousePosition;        
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
            if(deltaPosition.magnitude > 60f)
            {
                float x = deltaPosition.x;
                float y = deltaPosition.y;

                if(!manager.isCoins && !manager.isSettings && !isAnimating)
                {
                    if (!manager.isLeaderboard && Input.mousePosition.y < Screen.height / 4)
                    {
                        if (Mathf.Abs(x) < Mathf.Abs(y) && y > 0)
                        {
                            manager.ShowLeaderboard();
                        }
                    }
                    else if (manager.isLeaderboard)
                    {
                        if (Mathf.Abs(x) < Mathf.Abs(y) && y < 0)
                        {
                            manager.HideLeaderboard();
                        }
                    }
                }
                if (!manager.isLeaderboard && !manager.isSettings && !isAnimating)
                {
                    if (!manager.isCoins && Input.mousePosition.x > Screen.width / 2)
                    {
                        if (Mathf.Abs(x) > Mathf.Abs(y) && x < 0)
                        {
                            manager.ShowCoins();
                        }
                    }
                    else if (manager.isCoins)
                    {
                        if (Mathf.Abs(x) > Mathf.Abs(y) && x > 0)
                        {
                            manager.HideCoins();
                        }
                    }
                }
                
                Reset();
            }
            lastPosition = nextPosition;
        }
        
    }
    public void StopAnimating()
    {
        isAnimating = false;
    }
    private void Reset()
    {
        lastPosition = deltaPosition = Vector2.zero;
        swiping = false;
    }
}
