using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CubeModel : ScriptableObject {

    public string cubeName;
    public int cost;
    public string description;
    public bool bought; 

    public Material material;
}
