using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoBehaviour {
    static public UIManager Instance;
    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
    }


    public Text coins;
    public GameObject cube;
    public Transform cubePosition;
    public Animator contentAnim;

    [Header("Materials:")]
    public Renderer shopPlatform;
    public Renderer creditsPlatform;
    public Renderer exitPlatform;
    public Renderer background;
    [Space]
    [Header("Settings:")]
    public Scrollbar music;
    public Scrollbar sound;
    public Image disableSound;
    public Image enableSound;
    public GameObject disabledAd;
    public GameObject disableAd;
    public Animator disableAdAnim;
    UserData data;
    float musicVolume;
    float soundVolume;
    bool isMute = true;
    [HideInInspector]
    public bool isSettings;
    [HideInInspector]
    public bool isCoins;
    [HideInInspector]
    public bool isCredits;
    [HideInInspector]
    public bool isLeaderboard;

    private void Start()
    {
        PurchaseManager.Instance.disableAd = disableAdAnim;
        isSettings = false;
        isCredits = false;
        isLeaderboard = false;
        data = DataManager.Instance.GetUserData();
        musicVolume = data.musicVolume;
        soundVolume = data.soundVolume;      

        isMute = data.isMute;
        disableSound.enabled = isMute;
        enableSound.enabled = !isMute;
        music.value = musicVolume;
        sound.value = soundVolume;
        coins.text = data.GoldAmount.ToString();
        cube.GetComponent<MeshFilter>().mesh = SkinManager.cubeMesh;
        cube.GetComponent<Renderer>().material = SkinManager.cubeMat;
        shopPlatform.material = SkinManager.platformMat;
        creditsPlatform.material = SkinManager.platformMat;
        exitPlatform.material = SkinManager.platformMat;

        background.material = SkinManager.backgroundMat;
        Instantiate(cube, cubePosition);
        if(isMute && SceneController.previousScene != "Shop")
        {
            AudioCenter.Instance.StartMute("MenuTheme");
        }
        else if(SceneController.previousScene != "Shop")
        {
            AudioCenter.Instance.PlayMusic("MenuTheme",1f);
        }
        if (!isMute)
        {
            AudioCenter.Instance.SetVolumeToSounds(soundVolume);
        }
        else
        {
            AudioCenter.Instance.SetVolumeToSounds(0f);
        }

    }
    public void SetRemainingTime()
    {

    }
 

    public void BuyConsumable(int id)
    {

        PurchaseManager.Instance.BuyConsumable(id);
    }
    public void BuyNonConsumable(int id)
    {

        PurchaseManager.Instance.BuyNonConsumable(id);
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();    
        }
    }
    public void GoToShop()
    {
        AudioCenter.Instance.PlaySound("Button");
        StartCoroutine(StartScene("Shop"));
    }

    public void Play()
    {
        AudioCenter.Instance.PlaySound("Button");
        if(isMute)
        {
            AudioCenter.Instance.PauseMusic("MenuTheme", 0);
        } else
        {
            AudioCenter.Instance.PauseMusic("MenuTheme", 1f);
        }


        //StartCoroutine(StartScene("Game"));
        SceneManager.LoadScene("Game");
    }
    public void CoinsClick()
    {
        AudioCenter.Instance.PlaySound("CoinCollect");
        DataManager.Instance.SaveFullData();
        data = DataManager.Instance.GetUserData();
        coins.text = data.GoldAmount.ToString();
    }
    public void DiamondClick()
    {
        AudioCenter.Instance.PlaySound("DiamondDestroy");
        DataManager.Instance.SaveDefaultData();
        data = DataManager.Instance.GetUserData();
        coins.text = data.GoldAmount.ToString();
    }
    public void ShowSettings()
    {
        AudioCenter.Instance.PlaySound("Button");
        SwipeController.isAnimating = true;
        isSettings = true;
        contentAnim.Play("SettingsFadeIn");
        bool isDisabled = PurchaseManager.isDisabledAd;
        disableAd.SetActive(!isDisabled);
        disabledAd.SetActive(isDisabled);
        disabledAd.transform.parent.GetComponent<Animator>().enabled = !isDisabled;
    }
    public void HideSettings()
    {
        if(isSettings)
        {
            SwipeController.isAnimating = true;
            contentAnim.Play("SettingsFadeOut");
            isSettings = false;
        }            
    }
    public void ShowLeaderboard()
    {
        AudioCenter.Instance.PlaySound("Button");
        GooglePlayManager.Instance.ShowLeaderboard();
    }
    public void ShowAchievements()
    {
        AudioCenter.Instance.PlaySound("Button");
        GooglePlayManager.Instance.ShowAchievements();
    }
    public void ShowCoins()
    {
        AudioCenter.Instance.PlaySound("Button");
        isCoins = true;
        SwipeController.isAnimating = true;
        contentAnim.Play("CoinsFadeIn");
    }
    public void HideCoins()
    {
        if (isCoins)
        {
            SwipeController.isAnimating = true;
            contentAnim.Play("CoinsFadeOut");
            isCoins = false;
        }
    }
    public void ChangeLanguage(string lang)
    {
        LanguageManager.Instance.ChangeLanguage(lang);
    }
    public void ShowCredits()
    {
        AudioCenter.Instance.PlaySound("Button");
        isCredits = true;
        SwipeController.isAnimating = true;
        contentAnim.Play("CreditsFadeIn");
    }
    public void HideCredits()
    {
        if (isCredits)
        {
            SwipeController.isAnimating = true;
            contentAnim.Play("CreditsFadeOut");
            isCredits = false;
        }
    }
    public void Mute()
    {
        
        if(isMute)
        {
            AudioCenter.Instance.SetVolumeAllMusic(musicVolume);
            AudioCenter.Instance.SetVolumeToSounds(soundVolume);
        } else
        {
            AudioCenter.Instance.SetVolumeAllMusic(0f);
            AudioCenter.Instance.SetVolumeToSounds(0f);
        }
        isMute = !isMute;
        data.isMute = isMute;
        DataManager.Instance.SaveUserData(data);
        enableSound.enabled = !isMute;
        disableSound.enabled = isMute;
    }
    public void ChangeMusicVolume()
    {
        musicVolume = music.value;
        if (!isMute)
        { 
            AudioCenter.Instance.SetVolumeAllMusic(musicVolume);
        }  
    }
    public void ChangeSoundVolume()
    {
        soundVolume = sound.value;
        if (!isMute)
        {
            AudioCenter.Instance.SetVolumeToSounds(soundVolume);
            AudioCenter.Instance.PlaySound("CoinCollect");
        }
    }
    public void ApplySettings()
    {
        if(isSettings)
        {
            if (!isMute)
            {
                data = DataManager.Instance.GetUserData();
                data.musicVolume = musicVolume;
                data.soundVolume = soundVolume;
                DataManager.Instance.SaveUserData(data);

            }
            HideSettings();
        }      
    }
    public void Quit()
    {
        AudioCenter.Instance.PlaySound("Button");
        Application.Quit();
    }
    private void OnApplicationPause(bool pause)
    {
        
        if (pause)
        {
            Time.timeScale = 0f;
            if (!isMute)
                AudioCenter.Instance.PauseMusic("MenuTheme", 0f);
           
        }
        else
        {
            Time.timeScale = 1f;
            if(!isMute)
                AudioCenter.Instance.ResumeMusic("MenuTheme", 0f);
        }
    }
    
    IEnumerator StartScene( string scene )
    {
        contentAnim.Play("FadeOut");
        AsyncOperation loading = SceneManager.LoadSceneAsync(scene);
        contentAnim.Update(0f);
        loading.allowSceneActivation = false;
        while (contentAnim.GetCurrentAnimatorStateInfo(0).IsName("Main.FadeOut"))
        {
            yield return null;
        }
        loading.allowSceneActivation = true;
        SceneController.previousScene = "Menu";
    }

}
