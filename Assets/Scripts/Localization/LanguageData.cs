using System.Collections;
using System.Collections.Generic;


[System.Serializable]
public class LanguageData {
    public LanguageItem[] menuItems;
    public LanguageItem[] shopItems;
    public LanguageItem[] gameItems;
}

[System.Serializable]
public class LanguageItem
{
    public string key;
    public string value;
}