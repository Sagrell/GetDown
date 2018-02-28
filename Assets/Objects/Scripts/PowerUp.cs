using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "New PowerUp", menuName = "My/PowerUp")]
public class PowerUp : ScriptableObject
{
    public Sprite sprite;
    public string powerUpName;
    public string description;
    public int cost;

    [Range(0,5)]
    public int level;
}
