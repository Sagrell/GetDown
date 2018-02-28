using UnityEngine;
using UnityEngine.UI;

public class CubesManager : MonoBehaviour {
    public GameObject item;
    public CubeModel[] models;


    UserData data;
    CubeModel selected;
    // Use this for initialization
    void Start() {
        data = DataManager.Instance.GetUserData();
        Initialize();
    }

    public void Initialize()
    {
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }
        for (int i = 0; i < models.Length; i++)
        {
            CubeModel model = models[i];
            if (model.scoreRequire <= data.HighScore)
            {
                model.locked = false;
            }
            else
            {
                model.locked = true;
            }
            model.bought = data.CubesUnlocked[i];
            if(model.selected)
            {
                selected = model;
                GuiManager.Instance.Show(model);
            }
            GameObject prefab = Instantiate(item, transform);
            prefab.name = model.cubeName;
            prefab.GetComponent<Skin>().Initialize(model);
            prefab.GetComponent<Button>().onClick.AddListener(delegate { GuiManager.Instance.Show(model); });
        }
    }

    public void Deselect()
    {
        selected.selected = false;
    }

}
