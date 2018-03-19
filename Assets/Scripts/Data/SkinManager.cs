using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Skins
{
    public string type;
    public ShopItem[] items;
}
public class SkinManager : MonoBehaviour {
    public static SkinManager Instance;

    public Skins[] skins;

    static Dictionary<string, ShopItem[]> itemsDictionary;
    DataManager dataManager;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            itemsDictionary = new Dictionary<string, ShopItem[]>();
            for (int i = 0; i < skins.Length; i++)
            {
                Skins skin = skins[i];
                if(!itemsDictionary.ContainsKey(skin.type))
                {
                    ShopItem[] newList = new ShopItem[skin.items.Length];
                    itemsDictionary.Add(skin.type, newList);
                }
                for (int j = 0; j < skin.items.Length; j++)
                {
                    itemsDictionary[skin.type][j] = skin.items[j];
                }
            }
            dataManager = DataManager.Instance;
            cubeMat = itemsDictionary["Cube"][dataManager.GetUserData().SelectedCube].objectMat;
            platformMat = itemsDictionary["Platform"][dataManager.GetUserData().SelectedPlatform].objectMat;
            backgroundMat = itemsDictionary["Background"][dataManager.GetUserData().SelectedBackground].objectMat;
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

    public static ShopItem[] GetSkins(string type)
    {
        return itemsDictionary[type];
    }
}
