using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameState : MonoBehaviour {

    //Instance of object
    #region Singleton
    public static GameState Instance;
    private void Awake()
    {
        Instance = this;
    }
    #endregion

    public Text scoreText;
    public float currentSpeedFactor;

    float maxSpeedFactor;
    int countCoins;
    int score;

    bool isShield;
    float countShield;

    bool isPaused;
    bool isGameOver;
    // Use this for initialization
    void Start () {
        countCoins = 0;
        score = -1;
        //currentSpeedFactor = 1f;
        maxSpeedFactor = 3f;
        isPaused = false;
        isGameOver = false;
        isShield = false;
        countShield = 0f;
        StartCoroutine(increaseSpeedFactorEvery(5f));
    }
	
	// Update is called once per frame
	void Update () {
		
	}



    public int getScoreInt()
    {
        return int.Parse(scoreText.text);
    }
    public string getScoreString()
    {
        return scoreText.text;
    }
    public void AddScore()
    {
        score++;
        scoreText.text = score.ToString();
    }
    public void AddCoin()
    {
        countCoins++;
    }
    public void MakeShield()
    {
        isShield = true;
        countShield = 1f;
    }
    public bool IsShield()
    {
        return isShield;
    }
    public void TurnOffShield()
    {
        isShield = false;
    }
    public bool IsPaused()
    {
        return isPaused;
    }

    public void SetPause( bool pause )
    {
        isPaused = pause;
    }
    public void SetGameOver( bool gameover)
    {
        isGameOver = gameover;
    }

    public float getSpeedFactor()
    {
        return currentSpeedFactor;
    }
    IEnumerator increaseSpeedFactorEvery(float sec)
    {
        while (true)
        {
            if(currentSpeedFactor>maxSpeedFactor)
            {
                currentSpeedFactor = maxSpeedFactor;
                break;
            }
            currentSpeedFactor += GameManager.difficultyIncrease;
            yield return new WaitForSeconds(sec);
        }
    }
}
