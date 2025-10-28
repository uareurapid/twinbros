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

	
	private bool gameStarted = false;
	private bool gameEnded = false;

    //TODO all the managers should be here
    public SoundManager soundManager;
    public TutorialController tutorialController;
    
    
	// Use this for initialization
	void Start () {
    
     /*int width = 640; // or something else
     int height= 480; // or something else
     bool isFullScreen = false; // should be windowed to run in arbitrary resolution
     int desiredFPS = 60; // or something else
 
     Screen.SetResolution (width , height, isFullScreen, desiredFPS );*/
    
        bonusList = new List<string>();
    
        starField = GameObject.FindGameObjectWithTag("StarField");
		if(starField!=null) {
			starFieldInitialPosition = starField.transform.position;
		}

		if(soundManager==null || tutorialController == null) {
			soundManager = GetComponent<SoundManager>();
            //could be null if not on level 1
            tutorialController = GetComponent<TutorialController>();
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
        if(!HasBonusMove())
        {
            return bonusList != null && bonusList.Contains("T") && bonusList.Contains("W") && bonusList.Contains("I") && bonusList.Contains("N");
        }
        return false; //already has it
        
    }
    
    public void AddBonusMove() {
        PlayerPrefs.SetInt(GameConstants.HAS_BONUS_MOVE, 1);
        //clear the list
        bonusList.Clear();
    }
    
    private void RemoveBonusMove() {
        if(HasBonusMove() ) {
            PlayerPrefs.DeleteKey(GameConstants.HAS_BONUS_MOVE);
        }
    }
    
    public bool HasBonusMove() {
        return PlayerPrefs.GetInt(GameConstants.HAS_BONUS_MOVE,0) == 1;
    }
    
    public void KillPlayer() {
        soundManager.PlayDeathSound();
		Debug.Log("$$$$$$$$$$$$$$$$ KILL PLAYER $$$$$$$$$$$$$$$$$$ REMOVE BONUS MOVE $$$$$$$$$$$$$$$$$$$$4");
        RemoveBonusMove();
    }

	public void StartGame() {
		gameEnded = false;
		gameStarted = true;
        int musicOff = PlayerPrefs.GetInt("MUSIC_OFF", 0);

        if (soundManager!=null && musicOff == 0) { //if not OFF then is ON
            //lower the volume first
            soundManager.SwitchAudioClips(gameStarted);
		}
	}

	public void EndGame() {
		gameEnded = true;
		gameStarted = false;
        int musicOff = PlayerPrefs.GetInt("MUSIC_OFF", 0);

        if (soundManager!=null && musicOff == 0) {
            soundManager.SwitchAudioClips(gameStarted);
		}
	}
    
    //TODO move these 3 to GameManager???
    public void ShowTutorial() {
        if(tutorialController!=null) {
            tutorialController.ShowTutorial();  
        }

    }

    public bool IsTutorialEnded()
    {
        if (tutorialController != null)
        {
            return tutorialController.IsTutorialEnded();
        }

        return false;

    }

    public bool IsTutorialStarted()
    {
        if (tutorialController != null)
        {
            return tutorialController.IsTutorialStarted();
        }

        return false;

    }

    //check if i have these products already
    public bool HasPurchasedInfiniteRevives()
    {
        return PlayerPrefs.GetInt(GameConstants.PRODUCT_INFINITE_REVIVES, 0) == 1;
    }

    public void RemoveInfiniteRevives()
    {
        PlayerPrefs.SetInt(GameConstants.PRODUCT_INFINITE_REVIVES, 0);
    }

    public bool HasPurchasedExtraMoves()
    {
        return PlayerPrefs.GetInt(GameConstants.PRODUCT_EXTRA_MOVES, 0) == 1;
    }

    public void RemoveExtraMoves()
    {
        PlayerPrefs.SetInt(GameConstants.PRODUCT_EXTRA_MOVES, 0);
    }

    public bool HasPurchasedRemoveAds()
    {
        return PlayerPrefs.GetInt(GameConstants.PRODUCT_REMOVE_ADS, 0) == 1;
    }

    public void RemoveAdsPurchase()
    {
        PlayerPrefs.SetInt(GameConstants.PRODUCT_REMOVE_ADS, 0);
    }

    public void StopMusic()
    {
        soundManager.StopAudio();
    }

    public void StarMusic()
    {
        soundManager.StartAudio();
    }

}
