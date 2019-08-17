using UnityEngine;
using System.Collections;

public class ShakeScript : MonoBehaviour, ResetBehaviourScript {


	//public float shakeSpeed = 1.0f; //how fast it shakes
	public float shakeAmount = 0.055f; //how much it shakes
	public bool shake = false;
	public bool fallAfterShake = false;
	public float forHowLong = 0f;
    public bool shakeOnlyWhenVisible = true;
	//by default do not do it on Start
	public float shakeDelay = 0f;
    
    private bool isVisible = false;

    public bool onlyChangeColor = false;

	private bool changingColor = false;
    
	// Use this for initialization
	void Start () {
	
		if(shakeDelay > 0f && !shakeOnlyWhenVisible) {
        
        
            if(onlyChangeColor) {
                    Invoke("StartColorChange", shakeDelay);
                    if(forHowLong > 0f) {
                        Invoke("StopColorChange", forHowLong);
                    }
            } else {
                Invoke("StartShaking", shakeDelay);
                if(forHowLong > 0f) {
                    Invoke("StopShaking", forHowLong);
                }
            }
			
		}
        
        StartCoroutine(AddToResetableList());
	}
	
	// Update is called once per frame
	void Update () {
	  if(shake && !onlyChangeColor) {

		// Sets the position to be somewhere inside a circle
		// with radius 5 and the center at zero.

		Vector3 position = new Vector3(transform.position.x,transform.position.y,transform.position.z);
		Vector3 newPosition = Random.insideUnitCircle * shakeAmount;
		transform.position = position + newPosition;

	  }
		
	}
	//starts shaking, but will stop in stopDelay seconds
	public void StartShaking(float stopDelay) {

	  //Debug.Log("DEBUG: START SHAKING");
	  shake = true;
	  Invoke("StopShaking",stopDelay);
	}

	public void StartShaking() {
	  shake = true;
	  if(forHowLong > 0f) {
		 Invoke("StopShaking", forHowLong);
	  }
	}
    
    public void StartColorChange() {

	  if(!changingColor) {
		changingColor = true;
		GetComponent<BlinkSpriteScript>().enabled = true;
	  	//Debug.Log("DEBUG: START COLOR CHANGE");
      	if(forHowLong > 0f) {
         	Invoke("StopColorChange", forHowLong);
      	}
	  }
      
    }

	public void StopShaking() {

      shake = false;
      if(fallAfterShake) {

			//Debug.Log("DEBUG: STOP SHAKING");

			FallenTreeScript fall = GetComponent<FallenTreeScript>();
			fall.enabled = true;
            fall.ResetFalling(false);
			fall.StartFalling();
            
            //disable it again
            fall.enabled = false;
            
      }
	}
    
    public void StopColorChange() {

	  if(changingColor) {

			changingColor = false;
	  }
      if(fallAfterShake) {
			//Debug.Log("DEBUG: STOP COLOR CHANGE");
            FallenTreeScript fall = GetComponent<FallenTreeScript>();
            fall.enabled = true;
            fall.ResetFalling(false);
            fall.StartFalling();

            BlinkSpriteScript blink = GetComponent<BlinkSpriteScript>();
            blink.StopBlinking();
            blink.enabled = false;
            
            //disable it again
            fall.enabled = false;
            
      }
    }
    
    void OnBecameVisible() {

		//.Log("DEBUG: beCAME VISIBLE SHAKESCRIPT");
        if (!isVisible) {
		
            isVisible = true;
            //isFalling = false;
            if(shakeOnlyWhenVisible && !shake && !onlyChangeColor) {//start counting
                Invoke("StartShaking", shakeDelay);
                
            } else if(shakeOnlyWhenVisible && onlyChangeColor) {

			   changingColor = false;
               Invoke("StartColorChange", shakeDelay);
            }
        }

    }
    
    void OnBecameInvisible() {
        if (isVisible) {
            
            isVisible = false;
            
        }
    }

    public void ResetOriginalBehaviour()
    {

		changingColor = false;
        //prepare for falling again (if on visible only do the same calls)
        if(shakeDelay > 0f && !shakeOnlyWhenVisible) {
        
            if(onlyChangeColor) {
                Invoke("StartColorChange", shakeDelay);
                if(forHowLong > 0f) {
                    Invoke("StopColorChange", forHowLong);
                }
            } else {
                Invoke("StartShaking", shakeDelay);
                if(forHowLong > 0f) {
                    Invoke("StopShaking", forHowLong);
                }
            }
        
            
        } else if(shakeOnlyWhenVisible && !shake) {
        
               if(onlyChangeColor) {
                 Invoke("StartColorChange", shakeDelay);
               } else {
                Invoke("StartShaking", shakeDelay);
               }
               
        }

    }
    
    IEnumerator AddToResetableList() {
        yield return new WaitForSecondsRealtime(2f);
        GameObject scripts = GameObject.FindGameObjectWithTag("Scripts");
            if(scripts!=null) {

                LevelManager levelManager = scripts.GetComponent<LevelManager>();
                if(levelManager!=null) {
                    levelManager.AddResetableBehaviourObject(this);
                }
                
            }
     }
}
