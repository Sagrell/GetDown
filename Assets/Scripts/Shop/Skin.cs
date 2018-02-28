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
    Button button;

    public void Initialize(CubeModel model)
    {
        image = GetComponent<Image>();
        button = GetComponent<Button>();
        image.material = model.ShowMaterial;
        selected.SetActive(model.selected);
        locked.SetActive(model.locked);
        //button.interactable = !model.locked && model.bought;
        if (!model.bought && !model.locked)
        {
            cost.SetActive(true);
            cost.GetComponent<Text>().text = model.cost.ToString();

        }
        else
        {
            cost.SetActive(false);
        }
    }
}
