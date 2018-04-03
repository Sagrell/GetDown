using UnityEngine;
using GooglePlayGames;
using UnityEngine.SocialPlatforms;
public class GooglePlayManager : MonoBehaviour {

    #region Singleton
    public static GooglePlayManager Instance;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            PlayGamesPlatform.Activate();
            Social.localUser.Authenticate((success) => {
                if (success)
                {
                    Debug.Log("SUCCESS");
                    leaderboard = Social.CreateLeaderboard();
                    Debug.Log("leaderboard: " + leaderboard.id);
                }
                else
                {
                    Debug.Log("FAILED");
                }
            });
            
            DontDestroyOnLoad(gameObject);
        } else
        {
            Destroy(gameObject);
        }
    }
    #endregion

    string leaderboardId = "CgkIuYTB-6YdEAIQAA";
    ILeaderboard leaderboard;
    // Use this for initialization
    void Start () {
        
    }
	
    public void SetNewRecord(int newScore)
    {
        Social.ReportScore(newScore, leaderboardId, (success) => {
            Debug.Log("ScoreSaved");
        });
    }
    public User[] GetAllUsers()
    {
        IScore[] scores = leaderboard.scores;    
        User[] users = new User[scores.Length];
        string[] userIds = new string[scores.Length];
        for (int i = 0; i < scores.Length; i++)
        {
            userIds[i] = scores[i].userID;
        }
        Social.LoadUsers(userIds, (GPSUsers)=> {
            for (int i = 0; i < users.Length; i++)
            {
                IUserProfile userProfile = GPSUsers[i];

                users[i] = new User() {
                    userID = userProfile.id,
                    score = int.Parse(scores[i].formattedValue),
                    place = i + 1,
                    userImg = userProfile.image
                };

            }
        });
        Debug.Log("USERS");
        return users;
    }
    public User[] GetFriends()
    {
        IScore[] scores = leaderboard.scores;
        User[] users = new User[scores.Length];
        string[] userIds = new string[scores.Length];
        for (int i = 0; i < scores.Length; i++)
        {
            userIds[i] = scores[i].userID;
        }
        Social.LoadUsers(userIds, (GPSUsers) => {
            for (int i = 0; i < users.Length; i++)
            {
                IUserProfile userProfile = GPSUsers[i];
                if(userProfile.isFriend)
                {
                    users[i] = new User()
                    {
                        userID = userProfile.id,
                        score = int.Parse(scores[i].formattedValue),
                        place = i + 1,
                        userImg = userProfile.image
                    };
                }
            }
        });
        return users;
    }

}
