
[System.Serializable]
public class UserData
{ 

    public string Lang;
    public string Version;

    public int HighScore;
    public int GoldAmount;
    public int TotalGold;

    public int currentDayInARow;
    public int prevTimestamp;
    public bool isClaimedGift;
    public bool[] CubesUnlocked;
    public bool[] PlatformsUnlocked;
    public bool[] BackgroundsUnlocked;
    public bool isFirstTime;

    public int SelectedCube;
    public int SelectedPlatform;
    public int SelectedBackground;

    public int[] Shield;
    public int[] Magnet;
    public int[] DoubleCoin;
    public int[] FastRun;

    public float musicVolume;
    public float soundVolume;
    public bool isMute;
    public string hashOfContent;

}
