using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using GoogleMobileAds.Api;

public class InGameGUI : MonoBehaviour {
    public static InGameGUI Instance;
    private void Awake()
    {
        player = FindObjectOfType<PlayerController>();
        Instance = this;
    }
    public Text scoreInPause;
    public Text scoreInGameOver;
    public Text bestScoreInPause;
    public Text bestScoreInGameOver;
    public Text scoreInGame;
    public Text coinsInGame;
    public RectTransform powerUpContainer;
    public GameObject shieldUpgrade;
    public GameObject magnetUpgrade;
    public GameObject doubleCoinsUpgrade;
    public GameObject fastRunUpgrade;
    public GameObject gameOverPanel;
    public GameObject pausePanel;
    public PlatformSpawner spawner;
    public Animator anim;
    public Animator respawnAnim;
    public Animator buyCoinsAnim;
    public Image progressRespawn;
    public Button respawn;
    public Button buyRespawn;
    public Button freeRespawn;
    public Button watchAdRespawn;
    PlayerController player;
    bool isRespawnWindow;
    
    int bestScore;
    int respawnCost;
    DataManager dataManager;
    UserData data;
    Dictionary<string, Image> powerUps;

    bool isStartRespawn;
    bool isStartRespawnTimer;
    bool isRewarded;
    bool isStopTime;
    bool isDisabledAd;
    void Start () {
        dataManager = DataManager.Instance;
        data = dataManager.GetUserData();
        gameOverPanel.SetActive(true);
        pausePanel.SetActive(true);
        bestScore = data.HighScore;
        powerUps = new Dictionary<string,Image>();
        isRespawnWindow = false;
        isStartRespawn = false;
        isRewarded = false;
        isStartRespawnTimer = false;
        isDisabledAd = PurchaseManager.isDisabledAd;
        freeRespawn.gameObject.SetActive(isDisabledAd);
        watchAdRespawn.gameObject.SetActive(!isDisabledAd);
        respawnCost = 50;
    }
    private void Update()
    {
        if (isStartRespawn)
        {
            isStartRespawn = false;
            Respawn();
        }
        if (isStartRespawnTimer)
        {
            isStartRespawnTimer = false;
            Time.timeScale = 0f;
            StopAllCoroutines();
            StartCoroutine(RespawnProgress(5f, progressRespawn));
        }
        if (isStopTime)
        {
            isStopTime = false;
            Time.timeScale = 0f;
        }
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
        if (++AdManager.countRestart >= AdManager.nextShowAd)
        {
            AdManager.nextShowAd = Random.Range(3, 6);
            AdManager.Instance.ShowGameOverAd(HandleOnGameOverAdClosed);
            AdManager.countRestart = 0;
            
        } else
        {
            StopAllCoroutines();
            StartCoroutine(RespawnProgress(5f, progressRespawn));
        }
    }
    void HandleOnGameOverAdClosed(object sender, System.EventArgs args)
    {
        AdManager.Instance.LoadGameOverAd();
        isStartRespawnTimer = true;
    }
    public IEnumerator ResumeAnimation(float time)
    {
        yield return new WaitForSecondsRealtime(0.5f);
        Time.timeScale = 1f;
    }
    public IEnumerator RespawnAnimation()
    {
        yield return new WaitForSecondsRealtime(0.8f);
        GameState.playerPositionY += (Mathf.RoundToInt(LevelController.levelPosition.y/1.5f)) - GameState.playerPositionY;
        spawner.Restart();
        player.RespawnPlayer();
        Animator respawn = player.GetComponent<Animator>();
        respawn.enabled = true;
        respawn.Play("Respawn");
        respawn.Update(0f);
        while (respawn.GetCurrentAnimatorStateInfo(0).IsName("Main.Respawn"))
        {
            yield return null;
        }
        respawn.enabled = false;
        player.enabled = true;
        player.GetComponent<Rigidbody>().velocity = Vector3.zero;
        Time.timeScale = 1f;
    }
    public void Resume()
    {
        AudioCenter.Instance.ResumeMusic("MainTheme", 0.5f);
        GameState.isPaused = false;
        player.enabled = true;
        StopAllCoroutines();
        StartCoroutine(ResumeAnimation(2f));
        anim.SetBool("isPaused", false);
    }
    public void Respawn()
    {
        AudioCenter.Instance.ResumeMusic("MainTheme", 1.5f);
        GameState.isPaused = false;
        GameState.isGameOver = false;
        GameState.isAlive = true;  
        anim.SetBool("isGameOver", false);
        respawnAnim.Play("HideRespawnType");
        StartCoroutine(RespawnAnimation());
    }
    public void FreeRespawn()
    {
        Respawn();
        freeRespawn.interactable = false;
    }
    public void Restart()
    {
        SceneManager.LoadScene("Game");
    }
    public void ShowRespawnType()
    {
        isRespawnWindow = true;
        buyRespawn.GetComponentInChildren<Text>().text = respawnCost.ToString();
        respawnAnim.Play("ShowRespawnType");
    }
    public void HideRespawnType()
    {
        if (isRespawnWindow)
        {
            isRespawnWindow = false;
            respawnAnim.Play("HideRespawnType");
        }
      
    }
    public void ShowAdRespawn()
    {
        AdManager.Instance.ShowRewardAd(HandleOnAdRewarded, HandleOnRewardAdClosed);
    }
    public void BuyRespawn()
    {
        if (data.GoldAmount < respawnCost)
        {
            buyCoinsAnim.Play("ShowBuyCoins");
        } else
        {
            data.GoldAmount -= respawnCost;
            DataManager.Instance.SaveUserData(data);
            Respawn();
            respawnCost *= 2;
        }
    }
    public void HideBuyCoins()
    {
        buyCoinsAnim.Play("HideBuyCoins");
    }
    
    public void StopPowerUp( string powerUp )
    {
        if(powerUps.ContainsKey(powerUp)) {
            Destroy(powerUps[powerUp].gameObject);
            powerUps.Remove(powerUp);
        }
        
    }
   
    void HandleOnAdRewarded(object sender, Reward reward)
    {
        isRewarded = true;
    }

    void HandleOnRewardAdClosed(object sender, System.EventArgs args)
    {
        AdManager.Instance.LoadRewardAd();
        if(isRewarded)
        {
            watchAdRespawn.interactable = false;
            isStartRespawn = true;
        } else
        {
            isStopTime = true;
        }
    }
    public void StartPowerUp(string powerUp, float time)
    {
        Image powerUpImage = null;
        switch (powerUp)
        {
            case "Shield":
                powerUpImage = Instantiate(shieldUpgrade, powerUpContainer).GetComponent<Image>();
                break;
            case "Shield":
                powerUpImage = Instantiate(shieldUpgrade, powerUpContainer).GetComponent<Image>();
                break;
            case "Shield":
                powerUpImage = Instantiate(shieldUpgrade, powerUpContainer).GetComponent<Image>();
                break;
            case "Shield":
                powerUpImage = Instantiate(shieldUpgrade, powerUpContainer).GetComponent<Image>();
                break;
        }
        
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
    IEnumerator RespawnProgress(float time, Image progressImage)
    {
        respawn.interactable = true;
        progressImage.transform.parent.gameObject.SetActive(true);
        float currentProgress = 1f;
        while (currentProgress > 0)
        {
            currentProgress -= Time.unscaledDeltaTime / time;
            progressImage.fillAmount = currentProgress;
            yield return null;
        }
        respawn.interactable = false;
        progressImage.transform.parent.gameObject.SetActive(false);
    }
}
