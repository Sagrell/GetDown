using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FastRunUpgrade : Upgrade {

    // Use this for initialization
    public override void Initialize()
    {
        maxLevel = 8;
        foreach (Transform child in levelContainer)
        {
            Destroy(child.gameObject);
        }
        level = dataManager.GetUserData().FastRun[0];
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
                price = 300; time = 5; imp = 0;
                break;
            case 1:
                price = 600; time = 2; imp = 0;
                break;
            case 2:
                price = 990; time = 2; imp = 0;
                break;
            case 3:
                price = 1500; time = 3; imp = 0;
                break;
            case 4:
                price = 2000; time = 3; imp = 0;
                break;
            case 5:
                price = 3000; time = 2; imp = 0;
                break;
            case 6:
                price = 5000; time = 5; imp = 0;
                break;
            case 7:
                price = 10000; time = 10; imp = 0;
                break;
            default:
                break;
        }
        if (level == 0)
        {
            improvesText.text = time + secStr;
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
            buy.onClick.AddListener(delegate { ShopManager.Instance.Upgrade("FastRun", price, time, imp); });
        }
    }

}
