using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class ItemsManager : MonoBehaviour {
    public ScrollRect scrollRect;
    public GameObject item;
    public float cubeSize;

    public string type;

    [Range(0,100)]
    public float space;
    [Range(0, 100)]
    public float scaleOffset;   
    [Range(0, 100)]
    public float scaleSpeed;
    [Range(0, 100)]
    public float snapSpeed;

    UserData data;
    ShopItem selected;
    GameObject[] items;
    ShopItem[] skins;
    Vector2[] itemsPosition;
    Vector2 contentVector;
    Vector2[] cubeScale;
    RectTransform contentRect;
    int selectedCubeId;
    bool isScrolling;
    bool isFocusing;
    // Use this for initialization
    void Start() {
        data = DataManager.Instance.GetUserData();
        Initialize();
    }

    public void Initialize()
    {
        skins = SkinManager.GetSkins(type);
        data = DataManager.Instance.GetUserData();
        contentRect = GetComponent<RectTransform>();
        cubeScale = new Vector2[skins.Length];
        itemsPosition = new Vector2[skins.Length];
        items = new GameObject[skins.Length];
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }
        for (int i = 0; i < skins.Length; i++)
        {
            ShopItem model = skins[i];
            if (model.scoreRequire <= data.HighScore)
            {
                model.locked = false;
            }
            else
            {
                model.locked = true;
            }
            model.selected = false;
            switch (type)
            {
                case "Cube":
                    model.bought = data.CubesUnlocked[i];
                    if(data.SelectedCube==i)
                    {
                        model.selected = true;
                    }
                    break;
                case "Platform":
                    model.bought = data.PlatformsUnlocked[i];
                    if (data.SelectedPlatform == i)
                    {
                        model.selected = true;
                    }
                    break;
                case "Background":
                    model.bought = data.BackgroundsUnlocked[i];
                    if (data.SelectedBackground == i)
                    {
                        model.selected = true;
                    }
                    break;
                default:
                    break;
            }    
            if(model.selected)
            {
                selected = model;
                GuiManager.Instance.Show(model);
            }
            GameObject prefab = Instantiate(item, transform, false);
            prefab.GetComponent<RectTransform>().sizeDelta = new Vector2(cubeSize, cubeSize);
            prefab.name = model.itemName;

            prefab.GetComponent<Skin>().Initialize(model);
            
            if(i!=0)
            {
                prefab.transform.localPosition = new Vector2(items[i-1].transform.localPosition.x + cubeSize + space, 0);
            }
             
            itemsPosition[i] = -prefab.transform.localPosition;
            prefab.GetComponent<Button>().onClick.AddListener(delegate { GuiManager.Instance.Show(model); FocusOn(-prefab.transform.localPosition); PlaySound(); });
            float distance = Mathf.Abs(contentRect.anchoredPosition.x - itemsPosition[i].x);
            float scale = Mathf.Clamp(1 / (distance / space) * scaleOffset, 0.6f, 1f);
            cubeScale[i].x = scale;
            cubeScale[i].y = scale;

            items[i] = prefab;
            items[i].transform.localScale = cubeScale[i];
        }
        for (int i = 0; i < skins.Length; i++)
        {
            if(skins[i].selected)
            {
                FocusOn(itemsPosition[i]);
                break;
            }
        }
        
    }
   
    void FocusOn(Vector2 pos)
    {
        StopAllCoroutines();
        StartCoroutine(FocusingOn(pos));
    }
    IEnumerator FocusingOn (Vector2 pos)
    {
        isFocusing = true;
        scrollRect.horizontal = false;
        while (Mathf.Abs(contentVector.x - pos.x)>(cubeSize+space)/3)
        {
            contentVector.x = Mathf.SmoothStep(contentRect.anchoredPosition.x, pos.x, snapSpeed * Time.deltaTime);
            contentRect.anchoredPosition = contentVector;
            yield return null;
        }
        isFocusing = false;
        scrollRect.horizontal = true;


    }
    public void PlaySound()
    {
        AudioCenter.Instance.PlaySound("Button");
    }
    public void Deselect()
    {
        selected.selected = false;
    }

    int prevCube = -1;
    void Update()
    {
        float minDistance = float.MaxValue;
        for (int i = 0; i < skins.Length; i++)
        {
            float distance = Mathf.Abs(contentRect.anchoredPosition.x - itemsPosition[i].x);
            if (distance < minDistance)
            {
                minDistance = distance;
                selectedCubeId = i;     
            }
            float scale = Mathf.Clamp(1/(distance/space)* scaleOffset, 0.7f, 1f);
            cubeScale[i].x = Mathf.SmoothStep(items[i].transform.localScale.x, scale, scaleSpeed * Time.deltaTime);
            cubeScale[i].y = Mathf.SmoothStep(items[i].transform.localScale.y, scale, scaleSpeed * Time.deltaTime);
            items[i].transform.localScale = cubeScale[i];
        }
        if (prevCube != selectedCubeId && isScrolling)
        {
            PlaySound();
        }
        prevCube = selectedCubeId;
        if (!isScrolling && !isFocusing )
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
