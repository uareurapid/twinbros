using System.Runtime.InteropServices;
using UnityEngine;
using System.Collections;
using UnityEngine.SocialPlatforms;
#if UNITY_ANDROID && !UNITY_EDITOR
using GooglePlayGames;
#endif

/**
 * Implement Game Center
 * */
public class SocialAPI : MonoBehaviour {

	//[DllImport("__Internal")]
	//private static extern void _ReportAchievement( string achievementID, float progress );

	public bool isAuthenticated = false;
	public bool isAuthenticating = false;
	public bool gameCenterAvailable = false;

	private static SocialAPI instance;

	//this is on achievements script
	private IAchievement[] gameAchievements;
	private IScore[] gameScores;

	private Hashtable ANDROID_DICTIONARY = new Hashtable();
	
	//achievement ids

	private bool loadingGame = false;
	
	private const string PREVIOUS_ACTION_ADD_ACHIEVEMENT = "ADD_ACHIEVEMENT";
	private const string PREVIOUS_ACTION_AUTHENTICATION = "AUTHENTICATION";
	private const string PREVIOUS_ACTION_LOAD_ACHIEVEMENTS = "LOAD_ACHIEVEMENTS";
	
	private string previousAction = null;

	/*
   <?xml version="1.0" encoding="utf-8"?>
<!--
Google Play game services IDs.
Save this file as res/values/games-ids.xml in your project.
-->
<resources>
  <!-- app_id -->
  <string name="app_id" translatable="false">1063742711890</string>
  <!-- package_name -->
  <string name="package_name" translatable="false">com.crackedegggames.twins</string>
  <!-- achievement Cleared Stage 1 -->
  <string name="achievement_cleared_stage_1" translatable="false">CgkI0tCG4PoeEAIQAg</string>
  <!-- achievement Cleared Stage 2 -->
  <string name="achievement_cleared_stage_2" translatable="false">CgkI0tCG4PoeEAIQAw</string>
  <!-- achievement Cleared Stage 3 -->
  <string name="achievement_cleared_stage_3" translatable="false">CgkI0tCG4PoeEAIQBA</string>
  <!-- achievement Cleared Stage 4 -->
  <string name="achievement_cleared_stage_4" translatable="false">CgkI0tCG4PoeEAIQBQ</string>
  <!-- achievement Cleared Stage 5 -->
  <string name="achievement_cleared_stage_5" translatable="false">CgkI0tCG4PoeEAIQBg</string>
  <!-- leaderboard High Scores -->
  <string name="leaderboard_high_scores" translatable="false">CgkI0tCG4PoeEAIQAQ</string>
</resources>

	*/


	// Use this for initialization
	void Start () {

	     gameCenterAvailable = (Application.platform == RuntimePlatform.IPhonePlayer || Application.platform == RuntimePlatform.Android);

#if UNITY_ANDROID && !UNITY_EDITOR


		//achievements
		ANDROID_DICTIONARY.Add(GameConstants.ACHIEVEMENT_STAGE_1_ID, "CgkI0tCG4PoeEAIQAg");
		ANDROID_DICTIONARY.Add(GameConstants.ACHIEVEMENT_STAGE_2_ID, "CgkI0tCG4PoeEAIQAw");
		ANDROID_DICTIONARY.Add(GameConstants.ACHIEVEMENT_STAGE_3_ID, "CgkI0tCG4PoeEAIQBA");
		ANDROID_DICTIONARY.Add(GameConstants.ACHIEVEMENT_STAGE_4_ID, "CgkI0tCG4PoeEAIQBQ");
		ANDROID_DICTIONARY.Add(GameConstants.ACHIEVEMENT_STAGE_5_ID, "CgkI0tCG4PoeEAIQBg");

		//leaderboard
		ANDROID_DICTIONARY.Add(GameConstants.LEADERBOARD_ID, "CgkI0tCG4PoeEAIQAQ");

	     if(gameCenterAvailable) {
			// Activate the Google Play Games platform
	        // recommended for debugging:
            PlayGamesPlatform.DebugLogEnabled = true;
            // Activate the Google Play Games platform
            PlayGamesPlatform.Activate();
	     }

#endif

		isAuthenticating = false;
		isAuthenticated = gameCenterAvailable ? Social.localUser.authenticated : false;
		loadingGame = false;
		

		// Authenticate and register a ProcessAuthentication callback
		// This call needs to be made before we can proceed to other calls in the Social API
		if(!gameCenterAvailable) {
			
		  Debug.Log("DEBUG: Game center not available for this platform!");
		}
		else if(!isAuthenticated) {
			Debug.Log("DEBUG: calling game center authentication..");

			Social.localUser.Authenticate(ProcessAuthentication);
		}
		else {
			//already authenticated
			Debug.Log("DEBUG: already authenticated..");
			Social.LoadAchievements (ProcessLoadAchievements);
		}
	}
	
