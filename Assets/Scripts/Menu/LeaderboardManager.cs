/*using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms;
using UnityEngine.UI;

public class LeaderboardManager : MonoBehaviour {
    public static LeaderboardManager Instance;

    private void Awake()
    {
        Instance = this;
    }
    public GameObject all;
    public GameObject friends;
    public Button allButton;
    public Button friendsButton;
    public Transform content;
    public Transform friendsContent;
    public GameObject leaderPrefab;
    public GameObject loading;
    User[] users;
    User[] usersFriends;
    IScore[] scores;
    GooglePlayManager GPS;


    public void ShowLeaders()
    {
        friendsButton.interactable = true;
        allButton.interactable = false;
        all.SetActive(true);
        friends.SetActive(false);
        GPS = GooglePlayManager.Instance;       
        if(!GPS.IsUsersLoaded())
        {
            Leader[] leaders = content.GetComponentsInChildren<Leader>();
            foreach (Leader leader in leaders)
            {
                Destroy(leader.gameObject);
            }
            StopAllCoroutines();
            StartCoroutine(LoadAllUsers());
        }
       
    }
    public void Refresh()
    {
        Leader[] leaders = content.GetComponentsInChildren<Leader>();
        foreach (Leader leader in leaders)
        {
            Destroy(leader.gameObject);
        }
        Leader[] friends = friendsContent.GetComponentsInChildren<Leader>();
        foreach (Leader friend in friends)
        {
            Destroy(friend.gameObject);
        }
        StopAllCoroutines();
        StartCoroutine(LoadAllUsers());
    }
    public void ShowFriends()
    {
        allButton.interactable = true;
        friendsButton.interactable = false;
        all.SetActive(false);
        friends.SetActive(true);
        GPS = GooglePlayManager.Instance;
        if (!GPS.IsUsersLoaded())
        {
            Leader[] leaders = friendsContent.GetComponentsInChildren<Leader>();
            foreach (Leader leader in leaders)
            {
                Destroy(leader.gameObject);
            }
            StopAllCoroutines();
            StartCoroutine(LoadAllUsers());
        }
    }
    public void OnUsersLoadComplete(IUserProfile[] result)
    {
        int j = 0;

        users = new User[result.Length];
        usersFriends = new User[result.Length];

        for (int i = 0; i < users.Length; i++)
        {

            IUserProfile userProfile = result[i];

            users[i] = new User()
            {
                userName = userProfile.userName,
                score = int.Parse(scores[i].formattedValue),
                place = i + 1,
                userImg = userProfile.image
            };
            if (userProfile.isFriend)
            {
                usersFriends[j++] = users[i];
            }

        }
    }
    public void OnScoresLoadComplete(IScore[] result)
    {
        scores = result;
        string[] userIds = new string[scores.Length];
        for (int i = 0; i < scores.Length; i++)
        {
            userIds[i] = scores[i].userID;
        }
    }
    IEnumerator LoadAllUsers()
    {
        GPS.LoadUsers();
        loading.SetActive(true);
        while (!GPS.IsUsersLoaded())
        {
            yield return new WaitForSecondsRealtime(.1f);
        }
        loading.SetActive(false);
        users = GPS.GetAllUsers();
        usersFriends = GPS.GetFriends();
        for (int i = 0; i < users.Length; i++)
        {
            Leader leader = Instantiate(leaderPrefab, content).GetComponent<Leader>();
            leader.user = usersFriends[i];
            leader.Initialize();
        }
        for (int i = 0; i < usersFriends.Length; i++)
        {
            Leader leader = Instantiate(leaderPrefab, friendsContent).GetComponent<Leader>();
            leader.user = usersFriends[i];
            leader.Initialize();
        }
    }
}
*/