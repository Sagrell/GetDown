using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {

    public static float deltaTime = 0.0f;
    public static float fixedDeltaTime = 0.0f;
    public float gravity = 50f;
    public PlayerController player;
    public Animator pauseAnim;
    public Text playerScore;
    public Button resume;
    bool isPaused;

    void Awake () {
        Time.timeScale = 1f;
        Physics.gravity = new Vector3(0, -gravity);
        isPaused = false;
    }
	

	void Update () {

        deltaTime = Time.deltaTime;
        Time.timeScale = 0.2f;
        if(!player.isAlive())
        {
            StartCoroutine(Dead());
        }
       
    }
    private void FixedUpdate()
    {
        fixedDeltaTime = Time.fixedDeltaTime;
        //Debug.Log(fixedDeltaTime + ":" + deltaTime);
    }
    public void slowMotion()
    {
        StartCoroutine(slowTimeForSeconds(1.5f));
    }
    IEnumerator slowTimeForSeconds(float time)
    {
        float toTimeSpeed = 0.5f;
        for (var t = 0f; t < 1; t += Time.deltaTime / time)
        {
            Time.timeScale = Mathf.Lerp(1f, toTimeSpeed, t);
            yield return null;
        }
        yield return new WaitForSecondsRealtime(1f);
        Time.timeScale = 1f;
    }
    public void Quit()
    {
        Application.Quit();
    }
    public void Pause()
    {
        resume.interactable = true;
        isPaused = true;
        player.enabled = false;
        Time.timeScale = 0f;
        playerScore.text = player.score.text;
        pauseAnim.SetBool("isPaused", isPaused);
    }
    public void Resume()
    {
        isPaused = false;
        player.enabled = true;
        Time.timeScale = 1f;
        pauseAnim.SetBool("isPaused", isPaused);
    }
    public void Restart()
    {
        SceneManager.LoadScene("Level01");
    }

    public IEnumerator Dead()
    {
        yield return new WaitForSecondsRealtime(1.5f);
        resume.interactable = false;
        isPaused = true;
        player.enabled = false;
        Time.timeScale = 0f;
        playerScore.text = player.score.text;
        pauseAnim.SetBool("isPaused", isPaused);
    }
}
