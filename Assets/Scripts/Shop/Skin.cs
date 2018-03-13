using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Skin : MonoBehaviour {
    [Header("References")]
    public GameObject locked;
    public GameObject selected;
    public GameObject cost;

    Image image;

    public void Initialize(ShopItem model)
    {
        image = GetComponent<Image>();
        image.material = model.previewMat;
        selected.SetActive(model.selected);
        locked.SetActive(model.locked);
        //button.interactable = !model.locked && model.bought;
        if (!model.bought && !model.locked)
        {
            cost.SetActive(true);
            cost.GetComponentInChildren<Text>().text = model.cost.ToString();

        }
        else
        {
            cost.SetActive(false);
        }
    }
}
