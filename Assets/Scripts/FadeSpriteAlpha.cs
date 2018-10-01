using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//fades a sprite or a UI image
public class FadeSpriteAlpha : MonoBehaviour {
 
    public float fadeSpeed = 1f;
    public bool fadeIn = true;
    private SpriteRenderer sprite;
	private float fadeVal = 1.0f;

	public float minimum = 0.0f;
    public float maximum = 1f;
    public float duration = 5.0f;
    private float startTime;

	private UnityEngine.UI.Image image;
     // Use this for initialization

	public bool disableColliderIfTransparent = false;


	//public bool isTransparent = false;
	//public bool isOpaque = false;
     
    Collider2D coll;
     // Invisible on Awake
     void Start() {

		coll = GetComponent<Collider2D>();
		sprite = GetComponent<SpriteRenderer>();

		if (sprite == null)
		{
 			sprite = GetComponentInChildren<SpriteRenderer>();
		}
		
		if(sprite==null) {
			image = GetComponent<UnityEngine.UI.Image>();
		}

		 startTime = Time.time;

		 //isTransparent = sprite.color.a < 0.1f;
		 //isOpaque = sprite.color.a > 0.9f;
     }
     
     // Update is called once per frame
     void Update () {
		
		float t = (Time.time - startTime) / duration;

		sprite.color = new Color(1f,1f,1f,Mathf.SmoothStep(minimum,maximum, t));

		if(disableColliderIfTransparent) {


			if(sprite.color.a > 0.5f) {
		
			  	coll.enabled = true;
			}
			else {
				coll.enabled = false;
			}
		}

		//isTransparent = sprite.color.a < 0.1f;
		//isOpaque = sprite.color.a > 0.9f;
             
     }
 }