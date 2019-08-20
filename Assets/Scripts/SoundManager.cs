using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour {

	private AudioSource audioS;
	// Use this for initialization
	void Start () {
		audioS = GetComponent<AudioSource>();

	}
	
	// Update is called once per frame
	void Update () {
		
	}


	public void SetVolume(float toValue) {
		if(audioS!=null && toValue >= 0.0f && toValue <= 1.0f) {
			audioS.volume = toValue; //between 0.0 and 1.0
		}
	}

	public void DisableAudio() {
		if(audioS!=null) {
			audioS.enabled = false;
		}
	}

	public void EnableAudio() {
		if(audioS!=null) {
			audioS.enabled = true;
		}
	}
}
