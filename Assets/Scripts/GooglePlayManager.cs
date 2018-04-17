using UnityEngine;
using GooglePlayGames;
using UnityEngine.SocialPlatforms;
using System.Collections;
using System.Collections.Generic;

public class GooglePlayManager : MonoBehaviour {
    public static GooglePlayManager Instance;

    //Start the game
    public static string beginnerAchieve = "CgkIuYTB-6YdEAIQAg";

    //Reach 50
    public static string firstJumpsAchieve = "CgkIuYTB-6YdEAIQAw";
    //Reach 100 score
    public static string ugbprofessionalAchieve = "CgkIuYTB-6YdEAIQBA";
    //Reach 200 score
    public static string murProfessionalAchieve = "CgkIuYTB-6YdEAIQCg";
    //Reach 500 score
    public static string urdProfessionalAchieve = "CgkIuYTB-6YdEAIQCw";
    //Reach 1000 score
    public static string impossibleAchieve = "CgkIuYTB-6YdEAIQDA";

    //Collect 50 coins
    public static string trtWealthAchieve = "CgkIuYTB-6YdEAIQBQ";
    //Collect 200 coins
    public static string otrtWealthAchieve = "CgkIuYTB-6YdEAIQDg";
    //Collect 1000 coins
    public static string richGuyAchieve = "CgkIuYTB-6YdEAIQDw";
    //Collect 2000 coins
    public static string cugmSomeMoneyAchieve = "CgkIuYTB-6YdEAIQEA";
    //Collect 10000 coins
    public static string whyAchieve = "CgkIuYTB-6YdEAIQEQ";
    
    //Buy shield
    public static string shieldAchieve = "CgkIuYTB-6YdEAIQBg";
    //Buy fast run
    public static string fastRunAchieve = "CgkIuYTB-6YdEAIQBw";
    //Buy double coins
    public static string doubleCoinsAchieve = "CgkIuYTB-6YdEAIQCA";
    //Buy magnet
    public static string magnetAchieve = "CgkIuYTB-6YdEAIQCQ";


    //Found diamond
    public static string foundAchive = "CgkIuYTB-6YdEAIQDQ";



    string leaderboardId = "CgkIuYTB-6YdEAIQAA";
    Dictionary<string, IAchievement> achievements;
    User[] users;
    User[] friends;
    bool isAchievesLoaded;
    bool isUsersLoaded;
    bool isScoreLoaded;
    bool isLogIn;
    private void Awake()
    {
        if (Instance == null)
        {
            isScoreLoaded = false;
            isAchievesLoaded = false;
            Instance = this;
            PlayGamesPlatform.Activate();
            LogIn();
            DontDestroyOnLoad(gameObject);
        } else
        {
            Destroy(gameObject);
        }
    } 
   
    // Use this for initialization
    void Start () {
        
    }
	public void LogIn()
    {
        Social.localUser.Authenticate((success) => {
            if (success)
            {
                Social.LoadAchievements((achieves) => {
                    achievements = new Dictionary<string, IAchievement>();
                    for (int i = 0; i < achieves.Length; i++)
                    {
                        achievements.Add(achieves[i].id, achieves[i]);
                    }
                    isAchievesLoaded = true;
                });
                isLogIn = true;
                PlusControlller.Instance.SignInStateGoogle();
            }
            else
            {
                isLogIn = false;
                PlusControlller.Instance.SignOutStateGoogle();
            }
        });
    }
    public bool IsLogIn()
    {
        return isLogIn;
    }
    public void LogOut()
    {
        PlayGamesPlatform.Instance.SignOut();
        isLogIn = false;
        PlusControlller.Instance.SignOutStateGoogle();
    }
    public bool IsUsersLoaded()
    {
        return isUsersLoaded;
    }
    public void SetNewRecord(int newScore)
    {
        Social.ReportScore(newScore, leaderboardId, (success) => {
        });
        
    }
    public User[] GetAllUsers()
    {
        return users;
    }
    public void LoadAllUsers()
    {
        StartCoroutine(WaitForScoreLoading());
    }
    public void Achieve(string id)
    {
        if(isAchievesLoaded)
        {
            IAchievement achieve = achievements[id];
            if (!achieve.completed)
            {
                AudioCenter.Instance.PlaySound("Achieve");
                Social.ReportProgress(id, 100.0, (success) => {
                    if (success)
                    {
                        isAchievesLoaded = false;
                        Social.LoadAchievements((achieves) => {
                            achievements = new Dictionary<string, IAchievement>();
                            for (int i = 0; i < achieves.Length; i++)
                            {
                                achievements.Add(achieves[i].id, achieves[i]);
                            }
                            isAchievesLoaded = true;
                        });
                    }
                });
            }
        }    
    }
    IEnumerator WaitForScoreLoading()
    {
        isUsersLoaded = false;
        int j = 0;
        IScore[] scores = null;
        isScoreLoaded = false;
        Social.LoadScores(leaderboardId, (result)=> {
            scores = result;
            isScoreLoaded = true;
        });
        
        while (!isScoreLoaded)
        {
            yield return new WaitForSecondsRealtime(.1f);
        }
        users = new User[scores.Length];
        friends = new User[scores.Length];
        string[] userIds = new string[scores.Length];
        for (int i = 0; i < scores.Length; i++)
        {
            userIds[i] = scores[i].userID;
        }
        Social.LoadUsers(userIds, (GPSUsers) => {
            for (int i = 0; i < users.Length; i++)
            {

                IUserProfile userProfile = GPSUsers[i];
                users[i] = new User()
                {
                    userName = userProfile.userName,
                    score = int.Parse(scores[i].formattedValue),
                    place = i + 1,
                    userImg = userProfile.image
                };
                if(userProfile.isFriend)
                {
                    friends[j++] = users[i];
                }

            }
            isUsersLoaded = true;
        });
    }
    public User[] GetFriends()
    {
        return friends;
    }

}
