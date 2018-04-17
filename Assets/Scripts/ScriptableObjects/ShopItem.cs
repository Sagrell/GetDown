using UnityEngine;

[System.Serializable]
[CreateAssetMenu(fileName = "New shop item", menuName = "My/ShopItem")]
public class ShopItem : ScriptableObject {

    public int index;
    public string itemType;
    public int cost;
    public int scoreRequire;
    public bool bought;
    public bool selected;
    public bool locked;

    public Mesh model;
    public Material objectMat;
    public Material previewMat;
}
