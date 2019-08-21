using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//singleton will handle the activation/deactivation of all levels on the stage
public class GameManagerScript : MonoBehaviour {

	//list of all levels
	public Level[] levels;

    private GameObject starField;
	private Vector3 starFieldInitialPosition;
    
    private List<string> bonusList; // to write twin

	//all the managers should be here
	public SoundManager soundManager;
	private bool gameStarted = false;
	private bool gameEnded = false;
	// Use this for initialization
	void Start () {
    
        bonusList = new List<string>();
    
        starField = GameObject.FindGameObjectWithTag("StarField");
		if(starField!=null) {
			starFieldInitialPosition = starField.transform.position;
		}

		if(soundManager==null) {
			GameObject scripts = GameObject.FindWithTag("Scripts");
			if(scripts!=null) {
				soundManager = scripts.GetComponent<SoundManager>();
			}
		}
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	//deactivate all levels except the current one being played (will be better for performance i think)
	public void DeactivateAllLevelsExcept(Level current) {
		foreach(Level level in levels) {
			if(level.level != current.level) {
				level.gameObject.SetActive(false);	
			}
		}
	}

	//should be called when moving to the next level
	public void ActivateNextLevel(Level nextOne) {
		nextOne.gameObject.SetActive(true);	
		//TODO disable the others?
		//need to address other cases, like restarting, continue, respawning levels, etc.. for later..
	}

	public void ActivateNextLevelByNum(int nextOne) {
		foreach(Level level in levels) {
			if(level.level == nextOne) {
				level.gameObject.SetActive(true);	
			}
		}
		//TODO disable the others?
		//need to address other cases, like restarting, continue, respawning levels, etc.. for later..
	}

    public void DisableStarField() {
        if(starField!=null) {
            starField.GetComponent<StarFieldManagerComponent>().MoveStarFieldToNextLevel(); 
        }
    }

    public void EnableStarFieldOnLocation(Transform parent, Vector3 position) {
        if(starField!=null) {
            starField.GetComponent<StarFieldManagerComponent>().RestartStarFieldOnNewLocation(parent); 
        }
    }
    
    // TWIN BONUS
    public void AddBonusLetter(string letter) {
        if(bonusList !=null && !bonusList.Contains(letter)) {

            bonusList.Add(letter);
        }
    }
    
    public bool ShouldGiveBonusMove() {

        return bonusList != null && bonusList.Contains("T") && bonusList.Contains("W") && bonusList.Contains("I") && bonusList.Contains("N");
    }
    
    public void AddBonusMove() {
        PlayerPrefs.SetInt(GameConstants.HAS_BONUS_MOVE, 1);
    }
    
    private void RemoveBonusMove() {
        if(PlayerPrefs.HasKey(GameConstants.HAS_BONUS_MOVE) ) {
            PlayerPrefs.DeleteKey(GameConstants.HAS_BONUS_MOVE);
        }
    }
    
    public bool HasBonusMove() {
        return PlayerPrefs.HasKey(GameConstants.HAS_BONUS_MOVE);
    }
    
    public void KillPlayer() {
        RemoveBonusMove();
    }

	public void StartGame() {
		gameEnded = false;
		gameStarted = true;
		if(soundManager!=null) {
            //lower the volume first
            soundManager.SwitchAudioClips(gameStarted);
		}
	}

	public void EndGame() {
		gameEnded = true;
		gameStarted = false;
		if(soundManager!=null) {
            soundManager.SwitchAudioClips(gameStarted);
		}
	}

}
