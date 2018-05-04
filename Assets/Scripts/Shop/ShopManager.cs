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
    public ParticleSystem playerConfetti;
    public ParticleSystem platformConfetti;
    public ParticleSystem backgroundConfetti;
    public Animator cubeAnim;
    public Animator platformAnim;
    DataManager dataManager;
    UserData data;
    public Animator buyDoubleCoins;
    public GameObject check;
    public GameObject buy;
    public GameObject check1;
    public GameObject buy1;
    private void Awake()
    {
        Instance = this;
    }
    // Use this for initialization
    void Start()
    {
        dataManager = DataManager.Instance;
        data = dataManager.GetUserData();
        PurchaseManager.Instance.doubleCoins = buyDoubleCoins;
        PurchaseManager.Instance.disableAd = buyDoubleCoins;
        if (PurchaseManager.isDoubleCoins)
        {
            buy.SetActive(false);
            check.SetActive(true);
        }
        if (PurchaseManager.isDisabledAd)
        {
            buy1.SetActive(false);
            check1.SetActive(true);
        }
    }


    public void Select(ShopItem item)
    {
        AudioCenter.Instance.PlaySound("Button");
        Material material = item.objectMat;
        Mesh mesh = item.model;
        item.selected = true;
        dataManager.SaveSelectedItem(item);
        switch (item.itemType)
        {

            case "Cube":
                //Apply material to the cube
                SkinManager.cubeMat = material;
                SkinManager.cubeMesh = mesh;
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
        AudioCenter.Instance.PlaySound("Purchase");
        
        data.GoldAmount -= item.cost;
        switch (item.itemType)
        {
            case "Cube":
                data.CubesUnlocked[item.index] = true;
                dataManager.SaveUserData(data);
                Select(item);
                cubeAnim.Play("BuyCube");
                playerConfetti.gameObject.SetActive(true);
                playerConfetti.Play();
                break;
            case "Platform":
                data.PlatformsUnlocked[item.index] = true;
                dataManager.SaveUserData(data);
                Select(item);
                platformAnim.Play("BuyPlatform");
                platformConfetti.gameObject.SetActive(true);
                platformConfetti.Play();
                break;
            case "Background":
                data.BackgroundsUnlocked[item.index] = true;
                dataManager.SaveUserData(data);
                Select(item);
                backgroundConfetti.gameObject.SetActive(true);
                backgroundConfetti.Play();
                break;
            default:
                break;
        }

        GuiManager.Instance.UpdateInfo();
        
        GuiManager.Instance.Show(item);
    }
    public void BuyConsumable(int id)
    {
        
        PurchaseManager.Instance.BuyConsumable(id);
    }
    public void BuyNonConsumable(int id)
    {

        PurchaseManager.Instance.BuyNonConsumable(id);
    }
    private void OnApplicationPause(bool pause)
    {
        if (pause)
        {
            AudioCenter.Instance.PauseMusic("MenuTheme", .1f);
            Time.timeScale = 0f;
        }
        else
        {
            Time.timeScale = 1f;
            AudioCenter.Instance.ResumeMusic("MenuTheme", .1f);
        }
    }
    public void Upgrade(string type, int cost, int time, int imp)
    {
        AudioCenter.Instance.PlaySound("Purchase");
        data.GoldAmount -= cost;
        switch (type)
        {
            case "Shield":
                if(data.Shield[0]++ == 0)
                {
                    GooglePlayManager.Instance.Achieve(GooglePlayManager.shieldAchieve);
                }
                data.Shield[1] += time;
                data.Shield[2] += imp;
                break;
            case "Magnet":
                if (data.Magnet[0]++ == 0)
                {
                    GooglePlayManager.Instance.Achieve(GooglePlayManager.magnetAchieve);
                }
                data.Magnet[1] += time;
                data.Magnet[2] += imp;
                break;
            case "DoubleCoin":
                if (data.DoubleCoin[0]++ == 0)
                {
                    GooglePlayManager.Instance.Achieve(GooglePlayManager.doubleCoinsAchieve);
                }
                data.DoubleCoin[1] += time;
                data.DoubleCoin[2] += imp;
                break;
            case "FastRun":
                if (data.FastRun[0]++ == 0)
                {
                    GooglePlayManager.Instance.Achieve(GooglePlayManager.fastRunAchieve);
                }
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
