using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour {

	public Level currentLevel;
	public Level respawnLevel; //in wich level to restart the game after dying?
	public Level currentStageFirstLevel;
	public Level debugLevel;

	private Level[] allLevels;
	//1 stage is a scene that has many levels
	public int stage = 1;
	public bool respawnOnDyingLevel = false; 
	//TODO IN-APP
	//if true will respawn on the same level that previously was
	//otherwise if respawnLevel !=null set it as the currentLevel

	private int numMoves = 10;
	public int MAX_MOVES = 10;
	public bool hasExtraMoves = false; //TODO IN-APP

	private GUIManager guiManager;
	private bool isDead = false;
	private bool isDying = false;

	public bool isDebugMode = false;

	private bool leftTwinMoved = false;
	private bool rightTwinMoved = false;

	private bool leftTwinReady = false;
	private bool rightTwinReady = false;
	// Use this for initialization

	public PlayerMovement [] twins;

	private bool gameStarted = false;

	private SwipeDetector swipe;

	public List<ResetBehaviourScript> listOfBehaviours;

	//when i loose
	public List<Destroyable> listOfDestroyables;

	private AudioSource music;

	private long lastMovementTime = 0;
	//keep a reference for this
	private GameObject scripts;

	public int currentScore = 0;
	public int highScore = 0;

	void Start () {

		if(CheckHasExtraMoves() || hasExtraMoves) {
			numMoves = MAX_MOVES + 2;
		}

		if(Application.platform == RuntimePlatform.IPhonePlayer || Application.platform == RuntimePlatform.Android && swipe==null) {
			swipe = gameObject.AddComponent<SwipeDetector>();
			swipe.detectSwipeOnlyAfterRelease = true;
		}

		currentStageFirstLevel = respawnLevel;

		listOfBehaviours = new List<ResetBehaviourScript>();
		listOfDestroyables = new List<Destroyable>();

		scripts = GameObject.FindGameObjectWithTag("Scripts");
		guiManager = scripts.GetComponent<GUIManager>();
		music = scripts.GetComponent<AudioSource>();
		int musicOff = PlayerPrefs.GetInt("MUSIC_OFF", 0);
		if(musicOff == 0) {
			EnableMusic();
		}
		//RESET HIGH SCORE
		//if(currentLevel.level == 1 && stage == 1) {
		//	PlayerPrefs.SetInt(GameConstants.LEADERBOARD_ID, 0);
		//}
		//more points on higher levels
		currentScore = 0;
		PlayerPrefs.SetInt(GameConstants.CURRENT_SCORE, currentScore);
		highScore = PlayerPrefs.GetInt(GameConstants.LEADERBOARD_ID, 0);

		guiManager.UpdateCurrentHighScore(highScore);
		

		LoadAllLevels(currentLevel);

		if(CheckHasExtraMoves() || hasExtraMoves) {
			guiManager.ResetExtraMoves();
		}
		leftTwinMoved = rightTwinMoved = false;
		gameStarted = false;


		if(stage > 1) {
			Invoke("StartGame", 1.5f);
		}
		//Invoke("StartGame", 1f);
	}

	void LoadAllLevels(Level current) {
		if(allLevels == null || allLevels.Length == 0) {
			allLevels = FindObjectsOfType<Level>();
		}
		
		/*if(allLevels != null && allLevels.Length > 0) {
			foreach(Level level in allLevels) {
				if(level.level != current.level) {
					level.transform.gameObject.SetActive(false);
				}
				else {
					level.transform.gameObject.SetActive(true);
				}
			}
		}*/
	}
	public void DisableMusic() {
		if(music != null){
			music.Stop();
		}
	}

	public void EnableMusic() {
		if(music != null){
			music.Play();
		}
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void AddDestroyableObject(Destroyable script)
	{
		if(listOfDestroyables == null) {
			listOfDestroyables = new List<Destroyable>();
		}

		if(!listOfDestroyables.Contains(script)) {
			listOfDestroyables.Add(script);
		}
		
	}

	public void AddResetableBehaviourObject(ResetBehaviourScript script) {
		if(listOfBehaviours == null) {
			listOfBehaviours = new List<ResetBehaviourScript>();
		}
		//TODO check this
		if(!listOfBehaviours.Contains(script)) {
			listOfBehaviours.Add(script);
		}
		
	}

	public void ResetAllBehaviours() {
		foreach(ResetBehaviourScript script in listOfBehaviours) {
			script.ResetOriginalBehaviour();
		}
	}

	public void DestroyAllDestroyables() {
		foreach(Destroyable script in listOfDestroyables) {
			if(script!=null && script.gameObject!=null) {
				Destroy(script.gameObject);
			}
			
		}
		listOfDestroyables.Clear();
	}

	public void StartGame() {

		RestartLevel();
		ResetAllBehaviours();
		DestroyAllDestroyables();
		gameStarted = true;

		if (stage == 1 && currentLevel.level == 1)
		{
			scripts.GetComponent<TutorialController>().showPartOne();
		}
	}

	public void StartNextStage() {

		guiManager.DoStageTransitionEffect();
	}

	public bool IsGameStarted() {
		return gameStarted;
	}

	public void MoveLeftTwin() {
		leftTwinMoved = true;
	}

	public void MoveRightTwin() {

		rightTwinMoved = true;
	}

	public void RestartTwinMoves() {
		leftTwinMoved = rightTwinMoved = false;
	}

	public bool DidLeftTwinMoved() {
		return leftTwinMoved;
	}

	public bool DidRightTwinMoved() {
		return rightTwinMoved;
	}

	public void setCurrentLevel(Level level) {
		currentLevel = level;
		guiManager.SetLevelText(currentLevel.level);
	}

	public void ResetMoves() {
		bool hasExtra = CheckHasExtraMoves();
		numMoves = hasExtra ? MAX_MOVES + 2 : MAX_MOVES;
		guiManager.ResetMoves(hasExtra);
	}

	public void decreaseMove() {
		if(numMoves > 0 ) {
			numMoves -= 1;
		}
		else {
			KillPlayer();
		}

		guiManager.SetMovesText(numMoves, CheckHasExtraMoves());
	}

	public bool CheckIfBothAreDead() {

		if(twins[0].IsStopped() && twins[1].IsStopped() && numMoves == 0) {
			KillPlayer();
			return true;
		}
		return false;
	}

	public void KillPlayer() {
	
		if (!isDead && gameStarted)
		{
			Debug.Log("KillPlayer CALLED");
			isDead = true;
			numMoves = 0;
			guiManager.SetMovesText(numMoves, CheckHasExtraMoves());
			gameStarted = false;
			guiManager.ShowGameOver();
		}
		else Debug.Log("OH NO!!!!!!");
		
	}

	//after watching the video or purchasing the revives
	public void RestartFromDyingLevel() {
		SoundEffectsHelper.Instance.PlayTeleportSound(); //TODO change sound
		respawnOnDyingLevel = true;
		respawnLevel = currentLevel;
		RestartLevel();
		ResetAllBehaviours();
		DestroyAllDestroyables();
	}

	public bool isPlayerDead() {
		return isDead;
	}

	private bool CheckHasExtraMoves() {
		return (PlayerPrefs.GetInt(GameConstants.PRODUCT_EXTRA_MOVES, 0) == 1) || hasExtraMoves;
	}

	public void RestartLevel() {

		Debug.Log("######## RESTART LEVEL #############");
		//set initial rotation and scale
		foreach(PlayerMovement twin in twins) {
			twin.ResetPlayer();
		}
		isDead = false;
		isDying = false;
		numMoves = CheckHasExtraMoves() ? MAX_MOVES + 2 : MAX_MOVES;
		gameStarted = true;
		guiManager.ResetRegularMoves();
		if(hasExtraMoves || CheckHasExtraMoves()) {
			guiManager.ResetExtraMoves();
		}
		guiManager.HideGameOver();
		foreach(PlayerMovement player in twins) {
			player.ResetOriginalSprite();
			player.ResetOriginalPosition();
		}

		if(isDebugMode && debugLevel!=null) {
			MoveToRespawnLevel(debugLevel);
		}
		else {
			if(respawnOnDyingLevel) {
				MoveToRespawnLevel(respawnLevel);			
			}
			else {
				MoveToRespawnLevel(currentStageFirstLevel);
			}
		}

		
		
	}

	public void SetIsDying(bool willDie) {
		isDying = willDie;
	}

	public bool IsAboutToDie() {
		return isDying;
	}

	public void NewStageLoaded() {
		isDead = false;
		numMoves = CheckHasExtraMoves() ? MAX_MOVES + 2 : MAX_MOVES;
		gameStarted = true;
		guiManager.ResetRegularMoves();
	}

	void FixedUpdate() {

		// if (m_lastPressed != Time.time) {
     	//m_lastPressed = Time.time;
     	// Code here.
 		//}

		if(gameStarted) {

			
			int moved = 0;
			if ( (Input.GetKeyDown(KeyCode.UpArrow) || swipe!=null && swipe.upSwipe ) )
	        {
	
				
				foreach(PlayerMovement player in twins) {
					if(player.TrySlideUp()) {
						moved++;
					}
				}
	
				
	        }
	        else if ( (Input.GetKeyDown(KeyCode.RightArrow)|| swipe!=null && swipe.rightSwipe)  ) 
	        {
				
				foreach(PlayerMovement player in twins) {
					if(player.TrySlideRight()) {
						moved++;
					}
				}
				
	        }
	        else if ( (Input.GetKeyDown(KeyCode.DownArrow) || swipe!=null && swipe.downSwipe ) )
	        {
	
				foreach(PlayerMovement player in twins) {
					if(player.TrySlideDown()) {
						moved++;
					}
				}
				
	        }
	        else if ( (Input.GetKeyDown(KeyCode.LeftArrow) || swipe!=null && swipe.leftSwipe ) )
	        {  
	
				foreach(PlayerMovement player in twins) {
					if(player.TrySlideLeft()) {
						moved++;
					}
				}
	        }
	
			
			if(moved > 0) {
				//leftTwinMoved = rightTwinMoved = false;
				decreaseMove();
				
			}
	
			if( (leftTwinReady && rightTwinReady)/*&& currentLevel.level > 1*/) { //don´t do this at first start
				
				foreach(PlayerMovement player in twins) {
	
					player.ResetPlayerOnNewLevel(); //will also set the new associated level for both twins
				}

				if(currentLevel.level > 1) {
					ShowLevelNum();
				} //else called from GUIManager after the stage image
	
				leftTwinReady = rightTwinReady = false;
			}

		}
		
	}

	public void LeftTwinReachedNewLevel(bool reached, MoveTowardsScript move, LevelCheckPoint restrictions) {
		leftTwinReady = reached;
		//Debug.Log("#################### LEFT TWIN REACHED " + leftTwinReady);
		twins[0].SetReachedNewLevel(true, restrictions);
		twins[0].transform.parent = restrictions.transform.parent;
		move.gameObject.GetComponent<PlayerMovement>().SetReachedNewLevel(true,restrictions);
		move.startMoveTowards = false;
		move.enabled = false;
	}

	public void RightTwinReachedNewLevel(bool reached, MoveTowardsScript move, LevelCheckPoint restrictions) {
		rightTwinReady = reached;
		//Debug.Log("################### RIGHT TWIN REACHED " + rightTwinReady);
		twins[1].SetReachedNewLevel(true, restrictions);
		twins[0].transform.parent = restrictions.transform.parent;
		move.gameObject.GetComponent<PlayerMovement>().SetReachedNewLevel(true, restrictions);
		move.startMoveTowards = false;
		move.enabled = false;
	}

	public bool HasLeftTwinReachedNewLevel() {
		return leftTwinReady;
	}

	public bool HasRightTwinReachedNewLevel() {
		return rightTwinReady;
	}


	public void TwinCollidedWithPortal(GameObject twin) {

		//Debug.Log("TWIN COLLIDED WITH PORTAL: isLeftTwin? ");
		PlayerMovement player = twin.GetComponent<PlayerMovement>();
		if(player!=null) {

			//Debug.Log("TWIN COLLIDED WITH PORTAL: isLeftTwin? " + player.isLeftTwin);
			player.SetIsMovingBetweenLevels(true);
			//do the same for the twin
			player.otherTwin.SetIsMovingBetweenLevels(true);
		}
	}

	//this was on portal before 
    //this is called after TwinCollidedWithPortal(twin)
	public void MoveToNextLevel(Level nextLevel) {
		
		Debug.Log("COLLIDED WITH PORTAL, move to level" + nextLevel);
		
		CameraZoomInOutScript scr = Camera.main.GetComponent<CameraZoomInOutScript>();
		//Move the camera to next level position
		scr.currentLevel = currentLevel.level;
		scr.nextLevel = nextLevel.level;
		LoadAllLevels(nextLevel);
		scr.MoveToNextLevel();
		//Move the players
		StartMovePlayersIntoPosition(nextLevel);
		setCurrentLevel(nextLevel);
		ResetMoves();
		
		
	}

	public void MoveToRespawnLevel(Level respawnLevel) {
		
		//Debug.Log("###### RESPAWN MoveToRespawnLevel, move to level" + respawnLevel);

		twins[0].SetIsMovingBetweenLevels(true);
		twins[0].otherTwin.SetIsMovingBetweenLevels(true);
		twins[1].SetIsMovingBetweenLevels(true);
		twins[1].otherTwin.SetIsMovingBetweenLevels(true);
		
		CameraZoomInOutScript scr = Camera.main.GetComponent<CameraZoomInOutScript>();
		//Move the camera to next level position
		scr.currentLevel = currentLevel.level;
		scr.nextLevel = respawnLevel.level;
		LoadAllLevels(respawnLevel);
		scr.MoveToRespawnLevel(respawnLevel.level);
		//Move the players
		StartMovePlayersIntoPosition(respawnLevel);
		setCurrentLevel(respawnLevel);
		ResetMoves();
		
		
	}

	//show the num of the new level
	public void ShowLevelNum() {
		guiManager.ShowLevelNumImages(this.currentLevel.level);
		StartCoroutine(HideLevelNumImages());
	}

	//when i finish all levels on 1 stage
	public void StageCleared() {
		//gameStarted = false;
		//Report the achievement
		SocialAPI.Instance.AddAchievement(GameConstants.ACHIEVEMENT_STAGE_GENERIC_ID + stage);
		//add 500 extra points
		currentScore = PlayerPrefs.GetInt(GameConstants.CURRENT_SCORE, 0);
		currentScore += 500;
		//current score

		PlayerPrefs.SetInt(GameConstants.CURRENT_SCORE, currentScore);
		guiManager.UpdateCurrentScore(currentScore);
		
		//high score
		highScore = PlayerPrefs.GetInt(GameConstants.LEADERBOARD_ID, 0);

		//new best
		if(currentScore > highScore) {
			highScore = currentScore;
		}

		PlayerPrefs.SetInt(GameConstants.LEADERBOARD_ID, highScore);
		guiManager.UpdateCurrentHighScore(highScore);

		//also report it to the store
		SocialAPI.Instance.AuthenticateAndReport(currentScore, GameConstants.LEADERBOARD_ID);

		guiManager.ShowStageClearedImage();
		StartCoroutine("HideStageClearedImage");
	}

	public void LevelCleared() {
		//gameStarted = false;
		guiManager.ShowLevelClearedImage();
		//------------------ 100 points per level finished
		currentScore = PlayerPrefs.GetInt(GameConstants.CURRENT_SCORE, 0);
		currentScore += 100 * numMoves;
		PlayerPrefs.SetInt(GameConstants.CURRENT_SCORE, currentScore);
		guiManager.UpdateCurrentScore(currentScore);

		//high score
		highScore = PlayerPrefs.GetInt(GameConstants.LEADERBOARD_ID, 0);

		//new best
		if(currentScore > highScore) {
			highScore = currentScore;
		}

		PlayerPrefs.SetInt(GameConstants.LEADERBOARD_ID, highScore);
		guiManager.UpdateCurrentHighScore(highScore);

		//also report it to the store
		SocialAPI.Instance.AuthenticateAndReport(currentScore, GameConstants.LEADERBOARD_ID);
		//---------------------------
		
		StartCoroutine("HideLevelClearedImage");
	}

	IEnumerator HideLevelNumImages() {
		yield return new WaitForSeconds(1.2f);
		guiManager.HideLevelNumImages();
	}

	IEnumerator HideLevelClearedImage() {

		yield return new WaitForSeconds(1.2f);
		guiManager.HideLevelClearedImage();
	}

	IEnumerator HideStageClearedImage() {

		yield return new WaitForSeconds(3f);
		guiManager.HideStageClearedImage();
		yield return new WaitForSeconds(3f);
		guiManager.DisableStageClearedImage();
	}
	void StartMovePlayersIntoPosition(Level nextLevel) {
		

		LevelCheckPoint[] checkpoints = nextLevel.gameObject.GetComponentsInChildren<LevelCheckPoint>();
	//	yield return new WaitForSeconds(1.2f);
		SoundEffectsHelper.Instance.PlayTeleportSound();

		

		foreach(PlayerMovement moveScript in twins) {

			bool isLeft = moveScript.isLeftTwin;
			MoveTowardsScript moveTowards = moveScript.gameObject.GetComponent<MoveTowardsScript>();
			if(moveTowards!=null) {

				foreach(LevelCheckPoint check in checkpoints) {
					if( (check.isLeftCheckpoint && isLeft) || (!check.isLeftCheckpoint && !isLeft) ) {
						moveTowards.target = check.gameObject.transform;	
					}
				}

				moveTowards.startMoveTowards = false;
				moveTowards.enabled = true;
				moveTowards.StartMovingTowards(true, moveScript.isLeftTwin);
			}
			
		} 
		
			
	}


	public bool IsStillAwaitingLevelTransitions() {


		PlayerMovement moveScriptLeft = twins[0];
		PlayerMovement moveScriptRight = twins[1];
		if(moveScriptLeft.GetIsMovingBetweenLevels() || moveScriptRight.GetIsMovingBetweenLevels()) {

			//Debug.Log("### STILL WAITING....");
			return true;
		}

		//leftTwinReady = rightTwinReady = false;
		//if(currentLevel > 1)
		//Debug.Log("### NOT WAITING ANYMORE ...." + moveScriptLeft.GetIsMovingBetweenLevels() + " : " + moveScriptLeft.otherTwin.GetIsMovingBetweenLevels());
		return false;
	}
}
