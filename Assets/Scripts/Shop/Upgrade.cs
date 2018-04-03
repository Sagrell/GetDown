using UnityEngine;
using UnityEngine.UI;

public abstract class Upgrade : MonoBehaviour {
    [HideInInspector]
    public GameObject point;
    [HideInInspector]
    public GameObject emptyPoint;
    public Transform levelContainer;
    public Text improvesText;
    public Button buy;

    protected int maxLevel;
    protected int level;  
    protected DataManager dataManager;
    private void Start()
    {
        dataManager = DataManager.Instance;
        Initialize();
        
    }
    abstract public void Initialize();
}
