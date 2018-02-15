using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{

    public static float deltaTime = 0.0f;
    public static float fixedDeltaTime = 0.0f;
    public static float difficultyIncrease = 0.1f;

    [Header("Settings:")]
    public PlayerController player;
    public Animator pauseAnim;
    public Text playerScore;
    public Button resume;


    GameState GS;
    void Start () {
        GS = GameState.Instance;
        Time.timeScale = 1f;
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
        float toTimeSpeed = 0f;
        for (var t = 0f; t < 1; t += Time.unscaledDeltaTime / time)
        {
            Time.timeScale = Mathf.Lerp(1f, toTimeSpeed, t);
            yield return null;
        }
        Time.timeScale = 0f;
        resume.interactable = false;
        player.enabled = false;
        GS.SetGameOver(true);
        playerScore.text = GS.getScoreString();
        pauseAnim.SetBool("isPaused", true);
    }
    public void Quit()
    {
        Application.Quit();
    }
    public void Pause()
    {
        resume.interactable = true;
        GS.SetPause(true);
        player.enabled = false;
        Time.timeScale = 0f;
        playerScore.text = GS.getScoreString();
        pauseAnim.SetBool("isPaused", true);
    }
    public void Resume()
    {
        GS.SetPause(false);
        GS.SetGameOver(false);
        player.enabled = true;
        Time.timeScale = 1f;
        pauseAnim.SetBool("isPaused", false);
    }
    public void Restart()
    {
        SceneManager.LoadScene("Level01");
    }

}
