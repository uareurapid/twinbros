using UnityEngine;
using System.Collections;

public class BlinkSpriteScript : MonoBehaviour {

    public float delay = 0f;
    public float blinkInterval = 0f;
   
	public bool isUIImage = false;
	private bool canBlink = true;
	public bool isEnabled = true;
	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void OnEnable () {
		InvokeRepeating("Blink",delay,blinkInterval);
	}

	public void Blink() {
		isEnabled = !isEnabled;
		if(isUIImage) {
			UnityEngine.UI.Image image = GetComponent<UnityEngine.UI.Image>();
			image.enabled = isEnabled;
	  	}
	  	else {
			SpriteRenderer image = GetComponent<SpriteRenderer>();
			image.enabled = isEnabled;
	  	}
	}


}
