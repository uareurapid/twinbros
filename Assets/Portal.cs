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
		levelManager.setCurrentLevel(nextLevel);
		guiManager.SetLevelText(nextLevel);
		
	}
}
