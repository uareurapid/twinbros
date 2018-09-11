using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour {

	public int currentLevel = 1;
	private int numMoves = 10;
	public int MAX_MOVES = 10;

	private GUIManager guiManager;
	private bool isDead = false;


	private bool leftTwinMoved = false;
	private bool rightTwinMoved = false;

	private bool leftTwinReady = false;
	private bool rightTwinReady = false;
	// Use this for initialization

	public PlayerMovement [] twins;

	private bool gameStarted = false;

	void Start () {
		GameObject scripts = GameObject.FindGameObjectWithTag("Scripts");
		guiManager = scripts.GetComponent<GUIManager>();
		leftTwinMoved = rightTwinMoved = false;
		gameStarted = false;
		Invoke("StartGame", 1f);
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

	public void setCurrentLevel(int level) {
		currentLevel = level;
		guiManager.SetLevelText(currentLevel);
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
		numMoves = 0;
		guiManager.SetMovesText(numMoves);
		Debug.Log("IS DEAD");
		isDead = true;
		gameStarted = false;
		guiManager.ShowGameOver();
	}

	public bool isPlayerDead() {
		return isDead;
	}

	public void RestartLevel() {
		isDead = false;
		numMoves = MAX_MOVES;
		gameStarted = true;
	}

	void FixedUpdate() {
		if(leftTwinMoved || rightTwinMoved) {
			decreaseMove();
			leftTwinMoved = rightTwinMoved = false;
		}
		if( (leftTwinReady && rightTwinReady) && currentLevel > 1) { //don´t do this at first start
			
			foreach(PlayerMovement player in twins) {

				player.ResetPlayerOnNewLevel(); //will also set the new associated level for both twins
			}

			leftTwinReady = rightTwinReady = false;
		}
	}

	public void LeftTwinReachedNewLevel(bool reached, MoveTowardsScript move, LevelCheckPoint restrictions) {
		leftTwinReady = reached;
		Debug.Log("#################### LEFT TWIN REACHED " + leftTwinReady);
		twins[0].SetReachedNewLevel(true, restrictions);
		move.gameObject.GetComponent<PlayerMovement>().SetReachedNewLevel(true,restrictions);

		
		move.enabled = false;
	}

	public void RightTwinReachedNewLevel(bool reached, MoveTowardsScript move, LevelCheckPoint restrictions) {
		rightTwinReady = reached;
		Debug.Log("################### RIGHT TWIN REACHED " + rightTwinReady);
		twins[1].SetReachedNewLevel(true, restrictions);
		move.gameObject.GetComponent<PlayerMovement>().SetReachedNewLevel(true, restrictions);
		move.enabled = false;
	}

	public bool HasLeftTwinReachedNewLevel() {
		return leftTwinReady;
	}

	public bool HasRightTwinReachedNewLevel() {
		return rightTwinReady;
	}


	public void TwinCollidedWithPortal(GameObject twin) {

		PlayerMovement player = twin.GetComponent<PlayerMovement>();
		if(player!=null) {

			Debug.Log("TWIN COLLIDED WITH PORTAL: isLeftTwin? " + player.isLeftTwin);
			player.SetIsMovingBetweenLevels(true);
			//do the same for the twin
			player.otherTwin.SetIsMovingBetweenLevels(true);
		}
	}

	//this was on portal before 
    //this is called after TwinCollidedWithPortal(twin)
	public void MoveToNextLevel(int nextLevel) {
		
		Debug.Log("COLLIDED WITH PORTAL, move to level" + nextLevel);
		CameraZoomInOutScript scr = Camera.main.GetComponent<CameraZoomInOutScript>();
		scr.currentLevel = currentLevel;
		scr.nextLevel = nextLevel;
		scr.MoveToNextLevel();

		StartMovePlayersIntoPosition();

		setCurrentLevel(nextLevel);
		ResetMoves();
		
		
	}

	void StartMovePlayersIntoPosition() {
		
		StartCoroutine(MoveTwins());
	}

	IEnumerator MoveTwins() {
		yield return new WaitForSeconds(1.2f);
		SoundEffectsHelper.Instance.PlayTeleportSound();

		foreach(PlayerMovement moveScript in twins) {

			MoveTowardsScript moveTowards = moveScript.gameObject.GetComponent<MoveTowardsScript>();
			if(moveTowards!=null) {
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
