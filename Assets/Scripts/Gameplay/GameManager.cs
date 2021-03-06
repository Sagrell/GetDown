﻿using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using GoogleMobileAds.Api;

public class GameManager : MonoBehaviour
{
    #region Singleton
        public static GameManager Instance;
        private void Awake()
        {
            Instance = this;
            player = Instantiate(playerObject, playerPosition.position, playerPosition.rotation, playerPosition.parent).GetComponent<PlayerController>();
            trail.objToFollow = player.transform;
        }
    #endregion  
    public static float deltaTime = 0.0f;
    public static float fixedDeltaTime = 0.0f;

    public TrailLocal trail;
    public static PlayerController player;

    public Renderer background;
    [Header("Player settings:")]
    public GameObject playerObject;
    public Transform playerPosition;
    public float playerSpeed = 3f;
    public float maxPlayerSpeed = 7f;
    [Space]
    [Header("Difficulty settings:")]
    [Range(0f, 1f)]
    public float difficultyIncrease;
    public float increaseEvery;
    public float maxSpeedFactor;
    public Animator fadeAnim;
    public Renderer platform;
    public Renderer platformWithSpike;
    public Renderer brokenPlatform;
    DataManager dataManager;

    IEnumerator respawnProgress;
    bool isMute;
    float musicVolume;
    void Start () {
        platform.sharedMaterial = SkinManager.platformMat;
        platformWithSpike.sharedMaterial.SetTexture("_MainTex", SkinManager.platformMat.GetTexture("_MainTex"));
        platformWithSpike.sharedMaterial.SetTextureOffset("_MainTex", SkinManager.platformMat.GetTextureOffset("_MainTex"));
        platformWithSpike.sharedMaterial.SetTextureScale("_MainTex", SkinManager.platformMat.GetTextureScale("_MainTex"));

        brokenPlatform.sharedMaterial.SetTexture("_MainTex", SkinManager.platformMat.GetTexture("_MainTex"));
        brokenPlatform.sharedMaterial.SetTextureOffset("_MainTex", SkinManager.platformMat.GetTextureOffset("_MainTex"));
        brokenPlatform.sharedMaterial.SetTextureScale("_MainTex", SkinManager.platformMat.GetTextureScale("_MainTex"));

        dataManager = DataManager.Instance;
        background.material = SkinManager.backgroundMat;
        Time.timeScale = 0f;
        SceneController.currentScene = "Game";
        AudioCenter.Instance.PlayMusic("MainTheme", 1f);
        UserData data = dataManager.GetUserData();
        isMute = data.isMute;
        musicVolume = data.musicVolume;
        if (!isMute)
        {
            AudioCenter.Instance.SetVolumeAllMusic(musicVolume);
        }
        else
        {
            AudioCenter.Instance.SetVolumeAllMusic(0f);
        }
        GameState.isLearning = data.isFirstTime;

        StartCoroutine(StartGame());
    }
   
   IEnumerator StartGame()
    {
        fadeAnim.Play("FadeIn");
        fadeAnim.Update(0f);  
        while (fadeAnim.GetCurrentAnimatorStateInfo(0).IsName("Main.FadeIn"))
        {
            yield return null;
        }
        GooglePlayManager.Instance.Achieve(GooglePlayManager.beginnerAchieve);
        while (GameState.isLearning) { yield return new WaitForSeconds(.2f); }
        Time.timeScale = 1f;
        StartCoroutine(IncreaseSpeedFactorEvery(increaseEvery));
    }
    void Update () {
        deltaTime = Time.deltaTime;    
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

    private void OnApplicationPause(bool pause)
    {
        if (!GameState.isGameOver && !GameState.isLearning && !GameState.isPaused)
        {
            if (pause)
            {

                AudioCenter.Instance.PauseMusic("MainTheme", .2f);
                Time.timeScale = 0f;
            }
            else
            {
                
                AudioCenter.Instance.ResumeMusic("MainTheme", .2f);
                Time.timeScale = 1f;

            }
        }
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
