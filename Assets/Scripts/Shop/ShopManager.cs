using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopManager : MonoBehaviour {

    public static ShopManager Instance;
    [Header("Managers")]
    public ItemsManager cubesManager;
    public ItemsManager platformsManager;
    public ItemsManager backgroundsManager;

    DataManager dataManager;
    UserData data;

    private void Awake()
    {
        Instance = this;
    }
    // Use this for initialization
    void Start () {
        dataManager = DataManager.Instance;
        data = dataManager.GetUserData();
    }
	

    public void Select(ShopItem item)
    {
        Material material = item.objectMat;
        item.selected = true;
        dataManager.SaveSelectedItem(item);
        switch (item.itemType)
        {
            
            case "Cube":
                //Apply material to the cube
                SkinManager.cubeMat = material;
                cubesManager.Deselect();
                cubesManager.Initialize();
                break;
            case "Platform":
                //Apply material to the cube
                SkinManager.platformMat = material;
                platformsManager.Deselect();
                platformsManager.Initialize();
                break;
            case "Background":
                SkinManager.backgroundMat = material;
                backgroundsManager.Deselect();
                backgroundsManager.Initialize();
                break;
            default:
                break;
        }
        GuiManager.Instance.Show(item);
    }

    public void Buy(ShopItem item)
    {
        
        data.GoldAmount -= item.cost;
        switch (item.itemType)
        {
            case "Cube":
                data.CubesUnlocked[item.index] = true;
                dataManager.SaveUserData(data);
                cubesManager.Initialize();
                break;
            case "Platform":
                data.PlatformsUnlocked[item.index] = true;
                dataManager.SaveUserData(data);
                Debug.Log(dataManager.GetUserData().PlatformsUnlocked[1]);
                platformsManager.Initialize();
                break;
            case "Background":
                data.BackgroundsUnlocked[item.index] = true;
                dataManager.SaveUserData(data);
                backgroundsManager.Initialize();
                break;
            default:
                break;
        }

        GuiManager.Instance.UpdateInfo();
        GuiManager.Instance.Show(item);
    }
}
