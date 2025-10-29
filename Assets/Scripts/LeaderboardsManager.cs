using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Unity.Services.Authentication;
using Unity.Services.Core;
using Unity.Services.Leaderboards;
using Unity.Services.Leaderboards.Models;
using System.Linq;
using UnityEngine.UI;
using System.Diagnostics.Tracing;
using System;
using System.Dynamic;
using UnityEngine.SceneManagement;

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

    string playerID = "player1";
    List<string> FriendIds { get; set; }

    List<LeaderboardEntry> entries;

    public List<GameObject> top3;

    const int FIRST_PLACE = 0;
    const int SECOND_PLACE = 1;
    const int THIRD_PLACE = 2;
    const int YOU_PLACE = 3;

    async void Awake()
    {
        Debug.Log("AWAKING LEADERBOARDS....");

        if (entries == null)
        {
            entries = new List<LeaderboardEntry>();
        } 
        
        if(top3 == null)
        {
            top3 = new List<GameObject>();
        }

        if (instance != null)
        {
            Debug.Log("There is another instance of LeaderboardsManager running");
            //DestroyImmediate(gameObject);
        }

        instance = this;

        Debug.Log("STILL HERE:");

        await UnityServices.InitializeAsync();

        Debug.Log("STILL HERE 2:");

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
        
        isAuthenticating = true;

        if (!AuthenticationService.Instance.IsSignedIn && !isAuthenticated)
        {
        
            Debug.Log("SIGNIN IN....");
            AuthenticationService.Instance.SignedIn += () =>
            {
                isAuthenticated = true;
                isAuthenticating = false;
                playerID = AuthenticationService.Instance.PlayerId;
                Debug.Log("Signed in as: " + playerID);
                GetScores();
            };
            AuthenticationService.Instance.SignInFailed += s =>
            {
                // Take some action here...
                Debug.Log("SignIn Failed error: " + s);
                if (s.Message.Contains("player is already signing in"))
                {
                    isAuthenticated = true;
                }
                else
                {
                    isAuthenticated = false;
                }

                isAuthenticating = false;

            };
            
            await AuthenticationService.Instance.SignInAnonymouslyAsync();
            

        } else
        {
            Debug.Log("ALREADY SIGNED IN");
            isAuthenticated = true;
            isAuthenticating = false;
            playerID = AuthenticationService.Instance.PlayerId;
            Debug.Log("Signed in as: " + playerID);
            GetScores();
        }


    }
    
    // void OnApplicationQuit()
    // {
    // sent to all components, on quit, even if disabled
    // }

    public async void AddScore(int score, string leaderboard)
    {
        var scoreResponse = await LeaderboardsService.Instance.AddPlayerScoreAsync(leaderboard!=null ? leaderboard : LeaderboardId, score);
        Debug.Log(JsonConvert.SerializeObject(scoreResponse));
    }

    public async void GetScores()
    {
        if (SceneManager.GetActiveScene().name != "Leaderboards")
        {
            Debug.Log("Ignore parsing entries if scene is not Leaderboards one!");
            return;
        }
        if (isAuthenticated || AuthenticationService.Instance.IsSignedIn)
        {
            LeaderboardScoresPage scoresResponse = await LeaderboardsService.Instance.GetScoresAsync(LeaderboardId);
            Debug.Log("GetScores() response" + JsonConvert.SerializeObject(scoresResponse));

            List<LeaderboardEntry> results = scoresResponse.Results;
            Debug.Log("Results size: " + results.Count);
            // entries.Clear();
            entries = results;
            Debug.Log("Entries size: " + entries.Count);
            await ParseEntries();
        }
        else
        {
            await SignInAnonymously();
        }

    }
    /**
    {
    "limit": 10,
    "total": 7,
    "results": [
        {
        "playerId": "CLBn7z9N9Y8VMHvLWuHsQN6O0WqR",
        "playerName": "BreezyGlitteringCabinet#8",
        "rank": 0,
        "score": 3900.0
        },
        {
        "playerId": "IzI9gKDVzLqhk1crSAR0i0Om7TuE",
        "playerName": "SpryDevotedBedbug#9",
        "rank": 1,
        "score": 3600.0
        },
        {
        "playerId": "96PDOoNCOABnsmwNmzVvdqf7UL0m",
        "playerName": "AcidicInvolvedGear#2",
        "rank": 2,
        "score": 2500.0
        },
        {
        "playerId": "bXXkZP63TCUI5CA4BYjcoakKjZPf",
        "playerName": "ImpeccableRoastedFinch#5",
        "rank": 3,
        "score": 2400.0
        },
        {
        "playerId": "R6ZSoWHBAfQFJ9NgCWcWHsYP6v29",
        "playerName": "AdventurousWinningCantaloupe#7",
        "rank": 4,
        "score": 2400.0
        },
        {
        "playerId": "ZbFkUdcbyqMIUjqaywwiUpXGPzA4",
        "playerName": "FluffyDroppedDurian#1",
        "rank": 5,
        "score": 1200.0
        },
        {
        "playerId": "335r9gUqnSp0e0VW8ZyIvvgrwEXi",
        "playerName": "PessimisticBlushingSoup#1",
        "rank": 6,
        "score": 800.0
        }
    ]
    }
    */
    
    private async Task ParseEntries()
    {

        bool foundPlayer = false;
        double playerScore = 0;
        int playerScoreLocal = PlayerPrefs.GetInt(GameConstants.LEADERBOARD_ID, 0);
  
        for (int i = 0; i < entries.Count; i++)
        {
            LeaderboardEntry entry = entries[i];
            Debug.Log("PlayerID: " + playerID);
            Debug.Log("Entry. PlayerID: " + entry.PlayerId);

            if (i == FIRST_PLACE)
            {
                // 1st
                Text textField = top3[FIRST_PLACE].GetComponent<Text>();
                textField.text = "1st => " + entry.Score + " pts";
            }
            else if (i == SECOND_PLACE)
            {
                // 2nd
                Text textField = top3[SECOND_PLACE].GetComponent<Text>();
                textField.text = "2nd => " + entry.Score + " pts";
            }
            else if (i == THIRD_PLACE)
            {
                // 3
                Text textField = top3[THIRD_PLACE].GetComponent<Text>();
                textField.text = "3rd => " + entry.Score + " pts";
            }

            if (entry.PlayerId == playerID)
            {
                Text textField = top3[YOU_PLACE].GetComponent<Text>();
                textField.text = "you => " + entry.Score + " pts";
                foundPlayer = true;
                playerScore = entry.Score;
            }

            if (i > top3.Count) break; // for now we only show top 3 + player

        }
        if (!foundPlayer)
        {
            int currentHighScore = playerScoreLocal;
            Text textField = top3[YOU_PLACE].GetComponent<Text>();
            textField.text = "you => " + currentHighScore + " pts";
        }
        else
        {
            playerScore = Math.Max(playerScore, playerScoreLocal);
            Text textField = top3[YOU_PLACE].GetComponent<Text>();
            textField.text = "you => " + playerScore + " pts";
        }
        
        // if player is among the top 3, blink the score!
        if(entries.Count >= 3)
        {
            Text playerPosition = null;
            if(entries[FIRST_PLACE].Score == (double) playerScore)
            {
                playerPosition = top3[FIRST_PLACE].GetComponent<Text>();

            } else if(entries[SECOND_PLACE].Score == (double) playerScore)
            {
                playerPosition = top3[SECOND_PLACE].GetComponent<Text>();

            }if (entries[THIRD_PLACE].Score == (double)playerScore)
            {
                playerPosition = top3[THIRD_PLACE].GetComponent<Text>();
            }
            if(playerPosition!=null)
            {
                playerPosition.GetComponent<EnableDisableMonobehaviour>().enabled = true;
            }
        }
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
    
    public void CloseCurrentScene()
    {
        SoundEffectsHelper.Instance.PlayReplaySound();
		Debug.Log("Leaderboards CloseCurrentScene()");
		SceneSwitcher.UnLoadCurrentSceneFromTop();
    }
}

