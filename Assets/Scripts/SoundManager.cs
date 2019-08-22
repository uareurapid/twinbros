using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour {

	private AudioSource audioS;

    public AudioClip playOnLoad; //audio track to play on load
    public AudioClip playingAudioClip;//while playing
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
    
    public void SetAudioClip(AudioClip clip) {
        if(audioS!=null) {
            audioS.clip = clip;
        }
    }
    
    public void StopAudio() {
        if(audioS!=null && audioS.isPlaying) {
            audioS.Stop();
        }
    }
    
    public void StartAudio() {
        if(audioS!=null && !audioS.isPlaying) {
            audioS.Play();
        }
    }
    
    public void StopAudioWithDelay(float delay) {
        Invoke("StopAudio", delay);
    }
    
    public void StartAudioWithDelay(float delay) {
        Invoke("StartAudio", delay);
    }
    
    public void SwitchAudioClips(bool gameStarted) {
        
        StartCoroutine(SwitchAudioRoutine(1f, gameStarted));
    }
    
    IEnumerator SwitchAudioRoutine(float secs, bool gameStarted) {
        //lower volume of the current sound
        SetVolume(0.3f);
        yield return new WaitForSeconds(secs);
        //stop it
        StopAudio();
        yield return new WaitForSeconds(secs);
        //change audio clip
        SetAudioClip(gameStarted ? playingAudioClip : playOnLoad);
        //increase volume again
        SetVolume(gameStarted ? 0.3f : 0.7f);
        yield return new WaitForSeconds(secs);
        StartAudio();
    }
}
