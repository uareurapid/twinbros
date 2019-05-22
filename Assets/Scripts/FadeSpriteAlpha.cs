using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//fades a sprite or a UI image
public class FadeSpriteAlpha : MonoBehaviour {
 
    public float fadeSpeed = 1f;
    public bool fadeIn = true;
    private SpriteRenderer sprite;
	//private float fadeVal = 1.0f;

	public float minimum = 0.0f;
    public float maximum = 1f;
    public float duration = 5.0f;
    private float startTime;

	private UnityEngine.UI.Image image;
     // Use this for initialization

	public bool disableColliderIfTransparent = false;


	//public bool isTransparent = false;
	//public bool isOpaque = false;
     
    //Collider2D coll;

	Color color;
     // Invisible on Awake
     void Start() {


		 //isTransparent = sprite.color.a < 0.1f;
		 //isOpaque = sprite.color.a > 0.9f;
     }

	 void OnEnable() {

		//coll = GetComponent<Collider2D>();
		sprite = GetComponent<SpriteRenderer>();

		if (sprite == null)
		{
 			sprite = GetComponentInChildren<SpriteRenderer>();
			//color = sprite.color;
		}
		
		if(sprite==null) {
			image = GetComponent<UnityEngine.UI.Image>();
			color = image.color;
		}

		 startTime = Time.time;
	}
     
     // Update is called once per frame
     void Update () {
		
		float t = (Time.time - startTime) / duration;

		//Color c = new Color(1f,1f,1f,Mathf.SmoothStep(minimum,maximum, t));
		if(image!=null) {
			image.color = new Color(1f,1f,1f,Mathf.SmoothStep(minimum,maximum, t));
		}
		

		/*if(disableColliderIfTransparent) {


			if(sprite.color.a > 0.5f) {
		
			  	coll.enabled = true;
			}
			else {
				coll.enabled = false;
			}
		}*/

		//isTransparent = sprite.color.a < 0.1f;
		//isOpaque = sprite.color.a > 0.9f;
             
     }
 }