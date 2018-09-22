using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Portal : MonoBehaviour {

	public Level currentLevel;
	public Level nextLevel;

	//GUIManager guiManager;
	LevelManager levelManager;

	// Use this for initialization
	void Start () {
		GameObject scripts = GameObject.FindGameObjectWithTag("Scripts");
		levelManager = scripts.GetComponent<LevelManager>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}


	public void MoveToNextLevel() {
		if(nextLevel!=null) {
			levelManager.MoveToNextLevel(nextLevel);
		}//else, on last level
		
	}
}
