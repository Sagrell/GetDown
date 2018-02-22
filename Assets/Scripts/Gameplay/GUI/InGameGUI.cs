using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Text;
public class InGameGUI : MonoBehaviour {

    public Text scoreInPause;
    public Text scoreInGameOver;
    public Text bestScoreInPause;
    public Text bestScoreInGameOver;
    public Text scoreInGame;
    public GameObject gameOverPanel;
    public GameObject pausePanel;
    PlayerController player;

    Animator pauseAnimation;
    int bestScore;
    void Start () {
        gameOverPanel.SetActive(true);
        pausePanel.SetActive(true);
        player = FindObjectOfType<PlayerController>();
        pauseAnimation = GetComponent<Animator>();
        bestScore = DataManager.Instance.getUserData().HighScore;
    }
	
    public void UpdateScore()
    {
        string score = GameState.score.ToString();
        scoreInGame.text = score;
    }

    public void Quit()
    {
        Application.Quit();
    }
    public void Pause()
    {
        pausePanel.SetActive(true);
        Time.timeScale = 0f;
        scoreInPause.text = GameState.score.ToString();
        bestScoreInPause.text = bestScore.ToString();
        GameState.isPaused = true;
        player.enabled = false;
        pauseAnimation.SetBool("isPaused", true);
    }
    public void GameOver()
    {
        gameOverPanel.SetActive(true);
        Time.timeScale = 0f;
        scoreInGameOver.text = GameState.score.ToString();
        bestScoreInGameOver.text = bestScore.ToString();
        player.enabled = false;
        GameState.isGameOver = true;
        pauseAnimation.SetBool("isGameOver", true);
    }

    public void Resume()
    {
        GameState.isPaused = false;
        player.enabled = true;
        Time.timeScale = 1f;
        pauseAnimation.SetBool("isPaused", false);
    }
    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
