using Facebook.Unity;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FacebookManager : MonoBehaviour {
    private void Awake()
    {
        if(!FB.IsInitialized)
            FB.Init(()=> 
            {
                if (FB.IsInitialized)
                    FB.ActivateApp();
                else
                    Debug.Log("Couldnt init FB");   
            },
            isGameShown =>
            {
                if(!isGameShown)
                {
                    Time.timeScale = 0f; 
                } else
                {
                    Time.timeScale = 1f;
                }
            });
        else
            FB.ActivateApp();
    }

    #region LogIn / LogOut
    public void LogIn()
    {
        if(!FB.IsLoggedIn)
        {
            List<string> permissions = new List<string>() { "public_profile", "email", "user_friends" };
            FB.LogInWithReadPermissions(permissions, (result) => {
                if(FB.IsLoggedIn)
                {
                    PlusControlller.Instance.SignInStateFacebook();
                }
            });
        }
    }
    public void LogOut()
    {
        if(FB.IsLoggedIn)
        {
            FB.LogOut();
            StartCoroutine(WaitForLogOut());
        }
    }
    IEnumerator WaitForLogOut()
    {
        float elapsedTime = 0f;
        while(FB.IsLoggedIn && elapsedTime < 10f)
        {
            elapsedTime += 0.1f;
            yield return new WaitForSecondsRealtime(.1f);
        }
        if(elapsedTime <10f)
        {
            PlusControlller.Instance.SignOutStateFacebook();
        }
    }
    #endregion
    public void Share ()
    {
        FB.ShareLink(new System.Uri("http://resocoder.com"), "Check it out!", "This is nice site!",
           new System.Uri("http://resocoder.com/wp-content/uploads/2017/01/logoRound512.png"));

    }
    #region Inviting
    public void GameRequest()
    {
        FB.AppRequest("Hey! Come and play this awesome game!", title: "Get Down");
    }

    public void Invite()
    {
        FB.Mobile.AppInvite(new System.Uri("https://google.com"));
    }
    #endregion
    public void GetFriendsPlayingThisGame()
    {
        string query = "/me/friends";
        FB.API(query, HttpMethod.GET, result =>
        {
            Dictionary<string, object> dictionary = (Dictionary<string, object>)Facebook.MiniJSON.Json.Deserialize(result.RawResult);
            List<object> friendsList = (List<object>)dictionary["data"];
        });
        GameRequest();
    }
}
