﻿using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    #region Singleton
        public static GameManager Instance;
        private void Awake()
        {
            Instance = this;
            player = Instantiate(playerObject, playerPosition.position, playerPosition.rotation, playerPosition.parent).GetComponent<PlayerController>();
        }
    #endregion  
    public static float deltaTime = 0.0f;
    public static float fixedDeltaTime = 0.0f;

    public static PlayerController player;
    

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
    

    void Start () {
        platform.material = SkinManager.platformMat;
        platformWithSpike.sharedMaterial.SetTexture("_MainTex", SkinManager.platformMat.GetTexture("_MainTex"));
        brokenPlatform.sharedMaterial.SetTexture("_MainTex", SkinManager.platformMat.GetTexture("_MainTex"));
        dataManager = DataManager.Instance;
        RenderSettings.skybox = SkinManager.backgroundMat;
        Time.timeScale = 0f;
        AudioCenter.LoadMusic("MainTheme");
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
        Time.timeScale = 1f;
        AudioCenter.PlayMusic(1f);
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
