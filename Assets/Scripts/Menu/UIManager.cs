using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour {

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
