using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeaderboardManager : MonoBehaviour {
    public Transform content;
    public GameObject leaderPrefab;
    public GameObject loading;
    User[] users;
    GooglePlayManager GPS;

    // Use this for initialization
    void Start () {
        
	}

    public void ShowLeaders()
    {
        GPS = GooglePlayManager.Instance;
        Leader[] leaders = GetComponentsInChildren<Leader>();
        foreach (Leader leader in leaders)
        {
            Destroy(leader.gameObject);
        }
        StopAllCoroutines();
        StartCoroutine(WaitForLeaderboard());
    }

    IEnumerator WaitForLeaderboard()
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
}
