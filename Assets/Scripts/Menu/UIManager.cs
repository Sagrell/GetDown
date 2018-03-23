using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoBehaviour {

    public Text coins;
    public GameObject cube;
    public Transform cubePosition;
    public Animator contentAnim;

    [Header("Materials:")]
    public Renderer shopPlatform;
    public Renderer creditsPlatform;
    public Renderer exitPlatform;
    [Space]
    [Header("Settings:")]
    public Scrollbar music;
    public Scrollbar sound;
    public GameObject disableSound;
    UserData data;
    float musicVolume;
    float soundVolume;
    bool isMute;
    bool isSettings;
    private void Start()
    {
        isSettings = false;
        data = DataManager.Instance.GetUserData();
        musicVolume = data.musicVolume;
        soundVolume = data.soundVolume;      
        isMute = data.isMute;
        disableSound.SetActive(isMute);
        
        music.value = musicVolume;
        sound.value = soundVolume;
        coins.text = data.GoldAmount.ToString();
        cube.GetComponent<Renderer>().material = SkinManager.cubeMat;
        shopPlatform.material = SkinManager.platformMat;
        creditsPlatform.material = SkinManager.platformMat;
        exitPlatform.material = SkinManager.platformMat;
        RenderSettings.skybox = SkinManager.backgroundMat;
        Instantiate(cube, cubePosition);
        if(SceneController.previousScene != "Shop")
        {
            AudioCenter.Instance.PlayMusic("MenuTheme",1f);
        }
        if (!isMute)
        {
            AudioCenter.Instance.SetVolumeAllMusic(musicVolume);
            AudioCenter.Instance.SetVolumeToSounds(soundVolume);
        }
        else
        {
            AudioCenter.Instance.SetVolumeAllMusic(0f);
            AudioCenter.Instance.SetVolumeToSounds(0f);
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
        AudioCenter.Instance.PauseMusic("MenuTheme",1f);
        StartCoroutine(StartScene("Game"));
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
    public void GoToCredits()
    {
        AudioCenter.Instance.PlaySound("Button");
        coins.text = data.GoldAmount.ToString();
    }
    public void ShowSettings()
    {
        isSettings = true;
        contentAnim.Play("SettingsFadeIn");
    }
    public void HideSettings()
    {
        if(isSettings)
        {
            contentAnim.Play("SettingsFadeOut");
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
        disableSound.SetActive(isMute);
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
        }
    }
    public void ApplyVolume()
    {
        if(!isMute)
        {
            data = DataManager.Instance.GetUserData();
            data.musicVolume = musicVolume;
            data.soundVolume = soundVolume;
            DataManager.Instance.SaveUserData(data);
        } 
        HideSettings();
    }
    public void Quit()
    {
        AudioCenter.Instance.PlaySound("Button");
        Application.Quit();
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
