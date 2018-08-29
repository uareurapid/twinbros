using UnityEngine;
using System.Collections;

public class MoveTowardsScript : MonoBehaviour {

    //if there is a tag, find the associated game object
    public string targetTag;

    //either specify an object or just a vector 3 position
	public Transform target;
	public Vector3 targetPosition;

	public bool startMoveTowards = false;
	public float moveTowardsSpeed = 1.3f;
	private bool reachedTarget = false;
	public bool adjustYOnStart = false;
	public bool adjustXOnStart =  false;
	public bool adjustZOnStart =  true;
	public bool destroyWhenReach = false;
	//in this case the movement is not automatic, but manual
	public bool allowManualMovement = true;

	private bool targetIsOnTheRight = false;

	public bool adjustExactFinalPosition = true; //if true put right on target position, on reach target true

	public bool isVerticalMovent = false;

	public float delay = 0f;
	public bool objectInCanvasUI = false; //need to convert coords

	public Transform secondTarget; //another one to go back and fourth
	public bool goBackAndFourth = false;
	private bool isGoingBack = false;
	private Vector3 secondTargetPosition;
	// Use this for initialization
	void Start () {

	  if(delay > 0f) {
			Invoke("CheckPositions", delay);
	  }
	  else {
		  CheckPositions();
	  }
	  

	}

	void CheckPositions() {

	 if(targetTag!=null && target==null) {
	      target = GameObject.FindGameObjectWithTag(targetTag).transform; 
	 }

	 if(target!=null) {

		if(objectInCanvasUI) {
		    

			//Get the location of the UI element you want the 3d onject to move towards
             Vector3 screenPoint = target.transform.position + new Vector3(0,0,5);  //the "+ new Vector3(0,0,5)" ensures that the object is so close to the camera you dont see it
             
             //find out where this is in world space
             Vector3 worldPos = Camera.main.ScreenToWorldPoint( screenPoint );

				//move towards the world space position
				targetPosition = target.position; //worldPos;// Vector3.MoveTowards(transform.position, worldPos, currentMoveSpeed)
		}
		else {
			targetPosition = target.position;
			if(!adjustZOnStart) {
				targetPosition.z = transform.position.z;
			}
		}
	 }

	  if(secondTarget!=null && goBackAndFourth) {
		 secondTargetPosition = secondTarget.transform.position;
	  }
	}
	
	// Update is called once per frame
	void Update () {

	if(target == null || targetPosition == null) {
		return;
	}

	//check conditions to re-enable tracking again
	if(reachedTarget) {
		if ((!isVerticalMovent && Mathf.Abs(transform.position.x - targetPosition.x) > 0.1f) || (isVerticalMovent && Mathf.Abs(transform.position.y - targetPosition.y) > 0.1f))
		{
			reachedTarget = false;
		}
	}
	
     //move towrads a position or a specific object
	 if(startMoveTowards && !reachedTarget) {

		 if(!allowManualMovement) {

			// The step size is equal to speed times frame time.
		 	var step = moveTowardsSpeed * Time.deltaTime;
		 	// Move our position a step closer to the target.
		 	transform.position = Vector3.MoveTowards(transform.position, targetPosition, step);

			if(transform.position.x < target.position.x) {

				if(!targetIsOnTheRight) {
						FlipSprite();
						
				}
				targetIsOnTheRight = true;
			}
			else {

				if(targetIsOnTheRight) {
						FlipSprite();
				}
				targetIsOnTheRight = false;
			}

			if(target!=null){
				targetPosition = target.position;
			}


		 }

		if(  (!isVerticalMovent && Mathf.Abs(transform.position.x - targetPosition.x) < 0.1f) || (isVerticalMovent && Mathf.Abs(transform.position.y - targetPosition.y) < 0.1f)  ){
 			//It is within ~0.1f range, do stuff
			reachedTarget = true;
			if(isGoingBack) {
				isGoingBack = false;
			}
			Debug.Log("Reached position");
			if(destroyWhenReach) { //destroy this object
			  Destroy(gameObject);
			}
			else if(goBackAndFourth && secondTarget!=null) {

			   if(!isGoingBack) {
					Vector3 inicial = targetPosition;
					targetPosition = secondTargetPosition;
					secondTargetPosition = inicial;
					isGoingBack = true;
				}

				reachedTarget = false;
				
			}
 		}
		else {
			reachedTarget = false;
		}
	 }

	 //put exactly in place
	  if(reachedTarget) {

			if(adjustExactFinalPosition && transform.position != targetPosition) {
				transform.position = targetPosition;
			}
			
			//notify the handler that we reached target
			DelegateHandler actionHandler = GetComponent<DelegateHandler>();
			if(actionHandler!=null) {
				actionHandler.ActionCompleted();
			}
	  }

	}

	void FlipSprite() {
		SpriteRenderer sprite = transform.GetComponent<SpriteRenderer>();
		if(sprite==null) {
			sprite = transform.GetComponentInChildren<SpriteRenderer>();
		}
		if(sprite!=null) {
			sprite.flipX = !sprite.flipX;
		}
	}
	

	public void StartMovingTowards(bool start) {
		startMoveTowards = start;

		if(targetPosition == null && target !=null) {
			targetPosition = target.position;
		}
		//*********************************************************
		Vector3 aux = new Vector3 (transform.position.x, transform.position.y, transform.position.z);
		//maybe adjust to make smoother transitions, and avoid jumps if in top of platform collider...
		if (adjustYOnStart) {
			aux.y = targetPosition.y;
		}
		
		if (adjustZOnStart) {
			aux.z = targetPosition.z;
		}
		
		if (adjustXOnStart) {
			aux.x = targetPosition.x;
		}
		
		transform.position = aux;
		//*********************************************************

		/*bool isOnLeft = transform.position.x < target.position.x;
		if(isOnLeft && !player.IsPlayerFacingRight()) {
		  player.Flip();
		}
		else if(!isOnLeft && player.IsPlayerFacingRight()) { //is on the right and facing right, also need to flip
		  player.Flip();
		}*/
	}

	public bool HasReachedTarget() {
	  return reachedTarget;
	}

	void OnEnable() {
		StartMovingTowards(true);
	}

}