	void Awake() {

		// Register the singleton
		if(instance!=null) {
			Debug.Log("There is another instance of social api running");
			//DestroyImmediate(gameObject);
		}

		instance = this;
		//DontDestroyOnLoad(gameObject);
	}

	//dummy method
	public void LoadMe() {
	  Debug.Log("Social API Loaded");
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public static SocialAPI Instance {
		
		get
		{
			if (instance == null)
			{
				GameObject scripts = GameObject.FindGameObjectWithTag("Scripts");
				if(scripts!=null) {
				 instance = scripts.GetComponent<SocialAPI>();
				}
				if(instance==null) {
					instance = (SocialAPI)FindObjectOfType(typeof(SocialAPI));
				    if (instance == null) {
				    	//final chance, just create a new object
						instance = new GameObject("SocialAPI").AddComponent<SocialAPI>();
				    }
				      
				}

				
			}
			return instance;
		}
	}
	
	// This function gets called when Authenticate completes
	// Note that if the operation is successful, Social.localUser will contain data from the server. 
	public void ProcessAuthentication (bool success) {

		if (success) {

			Debug.Log ("DEBUG: Authentication successful");
			isAuthenticated = true;
			// Request loaded achievements, and register a callback for processing them
			Social.LoadAchievements (ProcessLoadAchievements);
			//Social.LoadAchievementDescriptions(ProcessLoadAchievementsDescriptions);
			//LoadScores();
		}
		else {
			Debug.Log ("DEBUG: Failed to authenticate");
			isAuthenticated = false;
		}
			
	}

	//do all in one shot
	//TODO, do similar method to load the scores
	public void AuthenticateAndReport(int score, string leaderboardId) {

	    if(isAuthenticated) {
			Debug.Log ("Authentication already done, reporting...");
			ReportScore(score,leaderboardId);
	    }
	    else {
	            //need to login first
				Social.localUser.Authenticate (success => {

				    if (success) {
				        Debug.Log ("Authentication successful");
				        string userInfo = "Username: " + Social.localUser.userName + 
				            "\nUser ID: " + Social.localUser.id + 
				            "\nIsUnderage: " + Social.localUser.underage;
				        Debug.Log (userInfo);

				        ReportScore(score,leaderboardId);
				    }
				    else {
						Debug.Log ("Authentication failed");
				    }
			        
				});
	    }
		
	}
	
	public IAchievement[] GetGameAchievements() {
		if(gameAchievements==null && isAuthenticated) {
			Social.LoadAchievements (ProcessLoadAchievements);
	    }
	  
	    return gameAchievements;
	}
	
	public IScore[] GetGameScores(string leaderboardId) {
		if(gameScores==null && isAuthenticated) {
			LoadScores(leaderboardId);
		}
		
		return gameScores;
	}
		

	public void ProcessLoadAchievements(IAchievement[] achievements) {
	
		gameAchievements = achievements;
		Debug.Log("adding achievements...");
		
		if (achievements.Length > 0) {

						Debug.Log (" Got " + achievements.Length + " achievements");
						string myAchievements = "My achievements:\n";
						foreach (IAchievement achievement in achievements) {
								myAchievements += "\t" +
										achievement.id + " " +
										achievement.percentCompleted + " " +
										achievement.completed + " " +
										achievement.lastReportedDate;
						}
						Debug.Log (myAchievements);
			
		} 
		else {
			Debug.Log("No achievements found!!");
		}

	}
	
	private void SetPreviousActionAddAchievement() {
	  previousAction = PREVIOUS_ACTION_ADD_ACHIEVEMENT;
	}

	//overload of above method
	public void AddAchievement(string id) {

		//already authenticated, just report progress
		if(isAuthenticated) {
			Debug.Log ("Authentication already done, reporting achievement: " + id);
			AddAchievement(id,100f);
	    }
	    else {
	            //need to login first
				Social.localUser.Authenticate (success => {

				    if (success) {
				      isAuthenticated=true;
					  AddAchievement(id,100f);
				    }
					else {
						Debug.Log ("Authentication failed, unable to report achievement");
				    }
			        
				});
		}
	  
	}
	/**
	*
	*/
	public void AddAchievement(string id, float percentageCompleted) {
		//TODO CHECK NAMES

#if UNITY_ANDROID && !UNITY_EDITOR
		    string achieveId = ANDROID_DICTIONARY[id] as string;

		    Social.ReportProgress(achieveId, 100.0f,result => {
      		    // handle success or failure
			    if (result)
				    Debug.Log ("Successfully reported progress of: " + achieveId);
		  	    else
		  		    Debug.Log ("Failed to report progress of: " + achieveId);
    	    });

#endif

#if UNITY_IPHONE || UNITY_IOS && !UNITY_EDITOR
		IAchievement achievement =  Social.CreateAchievement();
		achievement.id = id;
		achievement.percentCompleted = percentageCompleted;
	    achievement.ReportProgress( result => {
	  	if (result)
	  		Debug.Log ("Successfully reported progress");
	  	else
	  		Debug.Log ("Failed to report progress");
	    });
#endif


	}

	public void CreateAchievementResult(bool success) {
		if (success)
			Debug.Log ("Successfully reported progress");
		else
			Debug.Log ("Failed to report progress");
	}

	
	public void LoadScores(string leaderBoardID) {

#if UNITY_ANDROID && !UNITY_EDITOR
		    leaderBoardID = ANDROID_DICTIONARY[leaderBoardID] as string;
#endif
		Social.LoadScores(leaderBoardID,LoadScoresCallback);
	}
	
	public void LoadScoresCallback(IScore[] scores) {
	
		if (scores.Length > 0) {
			Debug.Log ("Got " + scores.Length + " scores");
			string myScores = "Leaderboard:\n";
			foreach (IScore score in scores)
				myScores += "\t" + score.userID + " " + score.formattedValue + " " + score.date + "\n";
			Debug.Log (myScores);
			gameScores = scores;
		}
		else
			Debug.Log ("No scores loaded!");
	
	}
	

	
	void OnGUI() {
	

	}

	
	//search for a given achievement;
	private bool foundAchievement(string ach) {
	
		bool found = false;
		foreach(IAchievement achievement in gameAchievements) {
		
			if(achievement.id.Equals(ach)) {
				return true;
			}
			
		}
		return found;
	}

	public void ReportScore (long score, string leaderboardID) {

#if UNITY_ANDROID && !UNITY_EDITOR
		    leaderboardID = ANDROID_DICTIONARY[leaderboardID] as string;
#endif
		Debug.Log ("Reporting score " + score + " on leaderboard " + leaderboardID);
		Social.ReportScore (score, leaderboardID, ReportScoreCallback);
	}
	
	void ReportScoreCallback(bool success) {
		Debug.Log(success ? "Reported score successfully" : "Failed to report score");
	}

	public void AuthenticateAndShowLeaderboards() {
		if(isAuthenticated) {
	      ShowLeaderBoards();
		}
		else {
			//need to login first
				Social.localUser.Authenticate (success => {

				    if (success) {
				        isAuthenticated=true;
				        Debug.Log ("Authentication successful");
				        string userInfo = "Username: " + Social.localUser.userName + 
				            "\nUser ID: " + Social.localUser.id + 
				            "\nIsUnderage: " + Social.localUser.underage;
				        Debug.Log (userInfo);

				        ShowLeaderBoards();
				    }
				    else {
						Debug.Log ("Authentication failed");
				    }
			        
				});
		}
	}

	public void AuthenticateAndShowAchievements() {
		if(isAuthenticated) {
			ShowAchievements();
		}
		else {
			//need to login first
				Social.localUser.Authenticate (success => {

				    if (success) {
				        Debug.Log ("Authentication successful");
				        string userInfo = "Username: " + Social.localUser.userName + 
				            "\nUser ID: " + Social.localUser.id + 
				            "\nIsUnderage: " + Social.localUser.underage;
				        Debug.Log (userInfo);

				        ShowAchievements();
				    }
				    else {
						Debug.Log ("Authentication failed");
				    }
			        
				});
		}
	}

	public void ShowLeaderBoards() {

#if UNITY_ANDROID && !UNITY_EDITOR
	//string toShow = ANDROID_DICTIONARY[GameConstants.LEADERBOARD_ID] as string;
	((PlayGamesPlatform) Social.Active).ShowLeaderboardUI();
#endif

#if UNITY_IPHONE && !UNITY_EDITOR
		Social.ShowLeaderboardUI();
#endif

	}

	public void ShowAchievements() {
		Social.ShowAchievementsUI();
	}

}
