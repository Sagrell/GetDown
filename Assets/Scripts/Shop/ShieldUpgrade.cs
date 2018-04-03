using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShieldUpgrade : Upgrade {

    // Use this for initialization
    public override void Initialize()
    {
        maxLevel = 8;
        foreach (Transform child in levelContainer)
        {
            Destroy(child.gameObject);
        }
        level = dataManager.GetUserData().Shield[0];
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
                price = 100; time = 20; imp = 1;     
                break;
            case 1:
                price = 150; time = 5; imp = 0;
                break;
            case 2:
                price = 300; time = 8; imp = 0;
                break;
            case 3:
                price = 600; time = 0;  imp = 1;
                break;
            case 4:
                price = 800; time = 10; imp = 0;
                break;
            case 5:
                price = 1000; time = 0; imp = 1;
                break;
            case 6:
                price = 1300; time = 10; imp = 0;
                break;
            case 7:
                price = 2500; time = 20; imp = 1;
                break;
            default:
                break;
        }
        if (level == 0)
        {
            improvesText.text = time + " sec\n"+imp+" charge";
        }
        else if (imp>0 && time > 0)
        {
            improvesText.text = "+" + time + " sec\n+"+imp+" charge";
        } else  if(time > 0)
        {
            improvesText.text = "+" + time + " sec";
        } else if (imp > 0)
        {
            improvesText.text = "+" + imp + " charge";
        } else
        {
            improvesText.text = "Maximum";
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
            buy.onClick.AddListener(delegate { ShopManager.Instance.Upgrade("Shield", price, time, imp); });
        }
    }

}
