using UnityEngine;
using System.Collections;

public class FadeSprite : MonoBehaviour {


// animate the game object from 0 to 1 and back
    private float fullyTransparent = 0F;
    private float fullyOpaque =  1F;

	//disable teh collider, allow passing through?
	public bool disableColliderIfTransparent = false;

    // starting value for the Lerp
    //static float interpolater = 0.0f;

	public bool isUIImage = false;
    UnityEngine.UI.Image image;

	private bool fadeIn = false;
	private bool started = false;
	SpriteRenderer spRend;
	Color col;

	public bool doBoth = true;
	private int countCycle = 0;
	// Use this for initialization

	public float duration = 4.0f; //4 seconds

	private bool isDone = false;
	public float speed = 0.01f;

	private Box scriptCaller;
	//the collider
	Collider2D coll;

	void Start () {

		if(isUIImage) {
			image = GetComponent<UnityEngine.UI.Image>();
			col = image.color;
		}
		else {

			spRend = GetComponent<SpriteRenderer>();
			//some might be in children
			if(spRend==null) {
				spRend = GetComponentInChildren<SpriteRenderer>();
			}
			col = spRend.color;
		}
		
		countCycle = 0;
	}
	
	// Update is called once per frame
	void Update () {
	
		if(started) {

			col = isUIImage ? image.color : spRend.color;

			if(fadeIn) {
				// store a reference to the SpriteRenderer on the current GameObject
				
				// copy the SpriteRenderer's color property
				//  change col's alpha value (0 = invisible, 1 = fully opaque)
				//col.a = 0f; // 0.5f = half transparent

				col.a += speed;
	
				//col.a = Mathf.Lerp(fullyOpaque,fullyTransparent, interpolater);
				// change the SpriteRenderer's color property to match the copy with the altered alpha value
				
			}
			else {
				// store a reference to the SpriteRenderer on the current GameObject
				// copy the SpriteRenderer's color property
				
				//  change col's alpha value (0 = invisible, 1 = fully opaque)
				col.a -= speed;
	
				//col.a = 1f; // 0.5f = half transparent
				// change the SpriteRenderer's color property to match the copy with the altered alpha value
	
				// now check if the interpolator has reached 1.0
	        	// and swap maximum and minimum so game object moves
	        	// in the opposite direction.
			        
			}

			if(col.a > 1.0f) {
				col.a = 1.0f;
				isDone = true;
			}
			else if(col.a < 0) {
				col.a = 0;
				isDone = true;
			}

			if(isUIImage) {
				image.color = col;
			}
			else {
				spRend.color = col;
			}

			//completed 1 fade cycle (either in or out)
			if ( isDone )
			{

				if(doBoth && countCycle == 0) {
					countCycle += 1;
					if(fadeIn) {
						Debug.Log("DO 2ND PASS, false");
						FadeSpriteNow(false);
					}
					else {
						//Debug.Log("DO 2ND PASS, true");
						FadeSpriteNow(true);
					}
				}
				else {
					/*
					if(started) {
						Debug.Log("I AM DONE HERE!");
					}*/
					started = false;
					countCycle = 0;
				}

			}
			else {
				//NO, this is not done yet!
				if(disableColliderIfTransparent && coll!=null) {

					//becaming transparent
					if(!fadeIn && col.a < 0.4f && coll.enabled) {
						//Debug.Log("1 OPTION DISABLE HERE ###########################");
						coll.enabled = false;
						if(scriptCaller!=null) {
							scriptCaller.FadeCompletedCallback();
						}
					}
					else if(fadeIn && col.a > 0.6f && !coll.enabled) {
						//becaming opaque

						PlayerMovement player = scriptCaller.GetTwin();
						if(player!=null) {
							if(!player.GetComponent<Collider2D>().IsTouching(coll)) {
									//Debug.Log("2 OPTION RE-ENABLE HERE ###########################");
									coll.enabled = true;
								}
						}

						else if(player==null) {
							//Debug.Log("3 OPTION RE-ENABLE HERE ###########################");
							coll.enabled = true;
						}
					}
				} // end if disableColliderIfTransparent
				
			} //end else !done
	
			
				
			
		}
		

        
		

	}

	public bool IsFadingInOrOut() {
		return started && !isDone;
	}

	public void FadeSpriteNow(bool fadeIn) {
		isDone = false;
		this.fadeIn = fadeIn;
		float startTime = Time.time;
		//speed = (Time.time - startTime) / duration;
		started = true;
	}

	public void FadeSpriteNow(bool fadeIn, Box boxScriptCaller) {
		isDone = false;
		this.scriptCaller = boxScriptCaller;
		this.fadeIn = fadeIn;
		float startTime = Time.time;
		//speed = (Time.time - startTime) / duration;
		coll = scriptCaller.GetColliderBox();
		started = true;
	}
}
