using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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
    GooglePlayManager GPS;


    public void ShowLeaders()
    {
        friendsButton.interactable = true;
        allButton.interactable = false;
        all.SetActive(true);
        friends.SetActive(false);
        GPS = GooglePlayManager.Instance;
        Leader[] leaders = content.GetComponentsInChildren<Leader>();
        foreach (Leader leader in leaders)
        {
            Destroy(leader.gameObject);
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
        Leader[] leaders = friendsContent.GetComponentsInChildren<Leader>();
        foreach (Leader leader in leaders)
        {
            Destroy(leader.gameObject);
        }
        StopAllCoroutines();
        StartCoroutine(LoadFriends());
    }
    IEnumerator LoadAllUsers()
    {
        GPS.LoadAllUsers();
        loading.SetActive(true);
        while (!GPS.IsUsersLoaded())
        {
            yield return new WaitForSecondsRealtime(.1f);
        }
        loading.SetActive(false);
        users = GPS.GetAllUsers();
        for (int i = 0; i < users.Length; i++)
        {
            Leader leader = Instantiate(leaderPrefab, content).GetComponent<Leader>();
            leader.user = users[i];
            leader.Initialize();
        }
    }
    IEnumerator LoadFriends()
    {
        GPS.LoadAllUsers();
        loading.SetActive(true);
        while (!GPS.IsUsersLoaded())
        {
            yield return new WaitForSecondsRealtime(.1f);
        }
        loading.SetActive(false);
        users = GPS.GetFriends();
        for (int i = 0; i < users.Length; i++)
        {
            Leader leader = Instantiate(leaderPrefab, friendsContent).GetComponent<Leader>();
            leader.user = users[i];
            leader.Initialize();
        }
    }
}
