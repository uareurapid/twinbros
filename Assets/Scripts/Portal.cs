using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Portal : MonoBehaviour {

	public Level currentLevel;
	public Level nextLevel;

	//GUIManager guiManager;
	LevelManager levelManager;

	SceneLoader loader;

	// Use this for initialization
	void Start () {
		GameObject scripts = GameObject.FindGameObjectWithTag("Scripts");
		levelManager = scripts.GetComponent<LevelManager>();
		loader = scripts.GetComponent<SceneLoader>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}


	public void MoveToNextLevel() {
		if(nextLevel!=null) {
			levelManager.MoveToNextLevel(nextLevel);
		}//else, on last level
		else {
			if(loader!=null) {
				loader.LoadNextScene();
			}
			else {
				levelManager.KillPlayer();
			}
		}
		
	}
}
