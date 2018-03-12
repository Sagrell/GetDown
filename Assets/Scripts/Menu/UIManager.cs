using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoBehaviour {
    public Text coins;
    public GameObject cube;
    public Transform cubePosition;
    UserData data;
    private void Start()
    {
        data = DataManager.Instance.GetUserData();
        coins.text = data.GoldAmount.ToString();
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
