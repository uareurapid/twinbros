using UnityEngine;
using System.Collections;

public class SwapSpriteScript : MonoBehaviour {

	public float swapInterval = 2.0f;
	public Sprite[] sprites;
	private int lastUsedSprite = 0;
	public bool canSwap = true;
	private float lastSwapTime=0;
	public float swapDelay = 0f;
	public float maxSwaps = 0; //0 means forever
	private int numSwaps = 0;

	public bool isController = false; 
	public float controllerSwitchDelay = 0.5f;//only applies if is controller
	
	// Use this for initialization
	void Start () {
		lastUsedSprite = 0;
		numSwaps = 0;
		lastSwapTime=0;
		if (swapDelay > 0f && canSwap) {
			Invoke ("AllowSwap", swapDelay);
		}
		//else {
		//	canSwap = true;
		//}
	}
	
	// Update is called once per frame
	void Update () {
	
		if (canSwap) {

			lastSwapTime += Time.deltaTime;

			if( lastSwapTime >= swapInterval ) {
				//time to swap images
				
				IncreaseSpriteIndex();		
				SwapSprites();
				numSwaps +=1;
				lastSwapTime = 0f;

				if(maxSwaps > 0 && numSwaps >= maxSwaps) {
				  canSwap = false;
				}
			}
		}
	  

		

	}
	//swap time
	public void SwapSprites() {
		
		SpriteRenderer renderer = gameObject.GetComponent<SpriteRenderer>();
		renderer.sprite = sprites[lastUsedSprite];
		//if is controller, swicth it at the same time of the sprite swap
		if (isController) {
			Invoke("SwitchController",controllerSwitchDelay);
		}
		
	}

	public void IncreaseSpriteIndex() {
		lastUsedSprite+=1;
		if(lastUsedSprite==sprites.Length) {
			lastUsedSprite = 0;
		}
	}
	
	//call directly
	public void SwapSprites(int index) {
		
		if(index<sprites.Length) {
			SpriteRenderer renderer = gameObject.GetComponent<SpriteRenderer>();
			renderer.sprite = sprites[index];
		}
		
		
	}

	void SwitchController() {
		//ControllerScript controller = gameObject.GetComponent<ControllerScript>();
		//controller.Switch();
	}
	
	public bool CanSwap() {
	
	     return canSwap;
	}
	
	public void BlockSwap(bool block) {
		
		canSwap = block;
	}

	public void AllowSwap() {
		
		canSwap = true;
	}
}
