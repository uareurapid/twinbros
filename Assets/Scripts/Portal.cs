using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Portal : MonoBehaviour {

	public int currentLevel = 0;
	public int nextLevel = 0;

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
		levelManager.MoveToNextLevel(nextLevel);
	}
}
