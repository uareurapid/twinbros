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

	public float ignoreCollisionInterval = 0.5f; //ignore if the distance is greater
	public Level associatedLevel; //can only move on the associated Level
	private LevelManager levelManager;

	public PlayerMovement otherTwin;

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

	private int collisionMasks = -1;
	public float minDistanceForNeighbour = 0.55f; //how close i can be to another element/box
												  //can move down, up, left? etc?

	Animator anim;
	private Sprite originalSprite;
	public Sprite burnedSprite;
	public Sprite electrocutedSprite;

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

		initialScale = transform.localScale;
		initialRotation = transform.localRotation;

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

		ResetPlayer();
		//DelegateHandler.actionDelegate += ReEnableCollidersOnNewLevel;
    	//TODO undelegate on destroy

    }

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
		return body.velocity == Vector2.zero;
	}

    void FixedUpdate()
    {

		if(!levelManager.IsGameStarted() || levelManager.isPlayerDead() || levelManager.IsAboutToDie()) {
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
		

		//block any position updates if any of these is happening
		if(associatedLevel.level != levelManager.currentLevel.level || isMovingBetweenLevels || 
					isMovingBetweenTeleportPoints || levelManager.isPlayerDead() || 
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
			
			
			

			
			//transform.position = Vector3.MoveTowards(transform.position, targetPosition, Time.deltaTime * speed);
		}
		else {
			//Debug.Log("REACHED TARGET true -> isLeft? " + isLeftTwin);
			reachedTarget = true;
		}

    }

	public bool IsMovingInAnyDirection() {
		return IsMovingUp() || IsMovingLeft() || IsMovingDown() || IsMovingRight();
	}

	public void SlideUp() {
		//Debug.Log("SLIDE UP isLeft? " + isLeftTwin);
		reachedTarget = false;
		isUpMovement = true;
        targetPosition += (Vector3.up)*tileSize*maxTilesMovement;
		isLeftMovement = isRightMovement = isDownMovement = false;
		SoundEffectsHelper.Instance.PlayMoveSound();
	}

	public void SlideRight() {
		reachedTarget = false;
		isRightMovement = true;
		targetPosition += (Vector3.right)*tileSize*maxTilesMovement;
		isLeftMovement = isUpMovement = isDownMovement = false;
		SoundEffectsHelper.Instance.PlayMoveSound();
	}

	public void SlideDown() {
		reachedTarget = false;
		//Debug.Log("SLIDE DOWN called will move: " + (Vector3.down)*tileSize*maxTilesMovement);
		isDownMovement = true;
        targetPosition += (Vector3.down)*tileSize*maxTilesMovement;
		isUpMovement = isLeftMovement = isRightMovement = false;
		SoundEffectsHelper.Instance.PlayMoveSound();
	}

	public void SlideLeft() {
		reachedTarget = false;
		isLeftMovement = true;
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

		canMoveLeft = false;

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

		//if(other.transform.CompareTag("Bomb")) {

			//avoid getting killed by the bomb
		//}
		
		Tile tile = other.transform.GetComponent<Tile>();
		if(tile!=null) {
			tile.HandleTileExitCollisions(this);
		}
		//AllowAllMovementsAgain();
		
	}

	public Rigidbody2D GetBody() {
		return body;
	}

	public bool IsIgnoreCollision(float pos1, float pos2) {
		return !(Mathf.Abs(pos1 - pos2) < ignoreCollisionInterval);
	}

    void OnCollisionEnter2D(Collision2D other)
	{
		//ignore it
		if(isMovingBetweenLevels || otherTwin.isMovingBetweenLevels || levelManager.isPlayerDead()) {
			return;
		}
		string otherTag = other.transform.tag;
		bool isPortal = otherTag.Equals("Portal");
		bool isEnemy = otherTag.Equals("Enemy");
		bool isBox = otherTag.Equals("Box");
		bool isBomb = otherTag.Equals("Bomb");
		bool isElectric = otherTag.Equals("Electric");
		bool colUp = false;
		bool colDown = false;
		bool colLeft = false;
		bool colRight = false;

		bool ignoreCollision = true;
		if(!isPortal) {
				Tile tile = other.transform.GetComponent<Tile>();
				if(tile!=null) {
					//Debug.Log("TILE COLLISION isLeft" + isLeftTwin + " name: " + other.transform.name);
					ignoreCollision = tile.HandlePlayerCollision(this);
				}
				else {

					//Debug.Log("----- IS SOMETHING ELSE COLLIDING box? " + isBox + " name" +  other.transform.name + " isLeftMovement? " + isLeftMovement + " isRightMovement? " + isRightMovement + " isUpMovement? " + isUpMovement)  ;
					if(isRightMovement) {
						//Mathf.Abs(other.transform.position.y - transform.position.y) < ignoreCollisionInterval
						if (!IsIgnoreCollision(other.transform.position.y,transform.position.y)) {
							collidedRight();
							colRight = true;
							ignoreCollision = false;
						}
						
					}
					else if(isLeftMovement) {
						//Mathf.Abs(other.transform.position.y - transform.position.y) < ignoreCollisionInterval
						if (!IsIgnoreCollision(other.transform.position.y,transform.position.y))
						{
							collidedLeft();
							colLeft = true;
							ignoreCollision = false;
						}
						
					}
					else if(isUpMovement) {
		
						//otherwise just ignore this one
						//Mathf.Abs(other.transform.position.x - transform.position.x) < ignoreCollisionInterval
						if (!IsIgnoreCollision(other.transform.position.x, transform.position.x))
						{
							collidedTop();
							colUp = true;
							ignoreCollision = false;
						}
							
					}
					else if(isDownMovement) {
						//Mathf.Abs(other.transform.position.x - transform.position.x) < ignoreCollisionInterval
						if (!IsIgnoreCollision(other.transform.position.x, transform.position.x))
						{
							collidedBottom();
							colDown = true;
							ignoreCollision = false;
							
						}
						
					}

					if(isEnemy) {
						 EnemyBox enemy = other.gameObject.GetComponent<EnemyBox>();
						 enemy.HandlePlayerCollision(this);
					}
					else if(isBomb) {
		
						Bomb bomb = other.gameObject.GetComponent<Bomb>();
						bomb.HandlePlayerCollision(this);
					}
					else if(isElectric) {
		
						ElectricWire wire = other.gameObject.GetComponent<ElectricWire>();
						wire.ElectrocutePlayer(this);
					}
					else if(isBox) {
						Box box = other.gameObject.GetComponent<Box>();
						//TODO there are game objects that are tagged box, but do not have the component CHECK!!!
						if(box!=null && box.isSurpriseBox) {
		
							Debug.Log("FADE SURPRISE BOX");
							box.FadeSurpriseBox(colUp,colRight,colDown,colLeft, this);
						}
						SpecialEffectsHelper.Instance.PlayBoxCollisionEffect(transform.position);
					}	
				}

				//this code is not reachable
				//death by moves
				//if(!ignoreCollision && levelManager.CheckIfBothAreDead()) {
				//	return;
				//}

			
		}//yes, is a portal collision
		else {
			//is portal
			if (isMovingBetweenLevels || levelManager.isPlayerDead())
			{
				//ignore this collision
				return;
			}
			else {
				//TODO keep coding me
				levelManager.TwinCollidedWithPortal(gameObject);
				Portal portal = other.gameObject.GetComponent<Portal>();
				StartCoroutine(MoveToNextLevel(portal));
			}
		}

		
		
	}

	IEnumerator MoveToNextLevel(Portal portal) {
		yield return new WaitForSeconds(0.5f);
		portal.MoveToNextLevel();
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

		Debug.Log("DISABLED COLLIDER ON isLeft? " + isLeftTwin);
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
		if(isMovingBetweenLevels) {
			//disable the rigidbody
			StopMovementVelocity();
			//disable all the colliders, parent and children
			DisableCollidersAndMovements();

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


	public void ShowBurnSpriteAnimation() {
		anim.enabled = false;
		GetComponentInChildren<SpriteRenderer>().sprite = burnedSprite;
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
