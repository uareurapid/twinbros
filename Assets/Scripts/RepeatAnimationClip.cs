using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RepeatAnimationClip : MonoBehaviour {

	Animator myAnimator;
	//Put your animation clip here, from within the inspector
	public AnimationClip anim;
	public string triggerName = "Blink";
	public float intervalBetweenPlays = 5f;

	private bool blink = false;

	// Use this for initialization
	void Start () {
		myAnimator = GetComponent<Animator>();
		float waitTime = anim.length + intervalBetweenPlays;
		InvokeRepeating ("PlayAnimation", intervalBetweenPlays, waitTime);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void PlayAnimation () {
		blink = !blink;
		myAnimator.SetBool("Blink",blink);
		
	}
}




 
