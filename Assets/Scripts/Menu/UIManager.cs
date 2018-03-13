using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoBehaviour {
    public Text coins;
    public GameObject cube;
    public Renderer shopPlatform;
    public Renderer creditsPlatform;
    public Renderer exitPlatform;
    UserData data;
    private void Start()
    {
        data = DataManager.Instance.GetUserData();
        coins.text = data.GoldAmount.ToString();
        cube.GetComponent<Renderer>().material = SkinManager.cubeMat;
        shopPlatform.material = SkinManager.platformMat;
        creditsPlatform.material = SkinManager.platformMat;
        exitPlatform.material = SkinManager.platformMat;
        RenderSettings.skybox = SkinManager.backgroundMat;
        Instantiate(cube);
    }
    public void GoToShop()
    {
        SceneManager.LoadScene("Shop");
    }

    public void Play()
    {
       SceneManager.LoadScene("Game");
    }

    public void GoToCredits()
    {

    }

    public void Quit()
    {
        Application.Quit();
    }
}
