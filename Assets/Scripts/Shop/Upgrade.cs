using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Upgrade : MonoBehaviour {

    public PowerUpModel powerUp;

    public Image image;
    public Text powerUpName;
    public Text description;
    public Text cost;

    int level;

    // Use this for initialization
    void Start () {
        image.sprite = powerUp.sprite;
        powerUpName.text = powerUp.powerUpName;
        description.text = powerUp.description;
        cost.text = powerUp.cost.ToString();
        level = powerUp.level;
    }
	
}
