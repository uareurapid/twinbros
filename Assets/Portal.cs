using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Portal : MonoBehaviour {

	public int currentLevel = 0;
	public int nextLevel = 0;

	GUIManager guiManager;
	LevelManager levelManager;

	// Use this for initialization
	void Start () {
		GameObject scripts = GameObject.FindGameObjectWithTag("Scripts");
		guiManager = scripts.GetComponent<GUIManager>();
		levelManager = scripts.GetComponent<LevelManager>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}


	public void MoveToNextLevel() {
		
		Debug.Log("COLLIDED WITH PORTAL, move to level" + nextLevel);
		CameraZoomInOutScript scr = Camera.main.GetComponent<CameraZoomInOutScript>();
		scr.currentLevel = currentLevel;
		scr.nextLevel = nextLevel;
		scr.MoveToNextLevel();

		MovePlayersIntoPosition();

		levelManager.setCurrentLevel(nextLevel);
		guiManager.SetLevelText(nextLevel);
		
	}

	void MovePlayersIntoPosition() {
		GameObject [] twins = GameObject.FindGameObjectsWithTag("Player");
		foreach(GameObject twin in twins) {

			//disable the rigidbody
			//Rigidbody2D body = twin.GetComponent<Rigidbody2D>();

			//disable all the colliders, parent and children
			Movement player = twin.GetComponent<Movement>();
			if(player!=null) {
				player.DisableColliders();
			}

			MoveTowardsScript move = twin.GetComponent<MoveTowardsScript>();

			if(move!=null) {

				StartCoroutine(MoveTwins(move));
				
			}
			
			
		} 
	}

	IEnumerator MoveTwins(MoveTowardsScript move) {
		yield return new WaitForSeconds(1.2f);
		move.enabled = true;//.StartMovingTowards(true);
			
	}
}
