using System.Collections;
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
    public Texture hideTex;
    public GameObject scoreLimit;
    public GameObject buy;
    public GameObject select;
    public Animator fadeAnim;
    public Material transparent;
    public Material diffuse;
    public Renderer background;
    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        background.material = SkinManager.backgroundMat;
        showCube.GetComponent<Renderer>().material.SetTexture("_MainTex", SkinManager.cubeMat.GetTexture("_MainTex"));
        showPlatform.GetComponent<Renderer>().material.SetTexture("_MainTex", SkinManager.platformMat.GetTexture("_MainTex"));
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

    public void ShowBuyCoins()
    {
        fadeAnim.Play("ShowBuyCoins");
    }
    public void HideBuyCoins()
    {
        fadeAnim.Play("HideBuyCoins");
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
            if(DataManager.Instance.GetUserData().GoldAmount < item.cost)
            {
                buy.GetComponent<Button>().onClick.AddListener(delegate { ShowBuyCoins(); });
            } else
            {
                buy.GetComponent<Button>().onClick.AddListener(delegate { ShopManager.Instance.Buy(item); });
            }
            buy.SetActive(true);

        }
        switch (item.itemType)
        {
            case "Cube":
                if(!item.bought)
                {
                    showCube.GetComponent<Renderer>().material = transparent;
                    showCube.GetComponent<Renderer>().material.SetFloat("_Alpha", 0.6f);
                }           
                else
                {
                    showCube.GetComponent<Renderer>().material = diffuse;
                }
                    
                showCube.GetComponent<MeshFilter>().mesh = item.model;
                showCube.GetComponent<Renderer>().material.SetTexture("_MainTex", item.objectMat.GetTexture("_MainTex"));
                showCube.GetComponent<Renderer>().material.SetTexture("_BumpMap", item.objectMat.GetTexture("_BumpMap"));
                break;
            case "Platform":
                if (!item.bought)
                {
                    showPlatform.GetComponent<Renderer>().material = transparent;
                    showPlatform.GetComponent<Renderer>().material.SetFloat("_Alpha", 0.6f);
                }   
                else
                {
                    showPlatform.GetComponent<Renderer>().material = diffuse;
                }   
                showPlatform.GetComponent<MeshFilter>().mesh = item.model;
                showPlatform.GetComponent<Renderer>().material.SetTexture("_MainTex", item.objectMat.GetTexture("_MainTex"));
                showPlatform.GetComponent<Renderer>().material.SetTexture("_BumpMap", item.objectMat.GetTexture("_BumpMap"));
                break;
            case "Background":
                background.material = item.objectMat;
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
