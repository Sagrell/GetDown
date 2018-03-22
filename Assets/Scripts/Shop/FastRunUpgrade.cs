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
                price = 50; time = 5; imp = 0;
                break;
            case 1:
                price = 100; time = 2; imp = 0;
                break;
            case 2:
                price = 200; time = 2; imp = 0;
                break;
            case 3:
                price = 500; time = 3; imp = 0;
                break;
            case 4:
                price = 700; time = 3; imp = 0;
                break;
            case 5:
                price = 900; time = 2; imp = 0;
                break;
            case 6:
                price = 1200; time = 5; imp = 0;
                break;
            case 7:
                price = 2000; time = 10; imp = 0;
                break;
            default:
                break;
        }
        if (level == 0)
        {
            improvesText.text = time + " sec";
        }
        else if (imp > 0)
        {
            improvesText.text = "+" + time + " sec +" + imp + " charge";
        }
        else if (time > 0)
        {
            improvesText.text = "+" + time + " sec";
        }
        else
        {
            improvesText.text = "Maximum";
            buy.interactable = false;
        }
        buy.GetComponentInChildren<Text>().text = price.ToString();
        buy.onClick.RemoveAllListeners();
        buy.onClick.AddListener(delegate { ShopManager.Instance.Upgrade("FastRun", price, time, imp); });
        if (DataManager.Instance.GetUserData().GoldAmount < price)
        {
            buy.interactable = false;
        }
        else
        {
            buy.interactable = true;
        }
    }

}
