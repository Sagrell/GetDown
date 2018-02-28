using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public static float deltaTime = 0.0f;
    public static float fixedDeltaTime = 0.0f;


    [Space]
    [Header("Difficulty:")]
    [Range(0f,1f)]
    public float difficultyIncrease;
    [Tooltip("How often difficulty should increase in seconds")]
    public float increaseEvery;
    public float maxSpeedFactor;

    DataManager dataManager;
    void Start () {
        Instance = this;
        Time.timeScale = 1f;
        StartCoroutine(increaseSpeedFactorEvery(5f));
        dataManager = DataManager.Instance;
        AudioManager.Instance.Play("MainTheme");
    }
	

	void Update () {

        deltaTime = Time.deltaTime;
        //Time.timeScale = 0.2f;
       
    }
    private void FixedUpdate()
    {
        fixedDeltaTime = Time.fixedDeltaTime;
    }

    public void Kill()
    {
        StartCoroutine(KillAnimation(1.5f));
    }
    public IEnumerator KillAnimation(float time)
    {
        float toTimeSpeed = 0.5f;
        for (var t = 0f; t < 1; t += Time.unscaledDeltaTime / time)
        {
            Time.timeScale = Mathf.Lerp(1f, toTimeSpeed, t);
            yield return null;
        }
        dataManager.SaveWithGameState();
        InGameGUI.Instance.GameOver();
    }
  

    IEnumerator increaseSpeedFactorEvery(float sec)
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
