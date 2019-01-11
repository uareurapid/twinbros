using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FlipSprite : MonoBehaviour {

	//NOTE we flip the entire object not just the sprite
	private Sprite image;
	public bool doOnEnable = false; //if false do on start()
	public float delay = 0f;
	public bool flipX = false;
	// Use this for initialization
	void Start () {
		if(image == null) {
			CheckImage();
		}
		if(image!=null && !doOnEnable) {
			Invoke("Flip", delay);
		}
	}

	void CheckImage() {
		SpriteRenderer rend = GetComponent<SpriteRenderer>();
		if(rend != null) {
			image = rend.sprite;
		}
		else {
			UnityEngine.UI.Image img = GetComponent<UnityEngine.UI.Image>();
			if(img !=null) {
				image = img.sprite;
			}
		}
	}

	void Flip() {
		if(image!=null) {
			if(flipX) {
				transform.localScale = new Vector2(transform.localScale.x * -1,transform.localScale.y);
			}
			else {
				transform.localScale = new Vector2(transform.localScale.x,transform.localScale.y * -1);
			}
		}
	}

	void OnEnable() {
		if(doOnEnable) {
			if(image == null) {
				CheckImage();
			}
			if(image!=null && !doOnEnable) {
				Invoke("Flip", delay);
			}
		}
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
