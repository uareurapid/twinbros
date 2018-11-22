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
			levelManager.LevelCleared();
			//------------------ 100 points per level finished
			int currentScore = PlayerPrefs.GetInt(GameConstants.LEADERBOARD_ID, 0);
			currentScore += 100;
			PlayerPrefs.SetInt(GameConstants.LEADERBOARD_ID, currentScore);
			SocialAPI.Instance.AuthenticateAndReport(currentScore, GameConstants.LEADERBOARD_ID);
			//---------------------------
			SoundEffectsHelper.Instance.PlayPowerupSound();
			levelManager.MoveToNextLevel(nextLevel);
		}//else, on last level
		else {
			if(loader!=null) {
				levelManager.StageCleared();
				loader.LoadNextScene(levelManager);
				
			}
			else {
				levelManager.KillPlayer();
			}
		}
		
	}
}
