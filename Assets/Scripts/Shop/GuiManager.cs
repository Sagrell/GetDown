﻿using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GuiManager : MonoBehaviour {
    public static GuiManager Instance;
    public Text currency;
    public GameObject showCube;
    public GameObject showPlatform;

    public GameObject scoreLimit;
    public GameObject buy;
    public GameObject select;

    public Animator fadeAnim;
    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        RenderSettings.skybox = SkinManager.backgroundMat;
        showCube.GetComponent<Renderer>().material = SkinManager.cubeMat;
        showPlatform.GetComponent<Renderer>().material = SkinManager.platformMat;
        currency.text = DataManager.Instance.GetUserData().GoldAmount.ToString();
        SceneController.currentScene = "Shop";
    }

    IEnumerator StartScene(string scene)
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
        SceneController.previousScene = "Shop";
    }

    public void BackToMenu()
    {
        AudioCenter.Instance.PlaySound("Button");
        StartCoroutine(StartScene("Menu"));
    }

    public void PlaySound()
    {
        AudioCenter.Instance.PlaySound("Button");
    }
    public void Show(ShopItem item)
    {
        buy.SetActive(false);
        select.SetActive(false);
        scoreLimit.SetActive(false);
        if (item.bought)
        {
            if (!item.selected)
            {
                select.GetComponent<Button>().onClick.RemoveAllListeners();
                select.GetComponent<Button>().onClick.AddListener(delegate { ShopManager.Instance.Select(item); });
                select.SetActive(true);
            }
        }
        else if (item.locked)
        {
            scoreLimit.GetComponentInChildren<Text>().text = Regex.Replace(scoreLimit.GetComponentInChildren<Text>().text, @"(>)\d+(<)", "${1}" + item.scoreRequire.ToString() + "${2}"); 
            scoreLimit.SetActive(true);
        }
        else
        {
            buy.GetComponentInChildren<Text>().text = Regex.Replace(buy.GetComponentInChildren<Text>().text, @"\d+", item.cost.ToString());
            buy.GetComponent<Button>().onClick.RemoveAllListeners();
            buy.GetComponent<Button>().onClick.AddListener(delegate { ShopManager.Instance.Buy(item); });
            if(DataManager.Instance.GetUserData().GoldAmount < item.cost)
            {
                buy.GetComponent<Button>().interactable = false;
            } else
            {
                buy.GetComponent<Button>().interactable = true;
            }
            buy.SetActive(true);
        }
        switch (item.itemType)
        {
            case "Cube":
                showCube.GetComponent<Renderer>().material = item.objectMat;
                break;
            case "Platform":
                showPlatform.GetComponent<Renderer>().material = item.objectMat;
                break;
            case "Background":
                RenderSettings.skybox = item.objectMat;
                break;
            default:
                break;
        }
        
    }

    public void UpdateInfo()
    {
        currency.text = DataManager.Instance.GetUserData().GoldAmount.ToString();
    }

}