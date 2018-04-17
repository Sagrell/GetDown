using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.UI;

public class Leader : MonoBehaviour {
    public Text userName;
    public Image userImg;
    public Text score;
    public Text place;

    public Color first;
    public Color second;
    public Color third;
    public Color simple;

   [HideInInspector]
    public User user;
	// Use this for initialization
	public void Initialize () {
        userName.text = user.userName;
        Debug.Log("score -"+score.text +" : "+user.score);
        score.text = user.score.ToString();
        place.text = user.place.ToString();
        switch(user.place)
        {
            case 1:
                place.color = first;
                score.color = first;
                break;
            case 2:
                place.color = second;
                score.color = second;
                break;
            case 3:
                place.color = third;
                score.color = third;
                break;
            default:
                place.color = simple;
                score.color = simple;
                break;
        }
        if (user.userImg)
            userImg.sprite = Sprite.Create(user.userImg, new Rect(0.0f, 0.0f, user.userImg.width, user.userImg.height), new Vector2(0.5f, 0.5f));
    }
}
