using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour {

	public int currentLevel = 1;
	public int numMoves = 10;

	private GUIManager guiManager;
	private bool isDead = false;
	// Use this for initialization
	void Start () {
		GameObject scripts = GameObject.FindGameObjectWithTag("Scripts");
		guiManager = scripts.GetComponent<GUIManager>();
		//guiManager.SetMovesText(numMoves+1);
	}


	
	// Update is called once per frame
	void Update () {
		
	}

	public void setCurrentLevel(int level) {
		currentLevel = level;
	}

	public void decreaseMove() {
		if(numMoves > 0) {
			numMoves -= 1;
		}
		
		if(numMoves == 0) {
			isDead = true;
			Debug.Log("IS DEAD");
		}

		guiManager.SetMovesText(numMoves);
	}
}
