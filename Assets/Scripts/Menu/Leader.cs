using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Leader : MonoBehaviour {
    public Text userName;
    public Image userImg;
    public Text score;
    public Text place;

    [HideInInspector]
    public User user;
	// Use this for initialization
	public void Initialize () {
        userName.text = user.userName;
        score.text = user.score.ToString();
        place.text = user.place.ToString();
        if (user.userImg)
            userImg.sprite = Sprite.Create(user.userImg, new Rect(0.0f, 0.0f, user.userImg.width, user.userImg.height), new Vector2(0.5f, 0.5f));
    }
}
