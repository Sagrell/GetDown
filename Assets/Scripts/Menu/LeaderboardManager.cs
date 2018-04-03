using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeaderboardManager : MonoBehaviour {
    public Transform content;
    public GameObject leaderPrefab;
    User[] users;
	// Use this for initialization
	void Start () {
        users = GooglePlayManager.Instance.GetAllUsers();

        for (int i = 0; i < users.Length; i++)
        {
            Leader leader = Instantiate(leaderPrefab, content).GetComponent<Leader>();
            leader.user = users[i];
            leader.Initialize();
        }
	}

}
