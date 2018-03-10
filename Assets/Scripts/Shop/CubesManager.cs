using UnityEngine;
using UnityEngine.UI;

public class CubesManager : MonoBehaviour {
    public GameObject item;
    public CubeModel[] models;
    [Range(0,100)]
    public float space;
    [Range(0, 100)]
    public float scaleOffset;
    [Range(0, 100)]
    public float scaleSpeed;
    [Range(0, 100)]
    public float snapSpeed;

    UserData data;
    CubeModel selected;
    GameObject[] items;
    Vector2[] itemsPosition;
    Vector2 contentVector;
    Vector2[] cubeScale;
    RectTransform contentRect;
    int selectedCubeId;
    bool isScrolling;
    // Use this for initialization
    void Start() {
        data = DataManager.Instance.GetUserData();
        Initialize();
    }

    public void Initialize()
    {       
        contentRect = GetComponent<RectTransform>();
        cubeScale = new Vector2[models.Length];
        itemsPosition = new Vector2[models.Length];
        items = new GameObject[models.Length];
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
            GameObject prefab = Instantiate(item, transform, false);
            prefab.name = model.cubeName;
            prefab.GetComponent<Skin>().Initialize(model);
            prefab.GetComponent<Button>().onClick.AddListener(delegate { GuiManager.Instance.Show(model); });
            if(i!=0)
            {
                prefab.transform.localPosition = new Vector2(items[i-1].transform.localPosition.x + item.GetComponent<RectTransform>().sizeDelta.x + space, 0);
            }
            items[i] = prefab;
            itemsPosition[i] = -prefab.transform.localPosition;

            float distance = Mathf.Abs(contentRect.anchoredPosition.x - itemsPosition[i].x);
            float scale = Mathf.Clamp(1 / (distance / space) * scaleOffset, 0.6f, 1f);
            cubeScale[i].x = scale;
            cubeScale[i].y = scale;
            items[i].transform.localScale = cubeScale[i];
        }
    }
   
    public void Deselect()
    {
        selected.selected = false;
    }

    private void Update()
    {
        float minDistance = float.MaxValue;
        for (int i = 0; i < models.Length; i++)
        {
            float distance = Mathf.Abs(contentRect.anchoredPosition.x - itemsPosition[i].x);
            if(distance < minDistance)
            {
                minDistance = distance;
                selectedCubeId = i;
            }
            float scale = Mathf.Clamp(1/(distance/space)* scaleOffset, 0.7f, 1f);
            cubeScale[i].x = Mathf.SmoothStep(items[i].transform.localScale.x, scale, scaleSpeed * Time.deltaTime);
            cubeScale[i].y = Mathf.SmoothStep(items[i].transform.localScale.y, scale, scaleSpeed * Time.deltaTime);
            items[i].transform.localScale = cubeScale[i];
        }
        if(!isScrolling)
        {
            contentVector.x = Mathf.SmoothStep(contentRect.anchoredPosition.x, itemsPosition[selectedCubeId].x, snapSpeed * Time.deltaTime);
            contentRect.anchoredPosition = contentVector;
        }
        
    }

    public void Scrolling(bool scroll)
    {
        isScrolling = scroll;
    }
}
