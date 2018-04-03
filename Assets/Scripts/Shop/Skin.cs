using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Skin : MonoBehaviour {
    [Header("References")]
    public GameObject locked;
    public GameObject selected;
    public GameObject buy;
    public Image border;

    [Header("RareColor:")]
    public Color bronze;
    public Color silver;
    public Color gold;
    public Color diamond;
    public Color red;
    public Color purple;
    Image image;

    public void Initialize(ShopItem model)
    {
        image = GetComponent<Image>();
        image.material = model.previewMat;
        selected.SetActive(model.selected);
        locked.SetActive(model.locked);
        Color borderColor = bronze;
        
        if (model.cost>=300)
        {
            borderColor = silver;
        }
        if(model.cost >= 700)
        {
            borderColor = gold;
        }
        if (model.cost >= 1500)
        {
            borderColor = diamond;
        }
        if (model.cost >= 3000)
        {
            borderColor = red;
        }
        if (model.cost >= 6000)
        {
            borderColor = purple;
        }

        border.color = borderColor;
        if (!model.bought && !model.locked)
        {
            buy.SetActive(true);
        }
        else
        {
            buy.SetActive(false);
        }
    }
}
