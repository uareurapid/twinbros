using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelStatus : MonoBehaviour {
	//is the level locked or unlocked?
	public bool locked = false;
	public int stage = 1;

	void Start()
	{
		// Use this for initialization
		//TODO assign the "locked" variable according the UserPrefs
		//achieved stage 1 end
		if (stage == 1)
        {
			this.locked = false;
        } else
        {
			this.locked = PlayerPrefs.GetInt(GameConstants.ACHIEVEMENT_STAGE_GENERIC_ID + (stage - 1).ToString(), 0) == 0;
		}
		
	}

    public bool GetIsLocked()
    {
		if (stage == 1)
		{
			this.locked = false;
		}
		else
		{
			this.locked = PlayerPrefs.GetInt(GameConstants.ACHIEVEMENT_STAGE_GENERIC_ID + (stage - 1).ToString(), 0) == 0;
		}
		return this.locked;
    }

// Update is called once per frame
void Update () {
		
	}
}
