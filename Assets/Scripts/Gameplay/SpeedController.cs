using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedController : MonoBehaviour {

    [Header("Player:")]
    public static float playerSpeed = 0;


    [Space]
    [Header("Difficulty:")]
    [Range(0f, 1f)]
    public float difficultyIncrease;
    public float increaseEvery;
    public float maxSpeedFactor;


    // Use this for initialization
    void Start () {
        StartCoroutine(IncreaseSpeedFactorEvery(increaseEvery));
    }

    IEnumerator IncreaseSpeedFactorEvery(float sec)
    {
        while (true)
        {
            if (GameState.currentSpeedFactor > maxSpeedFactor)
            {
                GameState.currentSpeedFactor = maxSpeedFactor;
                break;
            }
            GameState.currentSpeedFactor += difficultyIncrease;
            yield return new WaitForSeconds(sec);
        }
    }
}
