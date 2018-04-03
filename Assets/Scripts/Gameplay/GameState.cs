using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameState : MonoBehaviour {

    public static float currentSpeedFactor;
    public static int countCoins;
    public static int score;
    public static bool isAlive;
    public static bool isPaused;
    public static bool isGameOver;
    public static int playerPositionX;
    public static int playerPositionY;
    // Use this for initialization
    void Start () {
        countCoins = 0;
        score = -1;
        currentSpeedFactor = 1f;
        isPaused = false;
        isGameOver = false;
        isAlive = true;
        playerPositionX = 0;
        playerPositionY = 0;
    }
	

    
}
