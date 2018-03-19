using System;
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
       }
       userData.GoldAmount += GameState.countCoins;
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

    void SaveDefaultData()
    {
        userData = new UserData()
        {
            PlayerName = "Sagrell",
            Version = "1.0",
            HighScore = 100,
            GoldAmount = 10000,
            CubesUnlocked = new bool[9] { true, false, false, false, false, false, false, false, false },
            PlatformsUnlocked = new bool[8] { true, false, false, false, false, false, false, false },
            BackgroundsUnlocked = new bool[8] { true, false, false, false, false, false, false, false },
            SelectedCube = 0,
            SelectedPlatform = 0,
            SelectedBackground = 0,
            Shield = new int[] { 0, 0, 0 },
            Magnet = new int[] { 0, 0, 0 },
            DoubleCoin = new int[] { 0, 0, 0 },
            FastRun = new int[] { 0, 0, 0 }
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
