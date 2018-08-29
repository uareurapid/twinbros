using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

//Player class TODO refactor
public class Movement : MonoBehaviour {

    public float speed = 2.0f;
    Vector3 targetPosition;
    Vector3 old;
    Transform theTransform;

	private bool canMove = true;

	public int maxTilesMovement = 10;

	public bool canMoveLeft = true;
	public bool canMoveRight = true;
	public bool canMoveUp = true;
	public bool canMoveDown = true;

	private bool isLeftMovement = false;
	private bool isRightMovement = false;
	private bool isUpMovement = false;
	private bool isDownMovement = false;

	public Level associatedLevel; //can only move on the associated Level
	private LevelManager levelManager;

	private enum HitDirection { None, Top, Bottom, Forward, Back, Left, Right };

	private float tileSize = 0;
	/************************************************************
     ** Make sure to add rigidbodies to your objects.
     ** Place this script on your object not object being hit
     ** this will only work on a Cube being hit 
     ** it does not consider the direction of the Cube being hit
     ** remember to name your C# script "GetSideHit"
     ************************************************************/

	private Vector2 topLeftBound = Vector2.zero;
	private Vector2 topRightBound = Vector2.zero;
	private Vector2 bottomLeftBound = Vector2.zero;
	private Vector2 bottomRightBound = Vector2.zero;

	private bool reachedTarget = false;

	private Bounds bounds;
	//can move down, up, left? etc?
    void Start()
    {
        targetPosition = transform.position;
        theTransform = transform;
		tileSize = 1f;//bounds.size.x;
		reachedTarget = false;

		GameObject scripts = GameObject.FindGameObjectWithTag("Scripts");
		levelManager = scripts.GetComponent<LevelManager>();

		DelegateHandler.actionDelegate += ReEnableCollidersOnNewLevel;
    	//TODO undelegate on destroy

    }

    void FixedUpdate()
    {

		if(associatedLevel.level != levelManager.currentLevel) {
			Debug.Log("DO NOTHING BECAUSE: associatedLevel.level " + associatedLevel.level + " levelManager.currentLevel? " + levelManager.currentLevel);
			return;
		}

		//targetPosition = transform.position;
        //theTransform = transform;

		if (Input.GetKey(KeyCode.UpArrow) && (theTransform.position == targetPosition || reachedTarget)  && canMoveUp )
        {
            targetPosition += (Vector3.up)*tileSize*maxTilesMovement;
			isUpMovement = true;
			isLeftMovement = isRightMovement = isDownMovement = false;
        }
        else if (Input.GetKey(KeyCode.RightArrow) && (theTransform.position == targetPosition || reachedTarget) && canMoveRight)
        {
            targetPosition += (Vector3.right)*tileSize*maxTilesMovement;
			isRightMovement = true;
			isLeftMovement = isUpMovement = isDownMovement = false;
        }
        else if (Input.GetKey(KeyCode.DownArrow) && (theTransform.position == targetPosition || reachedTarget) && canMoveDown)
        {
            targetPosition += (Vector3.down)*tileSize*maxTilesMovement;
			isDownMovement = true;
			isUpMovement = isLeftMovement = isRightMovement = false;
        }
        else if (Input.GetKey(KeyCode.LeftArrow) && (theTransform.position == targetPosition || reachedTarget) && canMoveLeft)
        {
            targetPosition += (Vector3.left)*tileSize*maxTilesMovement;
			isLeftMovement = true;
			isRightMovement = isDownMovement = isUpMovement = false;
        }

        /*if (Input.GetKey(KeyCode.UpArrow) && tr.position == pos && canMoveUp )
        {
            pos += (Vector3.up)*10;
        }
        else if (Input.GetKey(KeyCode.RightArrow) && tr.position == pos && canMoveRight)
        {
            pos += (Vector3.right)/2;
        }
        else if (Input.GetKey(KeyCode.DownArrow) && tr.position == pos && canMoveDown)
        {
            pos += (Vector3.down)/2;
        }
        else if (Input.GetKey(KeyCode.LeftArrow) && tr.position == pos && canMoveLeft)
        {
            pos += (Vector3.left)/2;
        }*/

		//canMove = canMoveUp || canMoveDown || canMoveLeft || canMoveRight;

		if ( (canMoveUp && isUpMovement) || (canMoveDown && isDownMovement) 
			|| (isLeftMovement && canMoveLeft ) ||	(isRightMovement && canMoveRight) )
		{
			transform.position = Vector3.MoveTowards(transform.position, targetPosition, Time.deltaTime * speed);
		}

    }

