﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using UnityEngine;

public class DataManager : MonoBehaviour {

    public static DataManager Instance;

    UserData userData;
    string savePath;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;

            savePath = Path.Combine(Application.persistentDataPath, "saves");
            if (!File.Exists(savePath))
            {
                File.Create(savePath).Dispose();
                SaveDefaultData();
            }
            else
            {
                Load();
            }
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);
    }
    public void SaveWithGameState()
    {
       if(GameState.score> userData.HighScore)
       {
         userData.HighScore = GameState.score;
         GooglePlayManager.Instance.SetNewRecord(GameState.score);
       }
       userData.GoldAmount += GameState.countCoins;
       userData.TotalGold += GameState.countCoins;
        if(userData.TotalGold >= 300)
        {
            GooglePlayManager.Instance.Achieve(GooglePlayManager.trtWealthAchieve);
            if (userData.TotalGold >= 1000)
            {
                GooglePlayManager.Instance.Achieve(GooglePlayManager.otrtWealthAchieve);
                if (userData.TotalGold >= 5000)
                {
                    GooglePlayManager.Instance.Achieve(GooglePlayManager.richGuyAchieve);
                    if (userData.TotalGold >= 11000)
                    {
                        GooglePlayManager.Instance.Achieve(GooglePlayManager.cugmSomeMoneyAchieve);
                        if(userData.TotalGold >= 100000)
                        {
                            GooglePlayManager.Instance.Achieve(GooglePlayManager.whyAchieve);
                        }
                    }
                }
            }
        }
       userData.hashOfContent = GenerateHashFromData(userData);
       BinaryWrite(JsonUtility.ToJson(userData));
    }
    public void SaveSelectedItem(ShopItem item)
    {
        switch (item.itemType)
        {
            case "Cube":
                userData.SelectedCube = item.index;
                break;
            case "Platform":
                userData.SelectedPlatform = item.index;
                break;
            case "Background":
                userData.SelectedBackground = item.index;
                break;
            default:
                break;
        }
        userData.hashOfContent = GenerateHashFromData(userData);
        BinaryWrite(JsonUtility.ToJson(userData));
    }

    public void Load()
    {
        string saveData = BinaryRead();
        UserData tempData = JsonUtility.FromJson<UserData>(saveData);

        string hash = tempData.hashOfContent;
        if (GenerateHashFromData(tempData).Equals(hash))
        {
            userData = tempData;
            userData.hashOfContent = hash;
        } else
        {
            SaveDefaultData();
        }
    }

    public void SaveDefaultData()
    {
        userData = new UserData()
        {
            Lang = "En-en",
            Version = "1.0",
            HighScore = 0,
            GoldAmount = 0,
            TotalGold = 0,
            CubesUnlocked = new bool[28] { true, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false },
            PlatformsUnlocked = new bool[16] { true, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false },
            BackgroundsUnlocked = new bool[25] { true, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false },
            SelectedCube = 0,
            isClaimedGift = false,
            currentDayInARow = -1,
            prevTimestamp = 0,
            SelectedPlatform = 0,
            SelectedBackground = 0,
            Shield = new int[] { 0, 0, 0 },
            Magnet = new int[] { 0, 0, 0 },
            DoubleCoin = new int[] { 0, 0, 0 },
            FastRun = new int[] { 0, 0, 0 },
            musicVolume = 1f,
            soundVolume = 1f,
            isFirstTime = true,
            isMute = false
        };
        userData.hashOfContent = GenerateHashFromData(userData);

        BinaryWrite(JsonUtility.ToJson(userData));
    }
    public void SaveFullData()
    {
        userData = new UserData()
        {
            Lang = "En-en",
            Version = "1.0",
            HighScore = 1000,
            GoldAmount = 1000000,
            TotalGold = 1000000,
            currentDayInARow = -1,
            prevTimestamp = 0,
            isClaimedGift = false,
            CubesUnlocked = new bool[28] { true, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false,false, false, false, false, false, false, false, false, false, false, false, false },
            PlatformsUnlocked = new bool[16] { true, false,false, false, false, false, false, false, false, false, false, false, false, false, false, false },
            BackgroundsUnlocked = new bool[25] { true, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false },
            SelectedCube = 0,
            SelectedPlatform = 0,
            SelectedBackground = 0,
            Shield = new int[] { 0, 0, 0 },
            Magnet = new int[] { 0, 0, 0 },
            DoubleCoin = new int[] { 0, 0, 0 },
            FastRun = new int[] { 0, 0, 0 },
            musicVolume = 1f,
            soundVolume = 1f,
            isFirstTime = true,
            isMute = false
        };
        userData.hashOfContent = GenerateHashFromData(userData);

        BinaryWrite(JsonUtility.ToJson(userData));
    }
    string GenerateHashFromData(UserData userData)
    {
        userData.hashOfContent = "0105199627041997VLADIRAUESMYOCUDONTEVENTRY";
        string saveContent = JsonUtility.ToJson(userData, true);
        SHA256Managed crypt = new SHA256Managed();
        string hash = string.Empty;

        byte[] crypto = crypt.ComputeHash(Encoding.UTF8.GetBytes(saveContent), 0, Encoding.UTF8.GetByteCount(saveContent));

        for (int i = 0; i < crypto.Length; i++)
        {
            hash += (crypto[i]+i*(i-1)).ToString("x2");
        }

        return hash;
    }

    void BinaryWrite(string content)
    {
        File.WriteAllText(savePath, StringToBinary(content));
    }
    string BinaryRead()
    {
        return BinaryToString(File.ReadAllText(savePath));
    }
    string StringToBinary(string data)
    {
        StringBuilder sb = new StringBuilder();

        foreach (char c in data.ToCharArray())
        {
            sb.Append(Convert.ToString(c, 2).PadLeft(8, '0'));
        }
        return sb.ToString();
    }

    string BinaryToString(string data)
    {
        List<byte> byteList = new List<byte>();

        for (int i = 0; i < data.Length; i += 8)
        {
            byteList.Add(Convert.ToByte(data.Substring(i, 8), 2));
        }
        return Encoding.ASCII.GetString(byteList.ToArray());
    }

    public UserData GetUserData()
    {
        return userData;
    }
    public void SaveUserData(UserData data)
    {
        userData = data;
        data.hashOfContent = GenerateHashFromData(data);
        BinaryWrite(JsonUtility.ToJson(data));
    }
}
