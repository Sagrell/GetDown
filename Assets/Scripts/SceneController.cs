using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneController : MonoBehaviour {

    public static SceneController Instance;
    public static string currentScene;
    public static string previousScene;
    private void Awake()
    {

        if (Instance == null)
        {
            Instance = this;
            currentScene = "Menu";
            previousScene = "";
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);

    }

}