	/*void Update() {

		if(isLeftMovement || isRightMovement && Mathf.Abs(transform.position.x - targetPosition.x) < 0.1f) || (isVerticalMovent && Mathf.Abs(transform.position.y - targetPosition.y) < 0.1f)  ){
 			//It is within ~0.1f range, do stuff
			reachedTarget = true;
	}*/
	
    /*void OnCollisionEnter2D(Collision2D other)
    {

		//todo check also y and x in both collisions

		pos = transform.position;
		Vector3 otherPosition = other.transform.position;
		Bounds otherBounds = other.collider.bounds;

		if(topRightBound.x >= otherBounds.min.x  || bottomRightBound.x >= otherBounds.min.x   ) {
			canMoveRight = false;//right collision
			//canMoveLeft = true;
			Debug.Log("right collision");
		}

		if(topRightBound.x <=otherBounds.min.x || bottomRightBound.x <= otherBounds.min.x) {
			canMoveLeft = false;//left collision
			//canMoveRight = true;
			Debug.Log("left collision");
		}

		if(topLeftBound.y >=otherBounds.min.y || topRightBound.y >= otherBounds.min.y) {
			canMoveUp = false;//top collision
			//canMoveDown = true;
			Debug.Log("top collision");
		}

		if(bottomLeftBound.y <= otherBounds.max.y || bottomRightBound.y <= otherBounds.max.y) {
			canMoveDown = false;//down collision
			//canMoveUp = true;
			Debug.Log("down collision");
		}*/

		/*if(otherPosition.x >=pos.x) {
			canMoveRight = false;//right collision
			canMoveLeft = true;
			Debug.Log("right collision");
		}

		if(otherPosition.x <=pos.x) {
			canMoveLeft = false;//left collision
			canMoveRight = true;
			Debug.Log("left collision");
		}

		if(otherPosition.y >=pos.y) {
			canMoveUp = false;//top collision
			canMoveDown = true;
			Debug.Log("top collision");
		}

		if(otherPosition.y <=pos.y) {
			canMoveDown = false;//down collision
			canMoveUp = true;
			Debug.Log("down collision");
		}*/

		//canMove = false;
        /*float x = transform.position.x;
        float y = transform.position.y;
        float z = transform.position.z;

        if ((int)(x + 0.5) > (int)x && x > 0)
            x = (int)x + 0.5f;
        else if((int)(x - 0.5) < (int)x && x < 0)
            x = (int)x - 0.5f;
        else 
            x = (int)x;

        if ((int)(y + 0.5) > (int)y && y > 0)
            y = (int)y + 0.5f;
        else if ((int)(y - 0.5) < (int)y && y < 0)
            y = (int)y - 0.5f;
        else
            y = (int)y;

        z = 0f;

        Vector3 away = new Vector3(x, y, z);
        pos = away;*/
    //}

	public void collidedLeft() {
		Debug.Log("LEFT");
		if(isLeftMovement) {

			if(!reachedTarget) {
				levelManager.decreaseMove();
			}
			
			reachedTarget = true;
			targetPosition = transform.position;
		}
		else {
			reachedTarget = false;
		}
		canMoveLeft = false;
		canMoveRight = true;
	
	}
	public void collidedRight() {
		Debug.Log("RIGHT");
		canMoveRight = false;
		canMoveLeft = true;

		if(isRightMovement) {

			if(!reachedTarget) {
				levelManager.decreaseMove();
			}
			
			reachedTarget = true;
			targetPosition = transform.position;
		}
		else {
			reachedTarget = false;
		}
		
	}
	public void collidedTop() {
		Debug.Log("TOP");
		canMoveUp = false;
		canMoveDown = true;

		if(isUpMovement) {

			if(!reachedTarget) {
				levelManager.decreaseMove();
			}
			reachedTarget = true;
			targetPosition = transform.position;
		}
		else {
			reachedTarget = false;
		}
		
	}
	public void collidedBottom() {
		Debug.Log("BOTTOM");
		canMoveDown = false;
		canMoveUp = true;
	
		if(isDownMovement) {

			if(!reachedTarget) {
				levelManager.decreaseMove();
			}
			reachedTarget = true;
			targetPosition = transform.position;
		}
		else {
			reachedTarget = false;
		}
		
	}

	void OnCollisionExit2D(Collision2D other)
	{

		//if(can)
		canMoveUp = canMoveLeft = canMoveUp = canMoveDown = true;
		reachedTarget = false;
	}

