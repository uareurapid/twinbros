using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelStatus : MonoBehaviour
{
	//is the level locked or unlocked?
	public bool locked = false;

	public int number = 1; // level number
	public Sprite lockedImage;
	public Sprite unlockedImage;
	// Use this for initialization
	void Start()
	{
		//TODO assign the "locked" variable according the UserPrefs
	}

	// Update is called once per frame
	void Update()
	{

	}

	public void unlockLevel()
	{
		locked = true;
		GetComponent<Image>().sprite = unlockedImage;
	}
	public void lockLevel() {
		locked = false;
		GetComponent<Image>().sprite = lockedImage;
	}
}
