using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour {

	public float speed = 2.0f;
    Vector3 targetPosition;
    Vector3 old;
    Transform theTransform;

	private bool canMove = true;

	public int maxTilesMovement = 10;

	public bool isLeftTwin = true;

	public bool canMoveLeft = true;
	public bool canMoveRight = true;
	public bool canMoveUp = true;
	public bool canMoveDown = true;

	private bool isLeftMovement = false;
	private bool isRightMovement = false;
	private bool isUpMovement = false;
	private bool isDownMovement = false;

	public float ignoreCollisionInterval = 0.5f; //ignore if the distance is greater
	public Level associatedLevel; //can only move on the associated Level
	private LevelManager levelManager;

	public PlayerMovement otherTwin;

	private enum HitDirection { None, Top, Bottom, Forward, Back, Left, Right };

	private float tileSize = 0;

	private EnemyBox touchingEnemy = null;

	private bool isMovingBetweenLevels = false;
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

	private bool reachedTarget = true;

	private Bounds bounds;

	private bool reachedNewLevel = false;

	private LevelCheckPoint levelRestrictions; //when reached new Level

	private SwipeDetector swipe;

	private Rigidbody2D body;

	private Vector3[] previousPosition = new Vector3[2];

	private int collisionMasks = -1;
	public float minDistanceForNeighbour = 0.55f; //how close i can be to another element/box
												  //can move down, up, left? etc?

	Animator anim;
	private Sprite originalSprite;
	public Sprite burnedSprite;

	private Vector3 originalPositionInLevel;
	void Awake() {
		originalPositionInLevel = transform.position;
		originalSprite = GetComponentInChildren<SpriteRenderer>().sprite;
		anim = GetComponentInChildren<Animator>();
		collisionMasks = (1 << LayerMask.NameToLayer("Walls") ) | ( 1 << LayerMask.NameToLayer("Boxes") );
	}
    void Start()
    {
		if(Application.platform == RuntimePlatform.IPhonePlayer || Application.platform == RuntimePlatform.Android) {
			swipe = gameObject.AddComponent<SwipeDetector>();
		}

		body = GetComponent<Rigidbody2D>();
        
        //theTransform = transform;
		tileSize = 1f;//bounds.size.x;
		reachedTarget = true;
		targetPosition = transform.position;
		previousPosition[1] = previousPosition[0] = body.position;
		previousPosition[0] = body.position;

		isMovingBetweenLevels = false;

		GameObject scripts = GameObject.FindGameObjectWithTag("Scripts");
		levelManager = scripts.GetComponent<LevelManager>();

		//DelegateHandler.actionDelegate += ReEnableCollidersOnNewLevel;
    	//TODO undelegate on destroy

    }

	public void SetReachedNewLevel(bool reached, LevelCheckPoint newLevelRestrictions) {
		reachedNewLevel = reached;
		levelRestrictions = newLevelRestrictions;
		Debug.Log("### SetReachedNewLevel isLeft? " + isLeftTwin);
		isMovingBetweenLevels = false;
	}

	public bool GetReachedNewLevel() {

		return reachedNewLevel;
	}

	private bool CanMoveOnOppositeDirection() {

		return reachedTarget || targetPosition == transform.position;

	}

	//called from Level manager
	public bool TrySlideUp() {
		if( (transform.position == targetPosition || reachedTarget)  && canMoveUp) {

			SlideUp();
			return true;
		}
		return false;
	}

	public bool TrySlideDown() {
		if((transform.position == targetPosition || reachedTarget) && canMoveDown) {

			SlideDown();
			return true;
			
		}
		return false;
	}

	public bool TrySlideLeft() {
		if((transform.position == targetPosition || reachedTarget) && canMoveLeft) {

			SlideLeft();
			return true;
		}
		return false;
	}

	public bool TrySlideRight() {
		if( (transform.position == targetPosition || reachedTarget) && canMoveRight) {
			SlideRight();
			return true;
		}
		return false;
	}

    void FixedUpdate()
    {

		if(!levelManager.IsGameStarted()) {
			return;
		}

		previousPosition[1] = previousPosition[0];
		previousPosition[0] = body.position;

		if(!isMovingBetweenLevels) {

			//Fire some rays to check if we have anything on the left, right, up or down
			RaycastHit2D hitLeft = Physics2D.Raycast(transform.position, Vector2.left, 2.0f, collisionMasks  );
	        if (hitLeft.collider != null) {
	
				float distance = Mathf.Abs(hitLeft.point.x - transform.position.x);
				if(distance < minDistanceForNeighbour) {
					canMoveLeft = false;
				}
				
	        }
	
			RaycastHit2D hitRight = Physics2D.Raycast(transform.position, Vector2.right, 2.0f, collisionMasks  );
	        if (hitRight.collider != null) {
	
				float distance = Mathf.Abs(hitRight.point.x - transform.position.x);
				if(distance < minDistanceForNeighbour) {
					canMoveRight = false;
				}
			
	        }
	
			RaycastHit2D hitUp = Physics2D.Raycast(transform.position, Vector2.up, 2.0f, collisionMasks  );
	        if (hitUp.collider != null) {
	
				float distance = Mathf.Abs(hitUp.point.y - transform.position.y);
				if(distance < minDistanceForNeighbour) {
					canMoveUp = false;
				}
				
	        }
	
			RaycastHit2D hitDown = Physics2D.Raycast(transform.position, Vector2.down, 2.0f, collisionMasks  );
	        if (hitDown.collider != null) {
	
				float distance = Mathf.Abs(hitDown.point.y - transform.position.y);
				if(distance < minDistanceForNeighbour) {
					canMoveDown = false;
				}
				
	        }
		}
		
		

		//if (Input.GetMouseButtonDown(0) && !levelManager.IsGameStarted())
		//{
			//game not started yet
		//	levelManager.StartGame();
		//}

		if(associatedLevel.level != levelManager.currentLevel.level || isMovingBetweenLevels || levelManager.isPlayerDead() || levelManager.IsStillAwaitingLevelTransitions() ) {
			return;
		}


		//can not move down if still moving up
		if(IsMovingUp() && !CanMoveOnOppositeDirection() ) {
			canMoveDown = false;
		}
		if(IsMovingDown() && !CanMoveOnOppositeDirection() ) {
			canMoveUp = false;
		}

		if(IsMovingLeft() && !CanMoveOnOppositeDirection() ) {
			canMoveRight = false;
		}

		if(IsMovingRight() && !CanMoveOnOppositeDirection() ) {
			canMoveLeft = false;
		}
		
		/**
		if ( (Input.GetKey(KeyCode.UpArrow) || swipe!=null && swipe.upSwipe ) )
        {


			if( (transform.position == targetPosition || reachedTarget)  && canMoveUp) {

				SlideUp();
			}
			else {

				//Debug.Log("NO CAN MOVE UP? " + canMoveUp + " RECAHED TARGET? " + reachedTarget);
			}

			
        }*/
        /*else if ( (Input.GetKey(KeyCode.RightArrow)|| swipe!=null && swipe.rightSwipe)  ) 
        {
			//Debug.Log("AM INSIDE, can move right? " + canMoveRight);
			//Debug.Log("AM INSIDE, reachedTarget? " + reachedTarget);
			//Debug.Log("AM INSIDE, transform.position == targetPosition? " + (transform.position == targetPosition));
			//do not let move to right and kill right away
			 /*if(touchingEnemy != null && touchingEnemy.killPlayerOnTouch && touchingEnemy.GetTile().blockRightMovement) {
				levelManager.KillPlayer();
				return;
			 }*/

			 //if( (transform.position == targetPosition || reachedTarget) && canMoveRight) {
			//	SlideRight();

			 //}
			 //else {

				//Debug.Log("NO CAN MOVE RIGHT? " + canMoveRight + " RECAHED TARGET? " + reachedTarget + " canMoveRight? " + canMoveRight);
			 //}

			
        //}*/
        /**else if ( (Input.GetKey(KeyCode.DownArrow) || swipe!=null && swipe.downSwipe ) )
        {

			if((transform.position == targetPosition || reachedTarget) && canMoveDown) {

				SlideDown();
			
			}
			else {

				//Debug.Log("NO CAN MOVE DOWN? " + canMoveDown + " RECAHED TARGET? " + reachedTarget);
			}
			
        }*/
        /*else if ( (Input.GetKey(KeyCode.LeftArrow) || swipe!=null && swipe.leftSwipe ) )
        {  

			if((transform.position == targetPosition || reachedTarget) && canMoveLeft) {

				SlideLeft();
			}
			else {
				//Debug.Log("NO CAN MOVE LEFT? " + canMoveLeft + " RECAHED TARGET? " + reachedTarget);
			}
			
			//if(associatedLevel.level == 2)Debug.Log("WILL MOVE LEFT");
        }*/


		//canMove = canMoveUp || canMoveDown || canMoveLeft || canMoveRight;

		if ( (canMoveUp && isUpMovement) || (canMoveDown && isDownMovement) 
			|| (isLeftMovement && canMoveLeft ) ||	(isRightMovement && canMoveRight) )
		{
			reachedTarget = false;

			if(isLeftMovement && canMoveLeft) {
				//only move in x
				transform.position = Vector3.MoveTowards(transform.position, new Vector3(targetPosition.x,transform.position.y,transform.position.z), Time.deltaTime * speed);
			}
			if(isRightMovement && canMoveRight) {
				//x
				transform.position = Vector3.MoveTowards(transform.position, new Vector3(targetPosition.x,transform.position.y,transform.position.z), Time.deltaTime * speed);
			}
			if(isUpMovement && canMoveUp) {
				//y
				transform.position = Vector3.MoveTowards(transform.position, new Vector3(transform.position.x,targetPosition.y,transform.position.z), Time.deltaTime * speed);
			}
			if(isDownMovement && canMoveDown) {
				//y
				transform.position = Vector3.MoveTowards(transform.position, new Vector3(transform.position.x,targetPosition.y,transform.position.z), Time.deltaTime * speed);
			}
			
			
			

			
			//transform.position = Vector3.MoveTowards(transform.position, targetPosition, Time.deltaTime * speed);
		}

    }

	void SlideUp() {
		isUpMovement = true;
		reachedTarget = false;
        targetPosition += (Vector3.up)*tileSize*maxTilesMovement;
		isLeftMovement = isRightMovement = isDownMovement = false;
		SoundEffectsHelper.Instance.PlayMoveSound();
	}

	void SlideRight() {
		isRightMovement = true;
		reachedTarget = false;
		targetPosition += (Vector3.right)*tileSize*maxTilesMovement;
		isLeftMovement = isUpMovement = isDownMovement = false;
		SoundEffectsHelper.Instance.PlayMoveSound();
	}

	void SlideDown() {
		isDownMovement = true;
		reachedTarget = false;
        targetPosition += (Vector3.down)*tileSize*maxTilesMovement;
		isUpMovement = isLeftMovement = isRightMovement = false;
		SoundEffectsHelper.Instance.PlayMoveSound();
	}

	void SlideLeft() {

		isLeftMovement = true;
		reachedTarget = false;
        targetPosition += (Vector3.left)*tileSize*maxTilesMovement;
		isRightMovement = isDownMovement = isUpMovement = false;
		SoundEffectsHelper.Instance.PlayMoveSound();
	}


	public bool IsMovingLeft() {
		return isLeftMovement;
	}

	public bool IsMovingRight() {
		return isRightMovement;
	}

	public bool IsMovingUp() {
		return isUpMovement;
	}

	public bool IsMovingDown() {
		return isDownMovement;
	}

	public void collidedLeft() {
		Debug.Log("LEFT " + (isLeftTwin ? " left twin " : "right twin"));
		canMoveLeft = false;
		//canMoveRight = true;

		if(isLeftMovement) {

			
			StopMovementVelocity();
			canMoveRight = true;
			canMoveUp = canMoveDown = canMoveRight = true;
			//transform.position = previousPosition[1];
			transform.Translate(-body.velocity);

			if(!reachedTarget) {

				reachedTarget = true;

				if(isLeftTwin) {
					levelManager.MoveLeftTwin();
				}
				else {
					levelManager.MoveRightTwin();
				}
			}
			
			reachedTarget = true;
			targetPosition = transform.position;

		}
		else {
			reachedTarget = false;
		}

	
	}
	public void collidedRight() {
		Debug.Log("RIGHT " + (isLeftTwin ? " left twin " : "right twin") );
		canMoveRight = false;
		//canMoveLeft = true;

		if(isRightMovement) {

			StopMovementVelocity();
			canMoveLeft = true;
			canMoveLeft = canMoveUp = canMoveDown = true;
			transform.Translate(-body.velocity);

			if(!reachedTarget) {

				reachedTarget = true;
				if(isLeftTwin) {
					levelManager.MoveLeftTwin();
				}
				else {
					levelManager.MoveRightTwin();
				}

				//levelManager.decreaseMove();
			}
			
			reachedTarget = true;
			targetPosition = transform.position;
		}
		else {
			reachedTarget = false;
		}
		
	}
	public void collidedTop() {
		Debug.Log("TOP" + (isLeftTwin ? " left twin " : "right twin"));
		canMoveUp = false;
		//canMoveDown = true;

		if(isUpMovement) {

			StopMovementVelocity();
			canMoveDown = true;
			canMoveDown = canMoveLeft = canMoveRight = true;
			//transform.position = previousPosition[1];
			transform.Translate(-body.velocity);

			if(!reachedTarget) {

				reachedTarget = true;
				if(isLeftTwin) {
					levelManager.MoveLeftTwin();
				}
				else {
					levelManager.MoveRightTwin();
				}
			}
			reachedTarget = true;
			targetPosition = transform.position;
		}
		else {
			reachedTarget = false;
		}
		
	}
	public void collidedBottom() {
		Debug.Log("BOTTOM" + (isLeftTwin ? " left twin " : "right twin"));
		canMoveDown = false;
		//canMoveUp = true;
	
		if(isDownMovement) {

			StopMovementVelocity();
			canMoveUp = true;
			canMoveUp = canMoveLeft = canMoveRight = true;
			//transform.position = previousPosition[1];
			transform.Translate(-body.velocity);

			if(!reachedTarget) {
				
				reachedTarget = true;
				if(isLeftTwin) {
					levelManager.MoveLeftTwin();
				}
				else {
					levelManager.MoveRightTwin();
				}
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

		if(other.transform.CompareTag("Bomb")) {

			//avoid getting killed by the bomb
		}
		/*Debug.Log("EXIT COLLISION WITH SOMETHING " + other.transform.tag);
		Tile tile = other.transform.GetComponent<Tile>();
		if(tile!=null) {
			Debug.Log("IT IS A TILE, LET HIM HANDLE THE EXIT ");
			tile.HandleTileExitCollisions(this);
		}*/
		//AllowAllMovementsAgain();
		
	}

	public Rigidbody2D GetBody() {
		return body;
	}

    void OnCollisionEnter2D(Collision2D other)
	{
		string otherTag = other.transform.tag;
		bool isPortal = otherTag.Equals("Portal");
		bool isEnemy = otherTag.Equals("Enemy");
		bool isBox = otherTag.Equals("Box");
		bool isBomb = otherTag.Equals("Bomb");

		Tile tile = other.transform.GetComponent<Tile>();
		if(tile!=null) {
			//Debug.Log("IT IS A TILE, LET HIM HANDLE IT ");
			tile.HandlePlayerCollision(this);
		}
		//portal collision
		else if (isPortal)
		{

			if (isMovingBetweenLevels)
			{
				//ignore this collision
				return;
			}
			else {
				//TODO keep coding me
				levelManager.TwinCollidedWithPortal(gameObject);
				Portal portal = other.gameObject.GetComponent<Portal>();
				portal.MoveToNextLevel();
			}

		}
		else {

			Debug.Log("IS SOMETHING ELSE COLLIDING " +  other.transform.tag);
			if(isRightMovement) {
				Debug.Log("BLOCK FURTHER RIGHT MOVEMENT");
				if (Mathf.Abs(other.transform.position.y - transform.position.y) < ignoreCollisionInterval) {
					collidedRight();
				}
				
			}
		
			else if(isLeftMovement) {
				Debug.Log("BLOCK FURTHER LEFT MOVEMENT");
				if (Mathf.Abs(other.transform.position.y - transform.position.y) < ignoreCollisionInterval)
				{
					collidedLeft();
				}
				
			}
			else if(isUpMovement) {
				Debug.Log("BLOCK FURTHER UP MOVEMENT");

				//otherwise just ignore this one
				if( Mathf.Abs(other.transform.position.x - transform.position.x) < ignoreCollisionInterval) {
					collidedTop();
				}
					
			}
			else if(isDownMovement) {
				Debug.Log("BLOCK FURTHER DOWN MOVEMENT");
				if (Mathf.Abs(other.transform.position.x - transform.position.x) < ignoreCollisionInterval)
				{
					collidedBottom();
					
				}
				
			}

			if(isEnemy) {
				 EnemyBox enemy = other.gameObject.GetComponent<EnemyBox>();
				 enemy.HandlePlayerCollision();
			}
			else if(isBomb) {

				Bomb bomb = other.gameObject.GetComponent<Bomb>();
				bomb.HandlePlayerCollision(this);
			}	
			else if(isBox) {
				Box box = other.gameObject.GetComponent<Box>();
				//TODO there are game objects that are tagged box, but do not have the component CHECK!!!
				if(box!=null && box.isSurpriseBox) {

					Debug.Log("FADE SURPRISE BOX");
					FadeSpriteAlpha fade = box.gameObject.GetComponent<FadeSpriteAlpha>();
						if(fade!=null) {
							fade.enabled = true;
						}
				}
			}	

			
		}
		
		
	}

	public void AllowAllMovementsAgain() {

        targetPosition = transform.position;
		canMoveUp = !canMoveUp;
		canMoveLeft = !canMoveLeft;
		canMoveRight = !canMoveRight;
		canMoveDown = !canMoveDown;

		//apply any new level initial restrictions
		if(levelRestrictions!=null) {

			canMoveUp = levelRestrictions.canMoveUp;
			canMoveLeft = levelRestrictions.canMoveLeft;
			canMoveRight = levelRestrictions.canMoveRight;
			canMoveDown = levelRestrictions.canMoveDown;
		}

		isUpMovement = isDownMovement = isRightMovement = isLeftMovement = false;
		reachedTarget = false;

	}

	public void AllowUpMovementAgain()
	{

		canMoveUp = true;
		//TODO these ones sould be wrong
		//if(IsMovingUp()) {
		//	reachedTarget = true;
		//}
	}

	public void AllowDownMovementAgain()
	{

		canMoveDown = true;
		//if(IsMovingDown()) {
		//	reachedTarget = true;
		//}
	}

	public void AllowLeftMovementAgain()
	{

		canMoveLeft = true;
		//if(IsMovingLeft()) {
		//	reachedTarget = true;
		//}
	}

	public void AllowRightMovementAgain()
	{

		canMoveRight = true;
		//if(IsMovingRight()) {
		//	reachedTarget = true;
		//}
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

	public void ResetPlayerOnNewLevel() {

		Debug.Log("###### ResetPlayerOnNewLevel ON isLeft? " + isLeftTwin);
		if(isMovingBetweenLevels == false && reachedNewLevel == false) {
			Debug.Log("#### Already done this!! is left?" + isLeftTwin);
			return; //already done this!!
		}
		originalPositionInLevel = transform.position; //this becames the original position on level
		AllowAllMovementsAgain();
		EnableColliders();
		ResetLevel();
		//AllowAllMovementsAgain();
		
	}

	public void SetIsTouchingEnemy(EnemyBox touching) {

		touchingEnemy = touching;
	}

	public EnemyBox GetIsTouchingEnemy() {

		return touchingEnemy;
	}
	
	//sets the new Level on the player
	void ResetLevel() {

		Debug.Log("###### ResetLevel ON isLeft? " + isLeftTwin);
		//allow play/move again
		associatedLevel = levelManager.currentLevel;
		reachedTarget = true;
		targetPosition = transform.position;
		//TODO added this prop
		reachedNewLevel = false;
		isMovingBetweenLevels = false;
	}

	void EnableColliders() {

		Collider2D[] coll = GetComponents<Collider2D>();
		foreach(Collider2D col in coll) {
			col.enabled = true;
		}

		Collider2D[] coll2 = GetComponentsInChildren<Collider2D>();
		foreach(Collider2D col in coll2) {
			col.enabled = true;
		}
	}

    //this is called when is flying to another level
	public void DisableColliders() {

		Debug.Log("DISABLED COLLIDER ON isLeft? " + isLeftTwin);
		isMovingBetweenLevels = true;
		reachedTarget = false;
		canMoveLeft = canMoveDown = canMoveUp = canMoveRight = false; //TODO was true
		isUpMovement = isDownMovement = isLeftMovement = isRightMovement = false;

		Collider2D[] coll = GetComponents<Collider2D>();
		foreach(Collider2D col in coll) {
			col.enabled = false;
		}

		Collider2D[] coll2 = GetComponentsInChildren<Collider2D>();
		foreach(Collider2D col in coll2) {
			col.enabled = false;
		}

		
		
	}

	void StopMovementVelocity() {

		Rigidbody2D body = GetComponent<Rigidbody2D>();
		if(body!=null) {
			body.velocity = Vector3.zero;
		}
	}

	public void SetIsMovingBetweenLevels(bool moving) {

		isMovingBetweenLevels = moving;
		if(isMovingBetweenLevels) {
			//disable the rigidbody
			StopMovementVelocity();
			//disable all the colliders, parent and children
			DisableColliders();

		}
		
	}

	public bool GetIsMovingBetweenLevels() {

		return isMovingBetweenLevels;
	}

	// Unsubscribing Delegate ALWAYS!!!
    void OnDisable()
    {
      //DelegateHandler.actionDelegate -= ReEnableCollidersOnNewLevel;
    }


	public void ShowBurnSpriteAnimation() {
		anim.enabled = false;
		GetComponentInChildren<SpriteRenderer>().sprite = burnedSprite;
		levelManager.KillPlayer();
	}

	public void ResetOriginalSprite() {

		GetComponentInChildren<SpriteRenderer>().sprite = originalSprite;
		anim.enabled = true;
	}

	public void ResetOriginalPosition() {
		transform.position = originalPositionInLevel;
	}
}
