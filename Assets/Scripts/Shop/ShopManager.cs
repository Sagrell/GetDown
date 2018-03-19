using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopManager : MonoBehaviour
{

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
    void Start()
    {
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
                Select(item);
                break;
            case "Platform":
                data.PlatformsUnlocked[item.index] = true;
                dataManager.SaveUserData(data);
                Select(item);
                break;
            case "Background":
                data.BackgroundsUnlocked[item.index] = true;
                dataManager.SaveUserData(data);
                Select(item);
                break;
            default:
                break;
        }

        GuiManager.Instance.UpdateInfo();
        GuiManager.Instance.Show(item);
    }
    public void Upgrade(string type, int cost, int time, int imp)
    {

        data.GoldAmount -= cost;
        switch (type)
        {
            case "Shield":
                data.Shield[0]++;
                data.Shield[1] += time;
                data.Shield[2] += imp;
                break;
            case "Magnet":
                data.Magnet[0]++;
                data.Magnet[1] += time;
                data.Magnet[2] += imp;
                break;
            case "DoubleCoin":
                data.DoubleCoin[0]++;
                data.DoubleCoin[1] += time;
                data.DoubleCoin[2] += imp;
                break;
            case "FastRun":
                data.FastRun[0]++;
                data.FastRun[1] += time;
                data.FastRun[2] += imp;
                break;
            default:
                break;
        }
        dataManager.SaveUserData(data);
        UpgradeManager.Instance.Initialize();
        GuiManager.Instance.UpdateInfo();
    }
}
