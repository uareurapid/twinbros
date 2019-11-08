using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour {

	public float speed = 2.0f;
    Vector3 targetPosition;
    Vector3 old;
    Transform theTransform;

	Vector3 initialScale;
	Quaternion initialRotation;

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

	public float ignoreCollisionInterval = 0.15f; //ignore if the distance is greater
	public Level associatedLevel; //can only move on the associated Level
	private LevelManager levelManager;

	public PlayerMovement otherTwin;

    public Renderer playerBoxRenderer;

	private enum HitDirection { None, Top, Bottom, Forward, Back, Left, Right };

	private float tileSize = 0;

	private EnemyBox touchingEnemy = null;

	private bool isMovingBetweenLevels = false;
	private bool isMovingBetweenTeleportPoints = false;
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

    //the masks are used only for raycast, things that i can walk through like sliders/doors, etc MUST BE OUT!
	private int collisionMasks = -1;
	public float minDistanceForNeighbour = 0.55f; //how close i can be to another element/box //was 0.55 on editor
			
	//can move down, up, left? etc?
                                      
    public Transform deathWings;                                  

	Animator anim;
	private Sprite originalSprite;
	public Sprite burnedSprite;
	public Sprite electrocutedSprite;

	private Vector3 originalPositionInLevel;

	//for better collision checks
	float playerWidth = 0; 
	float playerHeight = 0;
	float playerLeft = 0;
	float playerRight = 0;
	float playerTop = 0;
	float playerBottom = 0;

	private Transform transportBubble;


    
    

	void Awake() {
		originalPositionInLevel = transform.position;
		originalSprite = GetComponentInChildren<SpriteRenderer>().sprite;
		anim = GetComponentInChildren<Animator>();
		collisionMasks = (1 << LayerMask.NameToLayer("Walls") ) | ( 1 << LayerMask.NameToLayer("Boxes") );
	}

	void CheckBounds() {

		playerWidth = playerBoxRenderer.bounds.size.x;
		playerHeight = playerBoxRenderer.bounds.size.y;
		playerLeft = playerBoxRenderer.bounds.center.x - (playerWidth / 2);
		playerRight = playerBoxRenderer.bounds.center.x + (playerWidth / 2);
		playerTop = playerBoxRenderer.bounds.center.y - (playerHeight / 2);
		playerBottom = playerBoxRenderer.bounds.center.y + (playerHeight / 2);
	}
    void Start()
    {
		body = GetComponent<Rigidbody2D>();
        body.isKinematic = false; //should be true
		body.gravityScale = 0;

		if(Application.platform == RuntimePlatform.IPhonePlayer || Application.platform == RuntimePlatform.Android) {
			swipe = gameObject.AddComponent<SwipeDetector>();
		}

        playerBoxRenderer = GetComponentInChildren<Renderer>();
		CheckBounds();

		//find the bubble
        /*
		int childs = transform.childCount;
		for (int i = 0; i < childs; i++)
		{
			Transform child = transform.GetChild(i);
			if (child.gameObject.name.Equals("bubble"))
			{
				transportBubble = child;
				EnableOrDisableTransportBubble();
				break;
			}
		}*/

		initialScale = transform.localScale;
		initialRotation = transform.localRotation;

        //theTransform = transform;
		tileSize = 1f;//bounds.size.x;
		reachedTarget = true;
		targetPosition = transform.position;
		previousPosition[1] = previousPosition[0] = body.position;
		previousPosition[0] = body.position;

		isMovingBetweenLevels = false;

		GameObject scripts = GameObject.FindGameObjectWithTag("Scripts");
		levelManager = scripts.GetComponent<LevelManager>();

        //find the bubble
        int childs = transform.childCount;
        for (int i = 0; i < childs; i++)
        {
            Transform child = transform.GetChild(i);
            if (child.gameObject.name.Equals("bubble"))
            {
                transportBubble = child;
                EnableOrDisableTransportBubble();
                break;
            }
        }
        
		ResetPlayer();
		//DelegateHandler.actionDelegate += ReEnableCollidersOnNewLevel;
    	//TODO undelegate on destroy

    }
    
    public LevelManager GetLevelManager() {
        return levelManager;
    }
    
    //END TWIN BONUS

	public void ResetPlayer() {
		AutoRotate rotate = GetComponent<AutoRotate>();
		if(rotate!=null) {
			rotate.enabled = false;
		}
		transform.localScale = initialScale;
		transform.localRotation = initialRotation;
	}
	public void SetReachedNewLevel(bool reached, LevelCheckPoint newLevelRestrictions) {
		reachedNewLevel = reached;
		levelRestrictions = newLevelRestrictions;
		isMovingBetweenLevels = false;
		EnableOrDisableTransportBubble();
	}

	public bool GetReachedNewLevel() {

		return reachedNewLevel;
	}

	private bool CanMoveOnOppositeDirection() {

		return reachedTarget || targetPosition == transform.position;

	}

	//called from Level manager
	public bool TrySlideUp() {
		if( (transform.position == targetPosition || reachedTarget)  && canMoveUp && !isMovingBetweenLevels && !isMovingBetweenTeleportPoints) {

			SlideUp();
			return true;
		}
		return false;
	}

	public bool TrySlideDown() {

		
		if((transform.position == targetPosition || reachedTarget) && canMoveDown && !isMovingBetweenLevels && !isMovingBetweenTeleportPoints) {

			SlideDown();
			return true;
			
		}
		return false;
	}

	public bool TrySlideLeft() {
		//Debug.Log("TRY SLIDE LEFT");
		if((transform.position == targetPosition || reachedTarget) && canMoveLeft && !isMovingBetweenLevels && !isMovingBetweenTeleportPoints) {
			
			SlideLeft();
			return true;
		}
		//Debug.Log("LEFT ONE? " + isLeftTwin + " I CANNOT!!!" + "transform.position == targetPosition?" + (transform.position == targetPosition) + " reachedTarget? " + reachedTarget + " canMoveLeft? " + canMoveLeft) ;
		return false;

	}

	public bool TrySlideRight() {
		if( (transform.position == targetPosition || reachedTarget) && canMoveRight && !isMovingBetweenLevels && !isMovingBetweenTeleportPoints) {
			SlideRight();
			return true;
		}
		return false;
	}

	public bool IsStopped() {
		//whne i movetowards it does not use physics, but the transform position directly, so the velocity is always zero, at least until it collides with something
		//NOT GOODreturn ( ( body.velocity == Vector2.zero || !IsMovingInAnyDirection()  ) && reachedTarget );
		return !IsMovingInAnyDirection();//(body.velocity == Vector2.zero) || 

        //TODO NOTE, before was only checking velocity, but this is wrong anyway
    }

    void FixedUpdate()
    {

		if(!levelManager.IsGameStarted() || levelManager.IsPlayerDead() || levelManager.IsAboutToDie()) {
			return;
		}

		previousPosition[1] = previousPosition[0];
		previousPosition[0] = body.position;

		//only check for raycast hits if not movement is blocked
		if(!isMovingBetweenLevels && !isMovingBetweenTeleportPoints) {

			//Fire some rays to check if we have anything on the left, right, up or down
			RaycastHit2D hitLeft = Physics2D.Raycast(transform.position, Vector2.left, 2.0f, collisionMasks  );
	        if (hitLeft.collider != null) {
	
				float distance = Mathf.Abs(hitLeft.point.x - transform.position.x);
				if(distance < minDistanceForNeighbour && (hitLeft.point.x <= transform.position.x)) {
					
					if(canMoveLeft && IsStopped() && !isUpMovement && !isDownMovement && !isRightMovement) {
						canMoveLeft = false;
						
						if(isLeftMovement) {
							collidedLeft();
						}
					}
					
				}
				
	        }
	
			RaycastHit2D hitRight = Physics2D.Raycast(transform.position, Vector2.right, 2.0f, collisionMasks  );
	        if (hitRight.collider != null) {
	
				float distance = Mathf.Abs(hitRight.point.x - transform.position.x);//make sure it is on the right of the player
				if(distance < minDistanceForNeighbour && (hitRight.point.x >= transform.position.x )) {
					
					if(canMoveRight && IsStopped() && !isUpMovement && !isDownMovement && !isLeftMovement) {
                        canMoveRight = false;
						if(isRightMovement) {
							collidedRight();
						}
					}
					
				}
			
	        }
	
			RaycastHit2D hitUp = Physics2D.Raycast(transform.position, Vector2.up, 2.0f, collisionMasks  );
	        if (hitUp.collider != null) {
	
				float distance = Mathf.Abs(hitUp.point.y - transform.position.y);
				if(distance < minDistanceForNeighbour && (hitUp.point.y >= transform.position.y)) {
						
					if(canMoveUp && IsStopped() && !isLeftMovement && !isRightMovement && !isDownMovement) {
						canMoveUp = false;

						if(isUpMovement) {
							collidedTop();
						}
					}
					
				}
				
	        }
	
			RaycastHit2D hitDown = Physics2D.Raycast(transform.position, Vector2.down, 2.0f, collisionMasks  );
	        if (hitDown.collider != null) {
	
				float distance = Mathf.Abs(hitDown.point.y - transform.position.y);
				if(distance < minDistanceForNeighbour && (hitDown.point.y <= transform.position.y)) {
					
					if(canMoveDown && IsStopped() && !isLeftMovement && !isRightMovement && !isUpMovement) {
						canMoveDown = false;

						if(isDownMovement) {
							collidedBottom();
						}
					}
					
				}
				
	        }
		}
		

		//block any position updates if any of these is happening
		if(associatedLevel.level != levelManager.currentLevel.level || isMovingBetweenLevels || 
					isMovingBetweenTeleportPoints || levelManager.IsPlayerDead() || 
					levelManager.IsStillAwaitingLevelTransitions() || HasAllMovementsBlocked() ) {
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

		//canMove = canMoveUp || canMoveDown || canMoveLeft || canMoveRight;

		if ( (canMoveUp && isUpMovement) || (canMoveDown && isDownMovement) 
			|| (isLeftMovement && canMoveLeft ) ||	(isRightMovement && canMoveRight) )
		{
			reachedTarget = false;

			//if(isLeftTwin) {
			//	Debug.Log("LEFT TWIN: canMoveDown? " + canMoveDown + " isDownMovemet? " + isDownMovement);
			//}

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
				//Debug.Log("SHOULD BE MOVING HERE isLeft " + isLeftTwin);
				transform.position = Vector3.MoveTowards(transform.position, new Vector3(transform.position.x,targetPosition.y,transform.position.z), Time.deltaTime * speed);
			}


			//body.isKinematic = !IsMovingInAnyDirection();

			
			//transform.position = Vector3.MoveTowards(transform.position, targetPosition, Time.deltaTime * speed);
		}
		else {
			//Debug.Log("REACHED TARGET true -> isLeft? " + isLeftTwin);
			reachedTarget = true;
		}

        if(IsPlayerStucked()) {
            Debug.Log("STUCKED ........................................");
        }

    }

	public void SetReachedTarget(bool reached) {
		reachedTarget = reached;
	}

	public bool IsMovingInAnyDirection() {
		return IsMovingUp() || IsMovingLeft() || IsMovingDown() || IsMovingRight();
	}

    public bool CanMoveInAnyDirection() {
        return canMoveUp || canMoveDown || canMoveLeft || canMoveRight;
    }

	public void SlideUp() {
		//Debug.Log("SLIDE UP isLeft? " + isLeftTwin);
		reachedTarget = false;
		isUpMovement = true;
        //body.isKinematic = false; //TODO FIXME
        targetPosition += (Vector3.up)*tileSize*maxTilesMovement;
		isLeftMovement = isRightMovement = isDownMovement = false;
		SoundEffectsHelper.Instance.PlayMoveSound();

        // the second argument, upwards, defaults to Vector3.up
        Quaternion rotationUp = Quaternion.Euler(new Vector3(0, 0, 90));
        SpecialEffectsHelper.Instance.PlaySmokeTrailTransform(transform.position,rotationUp);
	}

	public void SlideRight() {
        if(!isLeftTwin)
        {
            Debug.Log("SLIDE RIGHT can move right?" + canMoveRight);
        }
        
		reachedTarget = false;
		isRightMovement = true;
		//body.isKinematic = false;
		targetPosition += (Vector3.right)*tileSize*maxTilesMovement;
		isLeftMovement = isUpMovement = isDownMovement = false;
		SoundEffectsHelper.Instance.PlayMoveSound();
		Quaternion rotationRight = Quaternion.Euler(new Vector3(0, 0, 0));
        SpecialEffectsHelper.Instance.PlaySmokeTrailTransform(transform.position,rotationRight);
	}

	public void SlideDown() {
		reachedTarget = false;
		//Debug.Log("SLIDE DOWN called will move: " + (Vector3.down)*tileSize*maxTilesMovement);
		isDownMovement = true;
		//body.isKinematic = false;
        targetPosition += (Vector3.down)*tileSize*maxTilesMovement;
		isUpMovement = isLeftMovement = isRightMovement = false;
		SoundEffectsHelper.Instance.PlayMoveSound();
		Quaternion rotationDown = Quaternion.Euler(new Vector3(0, 0, -90));
        SpecialEffectsHelper.Instance.PlaySmokeTrailTransform(transform.position, rotationDown);
	}

	public void SlideLeft() {
		reachedTarget = false;
		isLeftMovement = true;
        //body.isKinematic = false;
        targetPosition += (Vector3.left)*tileSize*maxTilesMovement;
		isRightMovement = isDownMovement = isUpMovement = false;
		SoundEffectsHelper.Instance.PlayMoveSound();
		Quaternion rotationLeft = Quaternion.Euler(new Vector3(0, 0, 180));
        SpecialEffectsHelper.Instance.PlaySmokeTrailTransform(transform.position,rotationLeft);
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

	public bool IsMovingLeftAndNotStopped() {
		return isLeftMovement && !reachedTarget;
	}

	public bool IsMovingRightAndNotStopped() {
		return isRightMovement && !reachedTarget;
	}

	public bool IsMovingUpAndNotStopped() {
		return isUpMovement && !reachedTarget;
	}

	public bool IsMovingDownAndNotStopped() {
		return isDownMovement && !reachedTarget;
	}

	public void collidedLeft() {

		canMoveLeft = false;

		//can always go backwards from where i came
		canMoveRight = true;

        canMoveDown = !HitSomethingOnDown();
        canMoveRight = !HitSomethingOnRight(); //todo raycast



            if (!reachedTarget) {

				//reachedTarget = true;

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
	public void collidedRight() {

		canMoveRight = false;

		canMoveLeft = true;

        canMoveUp = !HitSomethingOnUp();
        canMoveDown = !HitSomethingOnDown(); //todo raycast
            //transform.Translate(-bodySpeed);

        if (!reachedTarget) {

				//reachedTarget = true;
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
	public void collidedTop() {

		canMoveUp = false;
		
		canMoveDown = true;

        canMoveLeft = !HitSomethingOnLeft();
        canMoveRight = !HitSomethingOnRight(); //todo raycast



            if (!reachedTarget) {

				//reachedTarget = true;
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
	public void collidedBottom() {
	
		canMoveDown = false;

        canMoveUp = true;

        canMoveLeft = !HitSomethingOnLeft();
        canMoveRight = !HitSomethingOnRight(); //TODO Depends, i need to raycast

        if (!reachedTarget) {
				
				//reachedTarget = true;
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

	void OnCollisionExit2D(Collision2D other)
	{

		HandlePlayerCollision handle = other.gameObject.GetComponent<HandlePlayerCollision>();
		
		Tile tile = other.transform.GetComponent<Tile>();
		if(tile!=null) {
			tile.HandleExitCollision(this);
		}
		else {
			if(!canMoveUp) {
				//Debug.Log("################# ALLOW UP ##################");
				canMoveUp = true;
			}
			if(!canMoveDown) {
				//Debug.Log("################# ALLOW DOWN ##################");
				canMoveDown = true;
			
			}
			if(!canMoveLeft) {
				//Debug.Log("################# ALLOW LEFT " + movement.isLeftTwin + "##################");
				canMoveLeft = true;
				
			}
			if(!canMoveRight) {
				//Debug.Log("################# ALLOW RIGHT ##################");

				canMoveRight = true;
			}

			//allow the block to move again
			MoveWayPoint move = other.gameObject.GetComponent<MoveWayPoint>();
			if(move!=null && move.IsPaused()) {
				move.ContinueMovement();
			}


			if(handle!=null) {
				handle.HandleExitCollision(this);
			}
						
		}
		//AllowAllMovementsAgain();
		
	}

	public Rigidbody2D GetBody() {
		return body;
	}

    //TODO pass enemy box?
	public bool IsIgnoreCollision(Transform otherObject, bool ignoreMovementDirection) {

		CheckBounds();

		Renderer otherRenderer = otherObject.GetComponent<Renderer>();
		if(otherRenderer == null) {
			otherRenderer = otherObject.GetComponentInChildren<Renderer>();
		}

		if (otherRenderer != null && playerBoxRenderer != null)
		{

			float otherWidth = otherRenderer.bounds.size.x;
			float otherHeight = otherRenderer.bounds.size.y;
			float otherLeft = otherRenderer.bounds.center.x - (otherWidth / 2);
			float otherRight = otherRenderer.bounds.center.x + (otherWidth / 2);
			float otherTop = otherRenderer.bounds.center.y - (otherHeight / 2);
		    float otherBottom = otherRenderer.bounds.center.y + (otherHeight / 2);

			if(isRightMovement && !ignoreMovementDirection) {

				//player right must be bigger than enemy left
				if( ( (playerRight + ignoreCollisionInterval) > otherLeft) && 
					(playerBoxRenderer.bounds.center.y  > otherTop ) && 
					(playerBoxRenderer.bounds.center.y  < otherBottom ) )   {
   
					return false;
				}
			}
			else if(isLeftMovement && !ignoreMovementDirection) {

				//Debug.Log("playerLeft: " + playerLeft + "otherLeft: " + otherLeft + " other right: " + otherRight); 
				//Debug.Log( (playerLeft - ignoreCollisionInterval) < otherRight);
				//Debug.Log("boxRenderer.bounds.center.y: " + boxRenderer.bounds.center.y + "> otherBottom?: " + otherBottom); 

				if( ( (playerLeft - ignoreCollisionInterval) < otherRight) && 
					(playerBoxRenderer.bounds.center.y  > otherTop) && 
					(playerBoxRenderer.bounds.center.y  < otherBottom ) )   {
		
					return false;
				}

			}
			else if(isUpMovement && !ignoreMovementDirection) {
			
				if( ( (playerTop - ignoreCollisionInterval) < otherBottom) && 
					(playerBoxRenderer.bounds.center.x  > otherLeft ) && 
					(playerBoxRenderer.bounds.center.x  < otherRight ) )   {

					return false;
				}
			}
			else if(isDownMovement && !ignoreMovementDirection) {
			
				if( ( (playerBottom + ignoreCollisionInterval) > otherTop) && 
					(playerBoxRenderer.bounds.center.x  > otherLeft ) && 
					(playerBoxRenderer.bounds.center.x  < otherRight ) )   {
		
					return false;
				}
			}
			//TODO NEW NEEDS REFACTORING
			else if( (!IsMovingInAnyDirection() || reachedTarget) && ignoreMovementDirection ) {
			
				if( ( (playerRight + ignoreCollisionInterval) > otherLeft) && 
					(playerBoxRenderer.bounds.center.y  > otherTop ) && 
					(playerBoxRenderer.bounds.center.y  < otherBottom ) )   {
   
					return false;
				}

				else if( ( (playerLeft - ignoreCollisionInterval) < otherRight) && 
					(playerBoxRenderer.bounds.center.y  > otherTop) && 
					(playerBoxRenderer.bounds.center.y  < otherBottom ) )   {
		
					return false;
				}
				else if( ( (playerTop - ignoreCollisionInterval) < otherBottom) && 
					(playerBoxRenderer.bounds.center.x  > otherLeft ) && 
					(playerBoxRenderer.bounds.center.x  < otherRight ) )   {

					return false;
				}

				else if( ( (playerBottom + ignoreCollisionInterval) > otherTop) && 
					(playerBoxRenderer.bounds.center.x  > otherLeft ) && 
					(playerBoxRenderer.bounds.center.x  < otherRight ) )   {
		
					return false;
				}
				
			}
			
			
		}
		return true;
		//return !(Mathf.Abs(otherPosition - playerPosition) < ignoreCollisionInterval);
	}
    //TODO maybe keep a reference foir the last object checked?? to avoid process the same again
    /*void OnCollisionStay2D(Collision2D other) {
        bool isEnemy = other.transform.Equals("Enemy");
        if(isEnemy) {
            bool ignoreCollision = IsIgnoreCollision(other.transform);
            EnemyBox enemy = other.gameObject.GetComponent<EnemyBox>();
            Debug.Log("ENEMY COLLSION WITH " + other.gameObject.name);
            //i could not be moving but if the enemy is we cant ignore it
            if (!ignoreCollision || ((!IsMovingInAnyDirection() || IsStopped()) && enemy.isMovingEnemy))
            {
                enemy.HandleCollision(this);
            }
            else Debug.Log("HANDLE CALLED"); 
        }

    }*/

    void OnCollisionEnter2D(Collision2D other)
	{
        
        //ignore it
        if (isMovingBetweenLevels || otherTwin.isMovingBetweenLevels || levelManager.IsPlayerDead() ) {

           Debug.Log("DEBUG: COLLISION IGNORED -±other.transform.name: " + other.transform.name);
			return;
		}

        string otherTag = other.transform.tag;
        bool isPortal = otherTag.Equals("Portal");
		
		bool isEnemy = otherTag.Equals("Enemy");
		bool isBox = otherTag.Equals("Box");
		bool isBomb = otherTag.Equals("Bomb");
		bool isElectric = otherTag.Equals("Electric");
		bool isMovingBlock = other.gameObject.GetComponent<MoveWayPoint>() != null; 
		bool isSlider = otherTag.Equals("Slider");

		bool ignoreCollision = true;

        Debug.Log("Collided with: " + other.gameObject.name + " right?" + isRightMovement);


        if (isEnemy)
        {
            Debug.Log("ENEMY COLLIDDED CALLED  " + other.gameObject.name + " right?" + isRightMovement + "left?" + isLeftMovement + " down?" + isDownMovement + " up?" + isUpMovement);

            EnemyBox enemy = other.gameObject.GetComponent<EnemyBox>();
            enemy.HandleCollision(this);
            return;
        }


        //Blocks and other things not tagged Enemy!
        if (isRightMovement && canMoveRight) {
			//Mathf.Abs(other.transform.position.y - transform.position.y) < ignoreCollisionInterval
				if (!IsIgnoreCollision( other.transform, false /* other.transform.position.y,transform.position.y) && other.transform.position.x >= transform.position.x*/) ){
				
					collidedRight();
					//colRight = true;
					ignoreCollision = false;

					Debug.Log("RIGHT COLLISION WITH ====> " + other.transform.name);
				}
						
		}
         else if(isLeftMovement && canMoveLeft) {


				//Debug.Log("DEBUG: IS LEFT AND CAN MOVE LEFT");
				if (!IsIgnoreCollision(other.transform, false /*other.transform.position.y,transform.position.y) && other.transform.position.x <= transform.position.x*/ ) )
				{
					collidedLeft();
					//colLeft = true;
					ignoreCollision = false;

					Debug.Log("LEFT COLLISION WITH ====> " + other.transform.name);
				}
						
		}//TODO raycast to see if i can move up or not
        else if(isUpMovement && canMoveUp) {
						
            Debug.Log("DEBUG: IS UP AND CAN MOVE UP");
				//otherwise just ignore this one
				//Mathf.Abs(other.transform.position.x - transform.position.x) < ignoreCollisionInterval

				if (!IsIgnoreCollision(other.transform, false /*other.transform.position.x, transform.position.x) && other.transform.position.y >= transform.position.y*/) )
				{
					collidedTop();
					//colUp = true;
					ignoreCollision = false;

					Debug.Log("TOP COLLISION WITH ====> " + other.transform.name);
				}
							
		}
        else if(isDownMovement && canMoveDown) {
			//Mathf.Abs(other.transform.position.x - transform.position.x) < ignoreCollisionInterval
              //Debug.Log("DEBUG: IS DOWN AND CAN MOVE DOWN");
                        
			if (!IsIgnoreCollision(other.transform, false/*other.transform.position.x, transform.position.x) && other.transform.position.y <= transform.position.y*/))
			{
			        collidedBottom();
					//colDown = true;
					ignoreCollision = false;

					Debug.Log("DOWN COLLISION WITH ====> " + other.transform.name);
							
			}
						
        } else if(!IsMovingInAnyDirection() && isEnemy) {
                 //even if not moving, if it is an enemy
                 Debug.Log("DEBUG: CODE ME, EMPTY BLOCK");
        }


        //---------------------------------------------------------

		Tile tile = other.transform.GetComponent<Tile>();
		TeletransportPoint point = other.transform.GetComponent<TeletransportPoint>();
					
		if(point!=null) {
			Debug.Log("################# TIle HandleTileCollisions --> TeletransportPoint ################## ");
			point.HandleCollision(this);
		}
		else if(tile!=null && !ignoreCollision) {
			tile.HandleCollision(this);
		}
		else if(isPortal && !ignoreCollision) {
            
            other.gameObject.GetComponent<Portal>().HandleCollision(this);

        } 
        else if(isSlider && !ignoreCollision) {
			SliderBlock slider = other.gameObject.GetComponent<SliderBlock>();
            slider.HandleCollision(this);
		}
		else if(isMovingBlock) {
			//pause the moving block
            if(!ignoreCollision) {
              MoveWayPoint move = other.gameObject.GetComponent<MoveWayPoint>();
              move.PauseMovement();
              move.RestartMovementAfterPause(2f);
            }
          
		}
		else if(isBomb && !ignoreCollision) {
		
            other.gameObject.GetComponent<Bomb>().HandleCollision(this);
		}
		else if(isElectric && !ignoreCollision) {
		
			ElectricWire wire = other.gameObject.GetComponent<ElectricWire>();
			wire.ElectrocutePlayer(this);
		}
        else if(isBox && !ignoreCollision) {
            other.gameObject.GetComponent<Box>().HandleCollision(this);
			//TODO there are game objects that are tagged box, but do not have the component CHECK!!!
		}
        else if(isEnemy /*&& !ignoreCollision*/ ) {
            EnemyBox enemy = other.gameObject.GetComponent<EnemyBox>();
           enemy.HandleCollision(this);
       
        }

		else if(!ignoreCollision) {
			HandlePlayerCollision handle = other.gameObject.GetComponent<HandlePlayerCollision>();
			if(handle!=null) {
				handle.HandleCollision(this);
			}
		}
		
	}


	//TODO this is not doing what the name suggests, is just reversing things DOUBLE CHECK
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

        Debug.Log("AllowAllMovementsAgain " + canMoveRight + " is left" + isLeftTwin);

	}

	public void AllowAllMovementsAgainV2()
	{
		canMoveUp = canMoveLeft = canMoveRight = canMoveDown = true;
		isLeftMovement = isUpMovement = isDownMovement = isRightMovement = false;
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

		//Debug.Log("###### ResetPlayerOnNewLevel ON isLeft? " + isLeftTwin);
		if(isMovingBetweenLevels == false && reachedNewLevel == false) {
			//Debug.Log("#### Already done this!! is left?" + isLeftTwin);
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

		//Debug.Log("###### ResetLevel ON isLeft? " + isLeftTwin);
		//allow play/move again
		associatedLevel = levelManager.currentLevel;
		reachedTarget = true;
		targetPosition = transform.position;
		//TODO added this prop
		reachedNewLevel = false;
		isMovingBetweenLevels = false;
	}

	public void EnableColliders() {

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
	public void DisableCollidersAndMovements() {

		//Debug.Log("DISABLED COLLIDER ON isLeft? " + isLeftTwin);
		isMovingBetweenLevels = true;
		reachedTarget = false;

		DisableAllMovements();


		DisableColliders();
		
		
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
	}
	public void SetReachTargetPosition(Vector3 position) {
		targetPosition = position;
		transform.position = targetPosition;
		reachedTarget = true;
	}

    public bool GetReachedTarget() {
        return reachedTarget;
    }

	public void StopMovementVelocity() {

		if(body!=null) {
			body.velocity = Vector3.zero;
		}
	}

	public void DisableAllMovements() {

		isUpMovement = isDownMovement = isLeftMovement = isRightMovement = false;
		canMoveLeft = canMoveDown = canMoveUp = canMoveRight = false;
		
	}

	bool HasAllMovementsBlocked() {

		if(!canMoveLeft && !canMoveRight && !canMoveDown && !canMoveUp) {
			return true;
		}
		return false;
	}

	public void SetIsMovingBetweenLevels(bool moving) {

		isMovingBetweenLevels = moving;
		EnableOrDisableTransportBubble();
		
		if(isMovingBetweenLevels) {
			//disable the rigidbody
			StopMovementVelocity();
			//disable all the colliders, parent and children
			DisableCollidersAndMovements();

		}
		
	}

    //TODO start with bubble enabled? and only burst
	private void EnableOrDisableTransportBubble()
	{
		if(transportBubble !=null && levelManager.IsGameStarted()) { //TODO check &&

			if(!isMovingBetweenLevels && transportBubble.gameObject.active) {
				//burst effect
				SpecialEffectsHelper.Instance.PlayBurstBubbleEffect(transform.position);
			}
			transportBubble.gameObject.SetActive(isMovingBetweenLevels);
		}
	}
    
    public void DisableBubbleOnStartup() {
        if(transportBubble !=null) {
            SpecialEffectsHelper.Instance.PlayBurstBubbleEffect(transform.position);
            transportBubble.gameObject.SetActive(false);
        }
    }
    
    public void EnableBubbleOnRespawn() {
        if(transportBubble !=null) {
            transportBubble.gameObject.SetActive(true);
        }
    }

	public void SetIsMovingBetweenTeleportPoints(bool moving) {
		isMovingBetweenTeleportPoints = moving;
	}

	public bool GetIsMovingBetweenLevels() {

		return isMovingBetweenLevels;
	}

	// Unsubscribing Delegate ALWAYS!!!
    void OnDisable()
    {
      //DelegateHandler.actionDelegate -= ReEnableCollidersOnNewLevel;
    }

    public bool IsPlayerStucked() {
        return reachedTarget && levelManager.IsGameStarted() && !levelManager.IsPlayerDead() && !CanMoveInAnyDirection();
    }
    
    public void ShowDeathSpriteAnimation() {
        //added this one
        anim.enabled = false;
        GetComponentInChildren<SpriteRenderer>().sprite = burnedSprite;
        deathWings.gameObject.SetActive(true);
        StartCoroutine(HideDeathWings());
    }

    IEnumerator HideDeathWings() {
        yield return new WaitForSeconds(2f);
        deathWings.gameObject.SetActive(false);
    }

	public void ShowBurnSpriteAnimation() {
		anim.enabled = false;
		GetComponentInChildren<SpriteRenderer>().sprite = burnedSprite;
        
        //show death wings too
        deathWings.gameObject.SetActive(true);
        StartCoroutine(HideDeathWings());
		levelManager.KillPlayer();
	}

	public void ShowElectrocutedSpriteAnimation() {
		anim.enabled = false;
		GetComponentInChildren<SpriteRenderer>().sprite = electrocutedSprite;
		StartCoroutine(Electrocussion());
	}

	IEnumerator Electrocussion() {
		yield return new WaitForSeconds(0.12f);
		GetComponentInChildren<SpriteRenderer>().sprite = originalSprite;
		yield return new WaitForSeconds(0.12f);
		GetComponentInChildren<SpriteRenderer>().sprite = electrocutedSprite;
		yield return new WaitForSeconds(0.12f);
		GetComponentInChildren<SpriteRenderer>().sprite = originalSprite;
        
        yield return new WaitForSeconds(0.12f);
        GetComponentInChildren<SpriteRenderer>().sprite = burnedSprite;
        
        //show death wings too
        deathWings.gameObject.SetActive(true);
        StartCoroutine(HideDeathWings());
        
		levelManager.KillPlayer();
		
	}

	public void ResetOriginalSprite() {

		GetComponentInChildren<SpriteRenderer>().sprite = originalSprite;
		anim.enabled = true;
	}

	public void ResetOriginalPosition() {
		transform.position = originalPositionInLevel;
	}

    public bool HitSomethingOnLeft()
    {
        //Fire some rays to check if we have anything on the left, right, up or down
        RaycastHit2D hitLeft = Physics2D.Raycast(transform.position, Vector2.left, 2.0f, collisionMasks);
        if (hitLeft.collider != null)
        {

            float dist = Mathf.Abs(hitLeft.point.x - transform.position.x);
            if (dist < minDistanceForNeighbour && (hitLeft.point.x <= transform.position.x))
            {

                return true;

            }

        }
        return false;

    }

    public bool HitSomethingOnRight()
    {
        RaycastHit2D hitRight = Physics2D.Raycast(transform.position, Vector2.right, 2.0f, collisionMasks);
        if (hitRight.collider != null)
        {

            float dist = Mathf.Abs(hitRight.point.x - transform.position.x);//make sure it is on the right of the player
            if (dist < minDistanceForNeighbour && (hitRight.point.x >= transform.position.x))
            {

                return true;

            }

        }
        return false;

    }

    public bool HitSomethingOnUp()
    {
        RaycastHit2D hitUp = Physics2D.Raycast(transform.position, Vector2.up, 2.0f, collisionMasks);
        if (hitUp.collider != null)
        {

            float dist = Mathf.Abs(hitUp.point.y - transform.position.y);
            if (dist < minDistanceForNeighbour && (hitUp.point.y >= transform.position.y))
            {

                return true;

            }

        }
        return false;
    }



    public bool HitSomethingOnDown()
    {
        RaycastHit2D hitDown = Physics2D.Raycast(transform.position, Vector2.down, 2.0f, collisionMasks);
        if (hitDown.collider != null)
        {

            float dist = Mathf.Abs(hitDown.point.y - transform.position.y);
            if (dist < minDistanceForNeighbour && (hitDown.point.y <= transform.position.y))
            {

                return true;

            }

        }
        return false;
    }

            
}
