using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Text;
using System.Collections;
using System.Collections.Generic;

public class InGameGUI : MonoBehaviour {
    public static InGameGUI Instance;
    private void Awake()
    {
        Instance = this;
    }
    public Text scoreInPause;
    public Text scoreInGameOver;
    public Text bestScoreInPause;
    public Text bestScoreInGameOver;
    public Text scoreInGame;
    public Text coinsInGame;
    public RectTransform powerUpContainer;
    public GameObject prefab;
    public GameObject gameOverPanel;
    public GameObject pausePanel;
    public Animator anim;
    PlayerController player;

    
    int bestScore;

    Dictionary<string, Image> powerUps;
    void Start () {
        gameOverPanel.SetActive(true);
        pausePanel.SetActive(true);
        player = FindObjectOfType<PlayerController>();
        bestScore = DataManager.Instance.GetUserData().HighScore;
        powerUps = new Dictionary<string,Image>();
    }

    IEnumerator StartScene(string scene)
    {
        anim.Play("FadeOut");
        AsyncOperation loading = SceneManager.LoadSceneAsync(scene);
        loading.allowSceneActivation = false;
        anim.Update(0f);
        while (anim.GetCurrentAnimatorStateInfo(0).IsName("Main.FadeOut"))
        {
            yield return null;
        }
        AudioCenter.Instance.StopMusic("MainTheme");
        loading.allowSceneActivation = true;
        SceneController.previousScene = "Game";
    }
    public void UpdateScore()
    {
        string score = GameState.score.ToString();
        scoreInGame.text = score;
    }
    public void UpdateCoins()
    {
        string coins = GameState.countCoins.ToString();
        coinsInGame.text = coins;
    }
    public void BackToMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("Menu");
    }
    public void Pause()
    {
        AudioCenter.Instance.PauseMusic("MainTheme", 0.5f);
        pausePanel.SetActive(true);
        Time.timeScale = 0f;
        scoreInPause.text = GameState.score.ToString();
        bestScoreInPause.text = bestScore.ToString();
        GameState.isPaused = true;
        player.enabled = false;
        anim.SetBool("isPaused", true);
    }
    public void GameOver()
    {
        AudioCenter.Instance.PauseMusic("MainTheme", 0.5f);
        gameOverPanel.SetActive(true);
        Time.timeScale = 0f;
        scoreInGameOver.text = GameState.score.ToString();
        bestScoreInGameOver.text = bestScore.ToString();
        player.enabled = false;
        GameState.isGameOver = true;
        anim.SetBool("isGameOver", true);
    }

    public void Resume()
    {
        AudioCenter.Instance.ResumeMusic("MainTheme", 0.5f);
        GameState.isPaused = false;
        player.enabled = true;
        Time.timeScale = 1f;
        anim.SetBool("isPaused", false);
    }
    public void Restart()
    {
        SceneManager.LoadScene("Game");
    }

    public void StopPowerUp( string powerUp )
    {
        if(powerUps.ContainsKey(powerUp)) {
            Destroy(powerUps[powerUp].gameObject);
            powerUps.Remove(powerUp);
        }
        
    }
    public void StartPowerUp(string powerUp, float time)
    {
        Image powerUpImage = Instantiate(prefab, powerUpContainer).GetComponent<Image>();
        powerUps[powerUp] = powerUpImage;
        StartCoroutine(PowerUpProgress(powerUp, time, powerUpImage));
    }
    IEnumerator PowerUpProgress(string powerUp, float time, Image powerUpImage)
    {
        float currentProgress = 1f;
        while (currentProgress>0 && powerUpImage)
        {
           currentProgress -= GameManager.deltaTime / time;
           powerUpImage.fillAmount = currentProgress; 
           yield return null;
        }
        if(powerUpImage)
        {
            Destroy(powerUpImage.gameObject);
            powerUps.Remove(powerUp);
        }
        PowerUpManager.Instance.Deactivate(powerUp);
    }
}
