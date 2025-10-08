using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Unity.Services.Authentication;
using Unity.Services.Core;
using Unity.Services.Leaderboards;

public class LeaderboardsManager : MonoBehaviour
{
    // Create a leaderboard with this ID in the Unity Dashboard
    const string LeaderboardId = GameConstants.LEADERBOARD_ID;
    private static LeaderboardsManager instance;

    public bool isAuthenticated = false;
	public bool isAuthenticating = false;
    string VersionId { get; set; }
    int Offset { get; set; }
    int Limit { get; set; }
    int RangeLimit { get; set; }
    List<string> FriendIds { get; set; }

    async void Awake()
    {
        Debug.Log("AWAKING LEADERBOARDS....");

        if (instance != null)
        {
            Debug.Log("There is another instance of social api running");
            //DestroyImmediate(gameObject);
        }

		instance = this;

        await UnityServices.InitializeAsync();

        await SignInAnonymously();
    }
    
    public async Task AuthenticateAndReport(int score, string leaderboardId)
	{

		if (isAuthenticated)
		{
			Debug.Log("Authentication already done, reporting...");
			AddScore(score, leaderboardId);
		}
		else
		{
			//need to login first
			await SignInAnonymously();

			if (isAuthenticated)
			{
				Debug.Log("Authentication successful");
				AddScore(score, leaderboardId);
			}
			else
			{
				Debug.Log("Authentication failed");
			}

		
		}

	}

    public static LeaderboardsManager Instance
    {

        get
        {
            if (instance == null)
            {
                GameObject scripts = GameObject.FindGameObjectWithTag("Scripts");
                if (scripts != null)
                {
                    instance = scripts.GetComponent<LeaderboardsManager>();
                }
                if (instance == null)
                {
                    instance = (LeaderboardsManager)FindObjectOfType(typeof(LeaderboardsManager));
                    if (instance == null)
                    {
                        //final chance, just create a new object
                        instance = new GameObject("LeaderboardsManager").AddComponent<LeaderboardsManager>();
                    }

                }


            }
            return instance;
        }
    }

    async Task SignInAnonymously()
    {
        Debug.Log("SIGNIN IN....");
        isAuthenticating = true;
        AuthenticationService.Instance.SignedIn += () =>
        {
            Debug.Log("Signed in as: " + AuthenticationService.Instance.PlayerId);
            isAuthenticated = true;
        };
        AuthenticationService.Instance.SignInFailed += s =>
        {
            // Take some action here...
            Debug.Log("SignIn Failed error: " + s);
            isAuthenticated = false;
            isAuthenticating = false;
        };

        await AuthenticationService.Instance.SignInAnonymouslyAsync();
    }

    public async void AddScore(int score, string leaderboard)
    {
        var scoreResponse = await LeaderboardsService.Instance.AddPlayerScoreAsync(leaderboard!=null ? leaderboard : LeaderboardId, score);
        Debug.Log(JsonConvert.SerializeObject(scoreResponse));
    }

    public async void GetScores()
    {
        var scoresResponse =
            await LeaderboardsService.Instance.GetScoresAsync(LeaderboardId);
        Debug.Log(JsonConvert.SerializeObject(scoresResponse));
    }

    public async void GetPaginatedScores()
    {
        Offset = 10;
        Limit = 10;
        var scoresResponse =
            await LeaderboardsService.Instance.GetScoresAsync(LeaderboardId, new GetScoresOptions{Offset = Offset, Limit = Limit});
        Debug.Log(JsonConvert.SerializeObject(scoresResponse));
    }

    public async void GetPlayerScore()
    {
        var scoreResponse =
            await LeaderboardsService.Instance.GetPlayerScoreAsync(LeaderboardId);
        Debug.Log(JsonConvert.SerializeObject(scoreResponse));
    }

    public async void GetVersionScores()
    {
        var versionScoresResponse =
            await LeaderboardsService.Instance.GetVersionScoresAsync(LeaderboardId, VersionId);
    Debug.Log(JsonConvert.SerializeObject(versionScoresResponse));
    }
}

