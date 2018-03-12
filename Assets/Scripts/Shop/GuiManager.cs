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

    public GameObject scoreLimit;
    public GameObject buy;
    public GameObject select;
    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        currency.text = DataManager.Instance.GetUserData().GoldAmount.ToString();
    }
    public void BackToMenu()
    {
        SceneManager.LoadScene("Menu");
    }

    public void Show(CubeModel cube)
    {
        buy.SetActive(false);
        select.SetActive(false);
        scoreLimit.SetActive(false);
        showCube.GetComponent<Renderer>().material = cube.CubeMaterial;
        if (cube.bought)
        {
            if (!cube.selected)
            {
                select.GetComponent<Button>().onClick.RemoveAllListeners();
                select.GetComponent<Button>().onClick.AddListener(delegate { ShopManager.Instance.Select(cube); });
                select.SetActive(true);
            }
        }
        else if (cube.locked)
        {
            scoreLimit.GetComponentInChildren<Text>().text = Regex.Replace(scoreLimit.GetComponentInChildren<Text>().text, @"(>)\d+(<)", "${1}" +cube.scoreRequire.ToString() + "${2}"); 
            scoreLimit.SetActive(true);
        }
        else
        {
            buy.GetComponentInChildren<Text>().text = Regex.Replace(buy.GetComponentInChildren<Text>().text, @"\d+", cube.cost.ToString());
            buy.GetComponent<Button>().onClick.RemoveAllListeners();
            buy.GetComponent<Button>().onClick.AddListener(delegate { ShopManager.Instance.Buy(cube); });
            if(DataManager.Instance.GetUserData().GoldAmount < cube.cost)
            {
                buy.GetComponent<Button>().interactable = false;
            } else
            {
                buy.GetComponent<Button>().interactable = true;
            }
            buy.SetActive(true);
        }
        
    }

    public void UpdateInfo()
    {
        currency.text = DataManager.Instance.GetUserData().GoldAmount.ToString();
    }

}
