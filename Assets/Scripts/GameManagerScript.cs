using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//singleton will handle the activation/deactivation of all levels on the stage
public class GameManagerScript : MonoBehaviour {

	//list of all levels
	public Level[] levels;

    private GameObject starField;
	private Vector3 starFieldInitialPosition;
	// Use this for initialization
	void Start () {
        starField = GameObject.FindGameObjectWithTag("StarField");
		if(starField!=null) {
			starFieldInitialPosition = starField.transform.position;
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
}
