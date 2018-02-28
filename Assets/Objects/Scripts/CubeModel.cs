using UnityEngine;

[System.Serializable]
[CreateAssetMenu(fileName = "New Cube", menuName = "My/CubeModel")]
public class CubeModel : ScriptableObject {

    public int index;
    public string cubeName;
    public int cost;
    public int scoreRequire;
    public string description;
    public bool bought;
    public bool selected;
    public bool locked;

    public Material CubeMaterial;
    public Material ShowMaterial;
}
