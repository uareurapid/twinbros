using UnityEngine;
using System.Collections;

public class FadeSprite : MonoBehaviour {


// animate the game object from 0 to 1 and back
    private float fullyTransparent = 0F;
    private float fullyOpaque =  1F;

	//disable teh collider, allow passing through?
	public bool disableColliderIfTransparent = false;

    // starting value for the Lerp
    static float interpolater = 0.0f;


	private bool fadeIn = false;
	private bool started = false;
	SpriteRenderer spRend;
	Color col;

	public bool doBoth = true;
	private int countCycle = 0;
	// Use this for initialization
	void Start () {

		spRend = GetComponent<SpriteRenderer>();
		//some might be in children
		if(spRend==null) {
			spRend = GetComponentInChildren<SpriteRenderer>();
		}
		col = spRend.color;
		countCycle = 0;
	}
	
	// Update is called once per frame
	void Update () {
	
		if(started) {

			if(fadeIn) {
				// store a reference to the SpriteRenderer on the current GameObject
				
				// copy the SpriteRenderer's color property
				col = spRend.color;
				//  change col's alpha value (0 = invisible, 1 = fully opaque)
				//col.a = 0f; // 0.5f = half transparent
	
				col.a = Mathf.Lerp(fullyOpaque,fullyTransparent, interpolater);
				// change the SpriteRenderer's color property to match the copy with the altered alpha value
				spRend.color = col;
			}
			else {
				// store a reference to the SpriteRenderer on the current GameObject
				// copy the SpriteRenderer's color property
				col = spRend.color;
				//  change col's alpha value (0 = invisible, 1 = fully opaque)
				col.a = Mathf.Lerp(fullyTransparent, fullyOpaque, interpolater);
	
				//col.a = 1f; // 0.5f = half transparent
				// change the SpriteRenderer's color property to match the copy with the altered alpha value
				spRend.color = col;
	
				// now check if the interpolator has reached 1.0
	        	// and swap maximum and minimum so game object moves
	        	// in the opposite direction.
			        
			}

			if (interpolater > 1.0f || interpolater < 0f)
			{

				if(doBoth && countCycle == 0) {
					countCycle += 1;
					if(fadeIn) {
						FadeSpriteNow(false);
					}
					else {
						FadeSpriteNow(true);
					}
				}
				else {
					started = false;
					interpolater = 0.0f;
					countCycle = 0;
				}

	

			}
	
			if(disableColliderIfTransparent) {
				if(fadeIn && col.a < 0.5) {
					//becaming transparent
					Collider2D coll = GetComponent<Collider2D>();
					coll.enabled = false;
				}
				else if(!fadeIn && col.a > 0.5) {
					Collider2D coll = GetComponent<Collider2D>();
					coll.enabled = true;
				}
			}
				
        	// .. and increase the t interpolater
        	interpolater += 0.25f * Time.deltaTime;
			
		}
		

        
		

	}

	public void FadeSpriteNow(bool fadeIn) {
		this.fadeIn = fadeIn;
		if(fadeIn) {
			
			col.a = 1f;
			interpolater = 0.1f;
		}
		else {
			interpolater = 0.1f;
			col.a = 0f;
		}

		this.started = true;
	}
}