	/*private HitDirection ReturnDirection( GameObject theObject, GameObject ObjectHit ){
         
         HitDirection hitDirection = HitDirection.None;
         RaycastHit MyRayHit;
         Vector3 direction = ( theObject.transform.position - ObjectHit.transform.position ).normalized;
         Ray MyRay = new Ray( ObjectHit.transform.position, direction );
         
         if ( Physics.Raycast( MyRay, out MyRayHit ) ){
                 
             if ( MyRayHit.collider != null ){
                 
                 Vector3 MyNormal = MyRayHit.normal;
                 MyNormal = MyRayHit.transform.TransformDirection( MyNormal );
                 
                 if( MyNormal == MyRayHit.transform.up ){ hitDirection = HitDirection.Top; }
                 if( MyNormal == -MyRayHit.transform.up ){ hitDirection = HitDirection.Bottom; }
                 if( MyNormal == MyRayHit.transform.forward ){ hitDirection = HitDirection.Forward; }
                 if( MyNormal == -MyRayHit.transform.forward ){ hitDirection = HitDirection.Back; }
                 if( MyNormal == MyRayHit.transform.right ){ hitDirection = HitDirection.Right; }
                 if( MyNormal == -MyRayHit.transform.right ){ hitDirection = HitDirection.Left; }
             }    
         }
         return hitDirection;
	}*/

	 /*void OnCollisionEnter2D(Collision2D  collision) 
     {
         Collider2D otherCollider = collision.collider;
         bool collideFromLeft;
         bool collideFromTop;
         bool collideFromRight;
         bool collideFromBottom;
         float RectWidth = GetComponent<Collider2D>().bounds.size.x;
         float RectHeight = GetComponent<Collider2D>().bounds.size.y;
         float circleRad = otherCollider.bounds.size.x;
 
         
             Vector3 contactPoint = collision.contacts[0].point;
             Vector3 center = otherCollider.bounds.center;
 
             if (contactPoint.y > center.y && //checks that circle is on top of rectangle
                 (contactPoint.x < center.x + RectWidth / 2 && contactPoint.x > center.x - RectWidth / 2)) {
                 collideFromTop = true;
				Debug.Log("top collision");
             }
             else if (contactPoint.y < center.y &&
                 (contactPoint.x < center.x + RectWidth / 2 && contactPoint.x > center.x - RectWidth / 2)) {
                 collideFromBottom = true;
				 Debug.Log("bottom collision");
             }
             else if (contactPoint.x > center.x &&
                 (contactPoint.y < center.y + RectHeight / 2 && contactPoint.y > center.y - RectHeight / 2)) {
                 collideFromRight = true;
				 Debug.Log("right collision");
             }
             else if (contactPoint.x < center.x &&
                 (contactPoint.y < center.y + RectHeight / 2 && contactPoint.y > center.y - RectHeight / 2)) {
                 collideFromLeft = true;
				 Debug.Log("left collision");
             }
			 else {
			Debug.Log("NO COLLISION???");
			 }
         
     }*/

	void ReEnableCollidersOnNewLevel() {

		if(associatedLevel.level == levelManager.currentLevel) {
			return; //already done this!!
		}
		Debug.Log("######## ReEnableColliders ######");
		Collider2D[] coll = GetComponents<Collider2D>();
		foreach(Collider2D col in coll) {
			col.enabled = true;
		}

		Collider2D[] coll2 = GetComponentsInChildren<Collider2D>();
		foreach(Collider2D col in coll2) {
			col.enabled = true;
		}

		ResetLevel();
		
	}
	
	void ResetLevel() {

		//allow play/move again
		associatedLevel.level = levelManager.currentLevel;
		reachedTarget = false;
		isUpMovement = isDownMovement = isLeftMovement = isRightMovement = false;
	}

	public void DisableColliders() {
		Collider2D[] coll = GetComponents<Collider2D>();
		foreach(Collider2D col in coll) {
			col.enabled = false;
		}

		Collider2D[] coll2 = GetComponentsInChildren<Collider2D>();
		foreach(Collider2D col in coll2) {
			col.enabled = false;
		}

		reachedTarget = true;
		canMoveLeft = canMoveDown = canMoveUp = canMoveRight = true;
	}

	// Unsubscribing Delegate ALWAYS!!!
    void OnDisable()
    {
      DelegateHandler.actionDelegate -= ReEnableCollidersOnNewLevel;
    }
}
