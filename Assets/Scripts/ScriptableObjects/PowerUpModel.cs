using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "New PowerUp", menuName = "My/PowerUp")]
public class PowerUpModel : ScriptableObject
{
    public Sprite sprite;
    public string powerUpName;
    public string description;
    public int cost;

    public int level;
    public int maxLevel;
}
