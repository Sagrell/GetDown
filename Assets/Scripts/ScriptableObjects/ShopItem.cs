using UnityEngine;

[System.Serializable]
[CreateAssetMenu(fileName = "New shop item", menuName = "My/ShopItem")]
public class ShopItem : ScriptableObject {

    public int index;
    public string itemName;
    public string itemType;
    public int cost;
    public int scoreRequire;
    public string description;
    public bool bought;
    public bool selected;
    public bool locked;

    public Material objectMat;
    public Material previewMat;
}
