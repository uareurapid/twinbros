using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Portal : MonoBehaviour, HandlePlayerCollision {

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
			Debug.Log("MoveToNextLevel: " + nextLevel.level);
			levelManager.LevelCleared();
			SoundEffectsHelper.Instance.PlayPowerupSound();
			levelManager.MoveToNextLevel(nextLevel);
		}//else, on last level
		else {
			if(loader!=null) {
				levelManager.StageCleared();
				loader.LoadNextScene(levelManager);
				
			}
			else {
				Debug.Log("WTF");//GAME OVER ALL STAGES DONE? TODO
				levelManager.KillPlayer();
			}
		}
		
	}

	public void HandleExitCollision(PlayerMovement player) {

	}

    public void HandleCollision(PlayerMovement player) {
        //is portal
        if (player.GetIsMovingBetweenLevels() || levelManager.isPlayerDead())
        {
            //ignore this collision
            return;
        }
        else
        {
            //TODO keep coding me
            levelManager.TwinCollidedWithPortal(player.gameObject);
            StartCoroutine(MoveToNextLevelCoroutine());
        }
    }

    IEnumerator MoveToNextLevelCoroutine()
    {
        yield return new WaitForSeconds(0.5f);
        MoveToNextLevel();
    }

	public void EnablePortal() {

		GetComponent<Collider2D>().enabled = true;
		GetComponentInChildren<SwapSpriteScript>().enabled = true;
	}

	//disable portals
	public void DisablePortal() {

		GetComponent<Collider2D>().enabled = false;
		GetComponentInChildren<SwapSpriteScript>().enabled = false;

	}
}
