using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class LanguageManager : MonoBehaviour {

    #region Singleton
    public static LanguageManager Instance;
    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        } else
        {
            Destroy(gameObject);
        }
    }
    #endregion

    //Localization
    public static Dictionary<string,Dictionary<string, string>> localization;

    //Data
    LanguageData languageData;
    DataManager dataManager;
    UserData data;
    string currentLang;
    // Use this for initialization
    void Start () {
        dataManager = DataManager.Instance;
        data = dataManager.GetUserData();
        currentLang = data.Lang;
        
        ChangeLanguage(currentLang);
    }
	
    public void ChangeLanguage(string lang)
    {
        currentLang = lang;

#if UNITY_ANDROID && !UNITY_EDITOR
        string filepath = Application.persistentDataPath + "/" + currentLang +".json";

        if (!File.Exists(filepath))

        {
            WWW loadDB = new WWW("jar:file://" + Application.dataPath + "!/assets/" + "Resources/Languages/" + lang + ".json");  // this is the path to your StreamingAssets in android

            while (!loadDB.isDone) { } 
            File.WriteAllBytes(filepath, loadDB.bytes);

        }
        languageData = JsonUtility.FromJson<LanguageData>(File.ReadAllText(filepath));
#else
        languageData = JsonUtility.FromJson<LanguageData>(File.ReadAllText(Application.streamingAssetsPath + "/Resources/Languages/" + lang + ".json"));
#endif
        localization = new Dictionary<string, Dictionary<string, string>>();
        //Menu scene localization
        LanguageItem[] menuItems = languageData.menuItems;
        Dictionary<string, string> menuData = new Dictionary<string, string>();

        for (int i = 0; i < menuItems.Length; i++)
        {
            menuData.Add(menuItems[i].key, menuItems[i].value);
        }
        localization.Add("Menu", menuData);

        //Shop scene localization
        LanguageItem[] shopItems = languageData.shopItems;
        Dictionary<string, string> shopData = new Dictionary<string, string>();

        for (int i = 0; i < shopItems.Length; i++)
        {
            shopData.Add(shopItems[i].key, shopItems[i].value);
        }
        localization.Add("Shop", shopData);

        //Game scene localization
        LanguageItem[] gameItems = languageData.gameItems;
        Dictionary<string, string> gameData = new Dictionary<string, string>();

        for (int i = 0; i < gameItems.Length; i++)
        {
            gameData.Add(gameItems[i].key, gameItems[i].value);
        }
        localization.Add("Game", gameData);

        
        if(!currentLang.Equals(data.Lang))
        {
            data.Lang = lang;
            dataManager.SaveUserData(data);
            LocalizedText[] texts = FindObjectsOfType<LocalizedText>();
            for (int i = 0; i < texts.Length; i++)
            {
                texts[i].SetText();
            }
        }
    }
    public string GetLocalizedValue(string scene, string key)
    {
        string result = "Missing";
        if(localization.ContainsKey(scene) && localization[scene].ContainsKey(key))
        {
            result = localization[scene][key];
        }
        return result;
    }
}
