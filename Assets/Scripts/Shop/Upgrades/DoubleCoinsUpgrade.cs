using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DoubleCoinsUpgrade : Upgrade {

    // Use this for initialization
    public override void Initialize()
    {
        maxLevel = 8;
        foreach (Transform child in levelContainer)
        {
            Destroy(child.gameObject);
        }
        level = dataManager.GetUserData().DoubleCoin[0];
        for (int i = 0; i < maxLevel; i++)
        {
            if (i < level)
            {
                Instantiate(point, levelContainer);
            }
            else
            {
                Instantiate(emptyPoint, levelContainer);
            }

        }
        int price = 0;
        int time = 0;
        int imp = 0;
        switch (level)
        {
            case 0:
                price = 50; time = 10; imp = 0;
                break;
            case 1:
                price = 100; time = 2; imp = 0;
                break;
            case 2:
                price = 200; time = 3; imp = 0;
                break;
            case 3:
                price = 500; time = 3; imp = 0;
                break;
            case 4:
                price = 700; time = 3; imp = 0;
                break;
            case 5:
                price = 900; time = 3; imp = 0;
                break;
            case 6:
                price = 1200; time = 5; imp = 0;
                break;
            case 7:
                price = 2500; time = 10; imp = 1;
                break;
            default:
                break;
        }
        if (level == 0)
        {
            improvesText.text = time + secStr;
        }
        else if (imp > 0)
        {
            improvesText.text = "+" + time + " x3";
        }
        else if (time > 0)
        {
            improvesText.text = "+" + time + secStr;
        }
        else
        {
            improvesText.text = maximumStr;
            buy.gameObject.SetActive(false);
        }
        buy.GetComponentInChildren<Text>().text = price.ToString();
        buy.onClick.RemoveAllListeners();
        if (DataManager.Instance.GetUserData().GoldAmount < price)
        {
            buy.onClick.AddListener(delegate { GuiManager.Instance.ShowBuyCoins(); });
        }
        else
        {
            buy.onClick.AddListener(delegate { ShopManager.Instance.Upgrade("DoubleCoin", price, time, imp); });
        }
    }

}
