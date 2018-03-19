using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoBehaviour {
    public static bool isComplete = true;
    public Text coins;
    public GameObject cube;
    public Transform cubePosition;
    public Renderer shopPlatform;
    public Renderer creditsPlatform;
    public Renderer exitPlatform;

    public Animator fadeAnim;
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
        Instantiate(cube, cubePosition);
    }
    public void GoToShop()
    {
        StartCoroutine(StartScene("Shop"));
    }

    public void Play()
    {
        StartCoroutine(StartScene("Game"));
    }

    public void GoToCredits()
    {

    }

    public void Quit()
    {
        Application.Quit();
    }

    IEnumerator StartScene( string scene )
    {
        fadeAnim.Play("FadeOut");
        AsyncOperation loading = SceneManager.LoadSceneAsync(scene);
        fadeAnim.Update(0f);
        loading.allowSceneActivation = false;
        while (fadeAnim.GetCurrentAnimatorStateInfo(0).IsName("Main.FadeOut"))
        {
            yield return null;
        }
        loading.allowSceneActivation = true;

    }

}
