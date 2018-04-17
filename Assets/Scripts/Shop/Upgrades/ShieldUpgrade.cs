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
                price = 400; time = 20; imp = 1;     
                break;
            case 1:
                price = 800; time = 5; imp = 0;
                break;
            case 2:
                price = 1500; time = 8; imp = 0;
                break;
            case 3:
                price = 2200; time = 0;  imp = 1;
                break;
            case 4:
                price = 3500; time = 10; imp = 0;
                break;
            case 5:
                price = 6000; time = 0; imp = 1;
                break;
            case 6:
                price = 9000; time = 10; imp = 0;
                break;
            case 7:
                price = 15000; time = 20; imp = 1;
                break;
            default:
                break;
        }
        if (level == 0)
        {
            improvesText.text = time + secStr + "\n" + imp+ chargeStr;
        }
        else if (imp>0 && time > 0)
        {
            improvesText.text = "+" + time + secStr+"\n+"+imp + chargeStr;
        } else  if(time > 0)
        {
            improvesText.text = "+" + time + secStr;
        } else if (imp > 0)
        {
            improvesText.text = "+" + imp + chargeStr;
        } else
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
            buy.onClick.AddListener(delegate { ShopManager.Instance.Upgrade("Shield", price, time, imp); });
        }
    }

}
