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
    public Button pause;
    public Button buyRespawn;
    public Button freeRespawn;
    public Button watchAdRespawn;

    public Animator learningAnim;
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
    bool isDisabledAd;
    bool isRespawnPaused;

    IEnumerator respawnProgress;
    void Start () {
        isRespawnPaused = false;
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
        respawnProgress = RespawnProgress(0, null);
        if(GameState.isLearning)
        {
            StartCoroutine(StartLearning());
        }
        StartCoroutine("CheckState");
    }

    bool waitForTap;
    bool isAnimating;
    bool isDead = false;
    IEnumerator StartLearning()
    {
        pause.interactable = false;
        Time.timeScale = 0f;
        AudioCenter.Instance.PauseMusic("MainTheme", 0.5f);
        if(!isDead)
        learningAnim.Play("ShowFirst");
        isAnimating = true;
        player.enabled = false; pause.interactable = false;
        yield return new WaitForSecondsRealtime(1f);
        waitForTap = true;
        while (waitForTap) { yield return null; }
        if (!isDead)
            learningAnim.Play("HideFirst");
        yield return new WaitForSecondsRealtime(1f);
        pause.interactable = true;
        player.enabled = true;
        isAnimating = false;
        AudioCenter.Instance.ResumeMusic("MainTheme", 0.5f);
        Time.timeScale = 1f;

        while (LevelController.levelPosition.y < 11) { yield return null; }
        Time.timeScale = 0f;
        AudioCenter.Instance.PauseMusic("MainTheme", 0.5f);
        player.enabled = false; pause.interactable = false;
        if (!isDead)
            learningAnim.Play("ShowSecond");
        isAnimating = true;
        yield return new WaitForSecondsRealtime(1f);
        waitForTap = true;
        while (waitForTap) { yield return null; }
        if (!isDead)
            learningAnim.Play("HideSecond");
        yield return new WaitForSecondsRealtime(1f);
        isAnimating = false;
        pause.interactable = true;
        player.enabled = true;
        AudioCenter.Instance.ResumeMusic("MainTheme", 0.5f);
        Time.timeScale = 1f;

        while (LevelController.levelPosition.y < 22) { yield return null; }
        Time.timeScale = 0f;
        AudioCenter.Instance.PauseMusic("MainTheme", 0.5f);
        player.enabled = false; pause.interactable = false;
        if (!isDead)
            learningAnim.Play("ShowThird");
        isAnimating = true;
        yield return new WaitForSecondsRealtime(1f);
        waitForTap = true;
        while (waitForTap) { yield return null; }
        if (!isDead)
            learningAnim.Play("HideThird");
        yield return new WaitForSecondsRealtime(1f);
        isAnimating = false;
        pause.interactable = true;
        player.enabled = true;
        AudioCenter.Instance.ResumeMusic("MainTheme", 0.5f);
        Time.timeScale = 1f;

        while (LevelController.levelPosition.y < 36) { yield return null; }
        Time.timeScale = 0f;
        AudioCenter.Instance.PauseMusic("MainTheme", 0.5f);
        player.enabled = false; pause.interactable = false;
        if (!isDead)
            learningAnim.Play("ShowQuad");
        isAnimating = true;
        yield return new WaitForSecondsRealtime(1f);
        waitForTap = true;
        while (waitForTap) { yield return null; }
        if (!isDead)
            learningAnim.Play("HideQuad");
        yield return new WaitForSecondsRealtime(1f);
        isAnimating = false;
        pause.interactable = true;
        player.enabled = true;
        AudioCenter.Instance.ResumeMusic("MainTheme", 0.5f);
        Time.timeScale = 1f;

        while (LevelController.levelPosition.y < 50) { yield return null; }
        Time.timeScale = 0f;
        AudioCenter.Instance.PauseMusic("MainTheme", 0.5f);
        player.enabled = false;pause.interactable = false;
        if (!isDead)
            learningAnim.Play("ShowFifth");
        isAnimating = true;
        yield return new WaitForSecondsRealtime(1f);
        waitForTap = true;
        while (waitForTap) { yield return null; }
        if (!isDead)
            learningAnim.Play("HideFifth");
        yield return new WaitForSecondsRealtime(1f);
        isAnimating = false;
        pause.interactable = true;
        player.enabled = true;
        AudioCenter.Instance.ResumeMusic("MainTheme", 0.5f);
        Time.timeScale = 1f;

        while (LevelController.levelPosition.y < 65) { yield return null; }
        pause.interactable = false;
        Time.timeScale = 0f;
        AudioCenter.Instance.PauseMusic("MainTheme", 0.5f);
        player.enabled = false;
        if (!isDead)
            learningAnim.Play("ShowSixth");
        isAnimating = true;
        yield return new WaitForSecondsRealtime(1f);
        waitForTap = true;
        while (waitForTap) { yield return null; }
        if (!isDead)
            learningAnim.Play("HideSixth");
        yield return new WaitForSecondsRealtime(1f);
        isAnimating = false;
        pause.interactable = true;
        player.enabled = true;
        AudioCenter.Instance.ResumeMusic("MainTheme", 0.5f);
        Time.timeScale = 1f;
        pause.interactable = false;
        while (LevelController.levelPosition.y < 70) { yield return null; }
        if (!isDead)
            learningAnim.Play("YouAreReady");
        yield return new WaitForSecondsRealtime(1f);
        if (!isDead)
            learningAnim.Play("YouAreReadyHide");

        GameState.isLearning = false;
        GameState.score += GameState.learningProgress;
        data.isFirstTime = false;
        dataManager.SaveUserData(data);
        pause.interactable = true;

    }
    public void DieInLearning(bool isDeadAfterJump)
    {
        StartCoroutine(DieInLearningIE(isDeadAfterJump));
    }
    IEnumerator DieInLearningIE(bool isDeadAfterJump)
    {
        isDead = true;
        learningAnim.Play("Idle");
        if (isDeadAfterJump)
        {
            learningAnim.Play("MarioShow");
            float toTimeSpeed = 0f;
            for (var t = 0f; t < 1; t += Time.unscaledDeltaTime / 2f)
            {
                Time.timeScale = Mathf.Lerp(1f, toTimeSpeed, t);
                yield return null;
            }
            learningAnim.Play("MarioHide");
            yield return new WaitForSecondsRealtime(.5f);
        }
        else
        {
            learningAnim.Play("DeadShow");
            float toTimeSpeed = 0f;
            for (var t = 0f; t < 1; t += Time.unscaledDeltaTime / 1.5f)
            {
                Time.timeScale = Mathf.Lerp(1f, toTimeSpeed, t);
                yield return null;
            }
            learningAnim.Play("DeadHide");
            yield return new WaitForSecondsRealtime(.5f);
        }
        SceneManager.LoadScene("Game");
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if(!isShowingAd)
            {
                if (!GameState.isGameOver && !GameState.isPaused)
                {
                    Pause();
                }
                else if (GameState.isPaused)
                {
                    Resume();
                }
            }   
        }
        if (GameState.isLearning)
        {
            Touch[] touches = Input.touches;
            if (Input.GetKey("d") || Input.GetKey("a") || (touches.Length == 1))
            {
                waitForTap = false;
            }
        } 
        
    }
    IEnumerator CheckState()
    {
        while(true)
        {
            if (isStartRespawn)
            {
                watchAdRespawn.interactable = false;
                isStartRespawn = false;
                Respawn();
            }
            if (isStartRespawnTimer)
            {
                isStartRespawnTimer = false;
                Time.timeScale = 0f;
                Debug.Log("AD SEND");
                StopCoroutine(respawnProgress);
                respawnProgress = RespawnProgress(5f, progressRespawn);
                StartCoroutine(respawnProgress);
            }
            yield return new WaitForSecondsRealtime(.2f);
        }
    }
    public void UpdateScore()
    {

        string score = GameState.isLearning ? GameState.learningProgress.ToString() : GameState.score.ToString();
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
        if (!isAnimating)
        {
            AudioCenter.Instance.PauseMusic("MainTheme", 0.5f);
            pausePanel.SetActive(true);
            Time.timeScale = 0f;
            scoreInPause.text = GameState.isLearning ? GameState.learningProgress.ToString() : GameState.score.ToString();
            bestScoreInPause.text = bestScore.ToString();
            GameState.isPaused = true;
            player.enabled = false;
            anim.SetBool("isPaused", true);
        }    
    }
    bool isShowingAd = false;
    public void GameOver()
    {
        isStartRespawnTimer = false;
        isRespawnPaused = false;
        AudioCenter.Instance.PauseMusic("MainTheme", 0.5f);
        gameOverPanel.SetActive(true);
        Time.timeScale = 0f;
        scoreInGameOver.text = GameState.isLearning ? GameState.learningProgress.ToString() : GameState.score.ToString();
        bestScoreInGameOver.text = bestScore.ToString();
        player.enabled = false;
        GameState.isGameOver = true;
        anim.SetBool("isGameOver", true);
        if (++AdManager.countRestart >= AdManager.nextShowAd)
        {
            isShowingAd = true;
            AdManager.nextShowAd = Random.Range(2, 4);
            Debug.Log("AD 1");
            AdManager.Instance.ShowGameOverAd(HandleOnGameOverAdClosed);
            AdManager.countRestart = 0;
            
        } else
        {
            StopCoroutine(respawnProgress);
            respawnProgress = RespawnProgress(5f, progressRespawn);
            StartCoroutine(respawnProgress);
        }
    }
    void HandleOnGameOverAdClosed(object sender, System.EventArgs args)
    {
        isShowingAd = false;
        AdManager.Instance.LoadGameOverAd();
        isStartRespawnTimer = true;
        Debug.Log("AD OVER");
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
        player.gameObject.SetActive(true);
        player.enabled = true;
        player.GetComponent<Rigidbody>().velocity = Vector3.zero;
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
        
        
        Time.timeScale = 1f;
    }
    public void Resume()
    {
        AudioCenter.Instance.ResumeMusic("MainTheme", 0.5f);
        GameState.isPaused = false;
        player.enabled = true;
        StopCoroutine("ResumeAnimation");
        StartCoroutine("ResumeAnimation",2f);
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
        StartCoroutine("RespawnAnimation");
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
        isRespawnPaused = true;
    }
    public void HideRespawnType()
    {
        if (isRespawnWindow)
        {
            isRespawnPaused = false;
            isRespawnWindow = false;
            respawnAnim.Play("HideRespawnType");
        }
      
    }
    public void ShowAdRespawn()
    {
        isRespawnPaused = true;
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
            powerUps[powerUp].gameObject.SetActive(false);
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
        isRespawnPaused = false;
        if (isRewarded)
        {      
            isStartRespawn = true;
        }
    }
    IEnumerator startShield;
    IEnumerator startMagnet;
    IEnumerator startDoubleCoins;
    IEnumerator startFastRun;
    public void StartPowerUp(string powerUp, float time)
    {
        Image powerUpImage = null;
        switch (powerUp)
        {
            case "Shield":
                shieldUpgrade.SetActive(true);
                powerUpImage = shieldUpgrade.GetComponent<Image>();
                if(startShield!=null)
                    StopCoroutine(startShield);
                startShield = PowerUpProgress(powerUp, time, powerUpImage);
                StartCoroutine(startShield);
                break;
            case "Magnet":
                magnetUpgrade.SetActive(true);
                powerUpImage = magnetUpgrade.GetComponent<Image>();
                if (startMagnet != null)
                    StopCoroutine(startMagnet);
                startMagnet = PowerUpProgress(powerUp, time, powerUpImage);   
                StartCoroutine(startMagnet);
                break;
            case "FastRun":
                fastRunUpgrade.SetActive(true);
                powerUpImage = fastRunUpgrade.GetComponent<Image>();
                if (startFastRun != null)
                    StopCoroutine(startFastRun);
                startFastRun = PowerUpProgress(powerUp, time, powerUpImage);               
                StartCoroutine(startFastRun);
                break;
            case "DoubleCoins":
                doubleCoinsUpgrade.SetActive(true);
                powerUpImage = doubleCoinsUpgrade.GetComponent<Image>();
                if (startDoubleCoins != null)
                    StopCoroutine(startDoubleCoins);
                startDoubleCoins = PowerUpProgress(powerUp, time, powerUpImage);           
                StartCoroutine(startDoubleCoins);
                break;
        }
        
        powerUps[powerUp] = powerUpImage;
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
            powerUpImage.gameObject.SetActive(false);
            powerUps.Remove(powerUp);
        }
        PowerUpManager.Instance.Deactivate(powerUp);
    }
    IEnumerator RespawnProgress(float time, Image progressImage)
    {
        Debug.Log("AD START PROGRESS");
        respawn.interactable = true;
        progressImage.transform.parent.gameObject.SetActive(true);
        float currentProgress = 1f;
        while (currentProgress > 0)
        { 
            currentProgress -= Time.unscaledDeltaTime / time;
            progressImage.fillAmount = currentProgress;
            yield return null;
            while (isRespawnPaused || isShowingAd) { yield return null; }
        }
        respawn.interactable = false;
        progressImage.transform.parent.gameObject.SetActive(false);
    }
}
