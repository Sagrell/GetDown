
[System.Serializable]
public class UserData
{ 

    public string PlayerName;
    public string Version;

    public int HighScore;
    public int GoldAmount;

    public bool[] CubesUnlocked;
    public bool[] PlatformsUnlocked;
    public bool[] BackgroundsUnlocked;

    public int SelectedCube;
    public int SelectedPlatform;
    public int SelectedBackground;

    public int[] Shield;
    public int[] Magnet;
    public int[] DoubleCoin;
    public int[] FastRun;

    public string hashOfContent;

}
