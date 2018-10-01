using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour {

	public Level currentLevel;
	public Level respawnLevel; //in wich level to restart the game after dying?

	public bool respawnOnDyingLevel = false; //TODO IN-APP
	//if true will respawn on the same level that previously was
	//otherwise if respawnLevel !=null set it as the currentLevel

	private int numMoves = 10;
	public int MAX_MOVES = 10;
	public bool hasExtraMoves = false; //TODO IN-APP

	private GUIManager guiManager;
	private bool isDead = false;


	private bool leftTwinMoved = false;
	private bool rightTwinMoved = false;

	private bool leftTwinReady = false;
	private bool rightTwinReady = false;
	// Use this for initialization

	public PlayerMovement [] twins;

	private bool gameStarted = false;

	private SwipeDetector swipe;

	void Start () {

		if(Application.platform == RuntimePlatform.IPhonePlayer || Application.platform == RuntimePlatform.Android && swipe==null) {
			swipe = gameObject.AddComponent<SwipeDetector>();
		}

		GameObject scripts = GameObject.FindGameObjectWithTag("Scripts");
		guiManager = scripts.GetComponent<GUIManager>();
		leftTwinMoved = rightTwinMoved = false;
		gameStarted = false;
		//Invoke("StartGame", 1f);
	}


	
	// Update is called once per frame
	void Update () {
		
	}

	public void StartGame() {

		RestartLevel();
		gameStarted = true;
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
		numMoves = MAX_MOVES;
		guiManager.ResetMoves();
	}

	public void decreaseMove() {
		if(numMoves > 0) {
			numMoves -= 1;
		}
		else {
			KillPlayer();
		}

		guiManager.SetMovesText(numMoves);
	}

	public void KillPlayer() {

		if(!isDead && gameStarted) {
			numMoves = 0;
			guiManager.SetMovesText(numMoves);
			Debug.Log("IS DEAD");
			isDead = true;
			gameStarted = false;
			guiManager.ShowGameOver();
		}
		
	}

	public bool isPlayerDead() {
		return isDead;
	}

	public void RestartLevel() {

		Debug.Log("######## RESTART LEVEL #############");
		isDead = false;
		numMoves = MAX_MOVES;
		gameStarted = true;
		guiManager.ResetAllMovesText();
		guiManager.HideGameOver();
		foreach(PlayerMovement player in twins) {
			player.ResetOriginalSprite();
			player.ResetOriginalPosition();
		}

		if(!respawnOnDyingLevel && currentLevel.level != respawnLevel.level ) {
			MoveToRespawnLevel(respawnLevel);			
		}
		
	}

	void FixedUpdate() {

		if(gameStarted) {
			int moved = 0;
			if ( (Input.GetKey(KeyCode.UpArrow) || swipe!=null && swipe.upSwipe ) )
	        {
	
				
				foreach(PlayerMovement player in twins) {
					if(player.TrySlideUp()) {
						moved++;
					}
				}
	
				
	        }
	        else if ( (Input.GetKey(KeyCode.RightArrow)|| swipe!=null && swipe.rightSwipe)  ) 
	        {
				
				foreach(PlayerMovement player in twins) {
					if(player.TrySlideRight()) {
						moved++;
					}
				}
				
	        }
	        else if ( (Input.GetKey(KeyCode.DownArrow) || swipe!=null && swipe.downSwipe ) )
	        {
	
				foreach(PlayerMovement player in twins) {
					if(player.TrySlideDown()) {
						moved++;
					}
				}
				
	        }
	        else if ( (Input.GetKey(KeyCode.LeftArrow) || swipe!=null && swipe.leftSwipe ) )
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
	
				leftTwinReady = rightTwinReady = false;
			}

		}
		
	}

	public void LeftTwinReachedNewLevel(bool reached, MoveTowardsScript move, LevelCheckPoint restrictions) {
		leftTwinReady = reached;
		Debug.Log("#################### LEFT TWIN REACHED " + leftTwinReady);
		twins[0].SetReachedNewLevel(true, restrictions);
		move.gameObject.GetComponent<PlayerMovement>().SetReachedNewLevel(true,restrictions);
		move.startMoveTowards = false;
		move.enabled = false;
	}

	public void RightTwinReachedNewLevel(bool reached, MoveTowardsScript move, LevelCheckPoint restrictions) {
		rightTwinReady = reached;
		Debug.Log("################### RIGHT TWIN REACHED " + rightTwinReady);
		twins[1].SetReachedNewLevel(true, restrictions);
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
		scr.MoveToNextLevel();
		//Move the players
		StartMovePlayersIntoPosition(nextLevel);
		setCurrentLevel(nextLevel);
		ResetMoves();
		
		
	}

	public void MoveToRespawnLevel(Level respawnLevel) {
		
		Debug.Log("###### RESPAWN MoveToRespawnLevel, move to level" + respawnLevel);

		twins[0].SetIsMovingBetweenLevels(true);
		twins[0].otherTwin.SetIsMovingBetweenLevels(true);
		twins[1].SetIsMovingBetweenLevels(true);
		twins[1].otherTwin.SetIsMovingBetweenLevels(true);
		
		CameraZoomInOutScript scr = Camera.main.GetComponent<CameraZoomInOutScript>();
		//Move the camera to next level position
		scr.currentLevel = currentLevel.level;
		scr.nextLevel = respawnLevel.level;
		scr.MoveToRespawnLevel(respawnLevel.level);
		//Move the players
		StartMovePlayersIntoPosition(respawnLevel);
		setCurrentLevel(respawnLevel);
		ResetMoves();
		
		
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
