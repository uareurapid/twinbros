using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SliderBlock : MonoBehaviour {

	public bool slideLeft = false;
	public bool slideRight = false;
	public bool slideUp = false;
	public bool slideDown = false;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void Slide(PlayerMovement player) {

		if(slideRight) {
			player.canMoveRight = true;
			player.SlideRight();
		}
	}
}	
