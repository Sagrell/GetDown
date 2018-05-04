using UnityEngine;

public class GameState : MonoBehaviour {

    public static float currentSpeedFactor;
    public static int countCoins;
    public static int score;
    public static bool isLearning;
    public static int learningProgress;
    public static bool isAlive;
    public static bool isPaused;
    public static bool isGameOver;
    public static int playerPositionX;
    public static int playerPositionY;
    // Use this for initialization
    void Start () {
        countCoins = 0;
        learningProgress = 0;
        score = -1;
        currentSpeedFactor = 1f;
        isLearning = false;
        isPaused = false;
        isGameOver = false;
        isAlive = true;
        playerPositionX = 0;
        playerPositionY = 0;
    }
	

    
}
