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

	private GameManagerScript gameManager;
	private bool isDead = false;
	private bool isDying = false;

	public bool isDebugMode = false;

	public bool isTestMode = false;

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
	public List<GameObject> listOfDestroyables;

	private SoundManager soundManager;

    private bool hasShownLevelNum = false;

	private long lastMovementTime = 0;
	//keep a reference for this
	private GameObject scripts;

	public int currentScore = 0;
	public int highScore = 0;

	void Start () {
                  
		if(CheckHasExtraMoves() || hasExtraMoves) {
			numMoves = MAX_MOVES + 2;
		} else if(CheckHasBonusMove()) {
            numMoves = MAX_MOVES + 1;
        }

		if(Application.platform == RuntimePlatform.IPhonePlayer || Application.platform == RuntimePlatform.Android && swipe==null) {
			swipe = gameObject.AddComponent<SwipeDetector>();
			swipe.detectSwipeOnlyAfterRelease = true;
		}

		currentStageFirstLevel = respawnLevel;

		listOfBehaviours = new List<ResetBehaviourScript>();
		listOfDestroyables = new List<GameObject>();

		scripts = GameObject.FindGameObjectWithTag("Scripts");
		guiManager = scripts.GetComponent<GUIManager>();
		gameManager = scripts.GetComponent<GameManagerScript>();
		soundManager = scripts.GetComponent<SoundManager>();
		int musicOff = PlayerPrefs.GetInt("MUSIC_OFF", 0);
		if(musicOff == 0) {
			EnableMusic();
		}

		CheckMoves();

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
		} else {
            guiManager.DisableExtraMoves();
        }
		leftTwinMoved = rightTwinMoved = false;
		gameStarted = false;


		if(stage > 1) {
			Invoke("StartGame", 1.5f);
		}
	}
    
    public GUIManager GetGUIManager() {
        return guiManager;
    }
    
    public GameManagerScript GetGameManagerScript() {
        return gameManager;
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

    public bool GetIsTestMode()
    {
		return isTestMode;
    }

    public bool ShouldRespawnOnDyingLevel()
    {
		return respawnOnDyingLevel;
    }

	public void DisableMusic() {
		if(soundManager != null){
			soundManager.StopAudio();
		}
	}

    //todo delegate to sound manager
	public void EnableMusic() {
		if(soundManager != null){
			soundManager.StartAudioWithDelay(3f);
		}
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void AddDestroyableObject(Destroyable script)
	{
		if(listOfDestroyables == null) {
			listOfDestroyables = new List<GameObject>();
		}

		if(!listOfDestroyables.Contains(script.gameObject)) {
			listOfDestroyables.Add(script.gameObject);
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
		foreach(GameObject obj in listOfDestroyables) {
            Destroyable script = obj.GetComponent<Destroyable>();
			if(script!=null && script.gameObject!=null) {
				Destroy(script.gameObject);
			}
			
		}
		listOfDestroyables.Clear();
	}

	public void StartGame() {

        if(!isDead){
          foreach(PlayerMovement twin in twins) {
              //make sure the sprite is on
                twin.ShowSprites();
                twin.DisableBubbleOnStartup();
            }
        }
		//TODO eu não posso carregar e começar  nivel antes do resetbehaviours
		//primeiro o reset e quando terminar é que posso clicar no botão!!!
		gameManager.StartGame();
		RestartLevel();
		ResetAllBehaviours();
		DestroyAllDestroyables();
        gameStarted = true;
		guiManager.ResetScore();
        CleanPreviousPaths();
        
	}

    private void CleanPreviousPaths()
    {
        foreach (GridTile childGrid in FindObjectsOfType<GridTile>())
        {
            childGrid.GetComponent<SpriteRenderer>().enabled = false;
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
		//TODO LATER
		//gameManager.DeactivateAllLevelsExcept(currentLevel);
		//gameManager.ActivateNextLevel(currentLevel);
	}

	public void ResetMoves() {
		bool hasExtra = CheckHasExtraMoves();
        bool hasBonus = CheckHasBonusMove();
		numMoves = hasExtra ? MAX_MOVES + 2 : MAX_MOVES;
        
        if(!hasExtra && hasBonus) {
          numMoves = MAX_MOVES + 1;
        }
		guiManager.ResetMoves(hasExtra, hasBonus);
	}

	public void increaseMoves(int num)
	{
        if (numMoves + num < MAX_MOVES + 2)
        {
            numMoves += num;

            Vector3 pos = guiManager.movesImage[numMoves - 1].gameObject.GetComponent<RectTransform>().position;
            SpecialEffectsHelper.Instance.PlayRiseMovesEffect(pos);
            Debug.Log("NUM MOVES: " + numMoves);
            //guiManager.UpdateMovesText(numMoves, CheckHasExtraMoves(), CheckHasBonusMove());
        }
		
	}

	public void decreaseMove() {
		if(numMoves > 0 ) {
			numMoves -= 1;
		}
		else {
			KillPlayer();
		}

		guiManager.SetMovesText(numMoves, CheckHasExtraMoves(), CheckHasBonusMove());
	}

	public bool CheckIfBothAreDead() {

		if(twins[0].IsStopped() && twins[1].IsStopped() && numMoves == 0) {
			KillPlayer();
			return true;
		}
		return false;
	}

	public bool IsThereAnyMovesLeft() {
		if( twins[0].GetReachedTarget() && twins[1].GetReachedTarget() && numMoves == 0) {

			KillPlayer();
			return false;
		}
		//Debug.Log("######### NUM MOVES ############: " + numMoves);
		return true;
	}

	public void KillPlayer() {
	
		if (!isDead && gameStarted)
		{
			//Debug.Log("KillPlayer CALLED");
			gameManager.EndGame();
			isDead = true;
			if(numMoves >=1) {
                numMoves -= 1;
            }
			guiManager.SetMovesText(numMoves, CheckHasExtraMoves(), CheckHasBonusMove());
			gameStarted = false;
			guiManager.ShowGameOver();
            gameManager.KillPlayer();
		}
		
	}
    
	//after watching the video or purchasing the revives
    //but also if respawnOnDyingLevel is set and test mode
	public void RestartFromDyingLevel() {
		SoundEffectsHelper.Instance.PlayTeleportSound(); //TODO change sound
		respawnOnDyingLevel = true;
		respawnLevel = currentLevel;
		RestartLevel();
		ResetAllBehaviours();
		DestroyAllDestroyables();

        //added these
        gameStarted = true;
        guiManager.ResetScore();
        CleanPreviousPaths();

    }

	public bool IsPlayerDead() {
		return isDead;
	}

	private bool CheckHasExtraMoves() {
		return (PlayerPrefs.GetInt(GameConstants.PRODUCT_EXTRA_MOVES, 0) == 1) || hasExtraMoves;
	}
    
    private bool CheckHasBonusMove() {
        return PlayerPrefs.HasKey(GameConstants.HAS_BONUS_MOVE) && PlayerPrefs.GetInt(GameConstants.HAS_BONUS_MOVE, 0) == 1;
    }


	private void CheckMoves() {

		numMoves = CheckHasExtraMoves() ? MAX_MOVES + 2 : CheckHasBonusMove() ? MAX_MOVES + 1 : MAX_MOVES;
		guiManager.ResetRegularMoves();
		if(hasExtraMoves || CheckHasExtraMoves()) {
			guiManager.ResetExtraMoves();
		} else {

            guiManager.DisableExtraMoves();//TODO check
        }
        
         if(CheckHasBonusMove()) {
            guiManager.ResetBonusMoves();
        }
	}

	public void RestartLevel() {

		//Debug.Log("######## RESTART LEVEL #############");
		//set initial rotation and scale
		foreach(PlayerMovement twin in twins) {
			twin.ResetPlayer();
		}
		isDead = false;
		isDying = false;
        
        gameStarted = true;
		
        CheckMoves();
		
		guiManager.HideGameOver();
		foreach(PlayerMovement player in twins) {
			player.ResetOriginalSprite();
			player.ResetOriginalPosition();
		}

		if(isDebugMode && debugLevel!=null) {
			MoveToRespawnLevel(debugLevel);
		}
		else {
			if(respawnOnDyingLevel && isTestMode) {
                respawnLevel = currentLevel;
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
    
    public void StartButtonVisible(bool visibleButton) {

        if (visibleButton)
        {
            soundManager.SetVolume(0.4f);
        }
                        
    }

	void FixedUpdate() {

		// if (m_lastPressed != Time.time) {
     	//m_lastPressed = Time.time;
     	// Code here.
 		//}

		if(gameStarted) {

			if(!IsThereAnyMovesLeft() || guiManager.IsGamePaused()){ //ignore gestures if game is paused
				return;
			}

			
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
	
			//on boss level there is no limitation of movements
			if(moved > 0 && !currentLevel.isBossLevel) {
				//leftTwinMoved = rightTwinMoved = false;
				decreaseMove();
				
			}
	
			if( (leftTwinReady && rightTwinReady)/*&& currentLevel.level > 1*/) { //don´t do this at first start
				
				foreach(PlayerMovement player in twins) {
	
					player.ResetPlayerOnNewLevel(); //will also set the new associated level for both twins
				}
                
                leftTwinReady = rightTwinReady = false;

				//TODO show if level 1, but if i just died than do not show the stage image
                //TODO check
				if(currentLevel.level > 1) {
					ShowLevelNum();
				} //else called from GUIManager after the stage image
	
				
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
		twins[1].transform.parent = restrictions.transform.parent;
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

        gameManager.DisableStarField();

		//Move the players
		StartMovePlayersIntoPosition(nextLevel);
		setCurrentLevel(nextLevel);
		ResetMoves();
		
		
	}

	public void MoveToRespawnLevel(Level respawnLevel) {
		
		//Debug.Log("###### RESPAWN MoveToRespawnLevel, move to level" + respawnLevel);

		twins[0].SetIsMovingBetweenLevels(true);
		twins[0].otherTwin.SetIsMovingBetweenLevels(true);
        //twins[0].EnableBubbleOnRespawn();
		twins[1].SetIsMovingBetweenLevels(true);
		twins[1].otherTwin.SetIsMovingBetweenLevels(true);
        //twins[1].EnableBubbleOnRespawn();
		
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

        hasShownLevelNum = true;
		guiManager.ShowLevelNumImages(this.stage, this.currentLevel.level);
		StartCoroutine(HideLevelNumImages());
	}

	//when i finish all levels on 1 stage
	public void StageCleared() {

		//gameStarted = false;
		//Report the achievement
        if(gameManager.IsMobilePlatform()) {
            SocialAPI.Instance.AddAchievement(GameConstants.ACHIEVEMENT_STAGE_GENERIC_ID + stage);
        }
		
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

        if (gameManager.IsMobilePlatform())
        {
            //also report it to the store
            SocialAPI.Instance.AuthenticateAndReport(currentScore, GameConstants.LEADERBOARD_ID);
        }
            

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
	    
		SoundEffectsHelper.Instance.PlayTeleportSound();

        //Debug.Log("MOVE PLAYERS INTO POSITION: " + nextLevel + " size: " + checkpoints.Length);

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
                moveTowards.StartMovingTowards(true, isLeft);
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
