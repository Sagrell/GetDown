using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkinManager : MonoBehaviour {
    public static SkinManager Instance;

    public ShopItem[] items;

    Dictionary<string, List<ShopItem>> itemsDictionary;
    DataManager dataManager;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            itemsDictionary = new Dictionary<string, List<ShopItem>>();
            for (int i = 0; i < items.Length; i++)
            {
                ShopItem item = items[i];
                if(!itemsDictionary.ContainsKey(item.itemType))
                {
                    List<ShopItem> newList = new List<ShopItem>();
                    itemsDictionary.Add(item.itemType, newList);
                }
                itemsDictionary[item.itemType].Add(item);

            }

            dataManager = DataManager.Instance;
            cubeMat = itemsDictionary["Cube"].Find((m) => (m.index == dataManager.GetUserData().SelectedCube)).objectMat;
            platformMat = itemsDictionary["Platform"].Find((m) => (m.index == dataManager.GetUserData().SelectedPlatform)).objectMat;
            backgroundMat = itemsDictionary["Background"].Find((m) => (m.index == dataManager.GetUserData().SelectedBackground)).objectMat;
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);
    }

    public static Material cubeMat;
    public static Material platformMat;
    public static Material backgroundMat;
}
