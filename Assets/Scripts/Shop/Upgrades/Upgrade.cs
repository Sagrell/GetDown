using UnityEngine;
using UnityEngine.UI;

public abstract class Upgrade : MonoBehaviour {
    public GameObject point;
    public GameObject emptyPoint;
    public Transform levelContainer;
    public Text improvesText;
    public Button buy;

    protected int maxLevel;
    protected int level;  
    protected DataManager dataManager;
    protected string secStr;
    protected string chargeStr;
    protected string maximumStr;
    private void Start()
    {
        dataManager = DataManager.Instance;
        secStr = LanguageManager.Instance.GetLocalizedValue("Shop", "Second");
        chargeStr = LanguageManager.Instance.GetLocalizedValue("Shop", "Charge");
        maximumStr = LanguageManager.Instance.GetLocalizedValue("Shop", "Maximum");
        Initialize();    
    }
    abstract public void Initialize();
}
