using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBox : MonoBehaviour, HandlePlayerCollision {

	public LevelManager levelmanager;
	public bool killPlayerOnTouch = true; //if not just dcrease a move or something

	public bool shrinkPlayer = false;
	private Tile associatedTile;
	//if moving do not count for the collision marging
	public bool isMovingEnemy = false;

    //TODO WHY WHY WHY???
    public bool canIgnoreCollisions = true; //if set to false cannot ignore any touch

	// Use this for initialization
	void Start () {
		associatedTile = GetComponent<Tile>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public Tile GetTile() {

		return associatedTile;
	}

	bool CannotIgnoreRightCollision(PlayerMovement player) {

		return player.IsMovingRight() && !IsIgnoreCollision(player, false);
	}

	bool CannotIgnoreLeftCollision(PlayerMovement player)
	{
		return player.IsMovingLeft() && !IsIgnoreCollision(player, false);
	}

	bool CannotIgnoreUpCollision(PlayerMovement player) {
		return player.IsMovingUp() && !IsIgnoreCollision(player, false);
	}

	bool CannotIgnoreDownCollision(PlayerMovement player)
	{
		return player.IsMovingDown() && !IsIgnoreCollision(player, false);
	}

    bool CannotIgnoreCollisionWhenNotMoving(PlayerMovement player) {
		return (!player.IsMovingInAnyDirection() || player.GetReachedTarget()) && !IsIgnoreCollision(player, true);
    }

	bool CannotIgnoreCollisionWhenMoving(PlayerMovement player) {
        return CannotIgnoreUpCollision(player) || CannotIgnoreDownCollision(player) || CannotIgnoreLeftCollision(player) || CannotIgnoreRightCollision(player);
    }

	public void HandleExitCollision(PlayerMovement player) {

	}

	//bool isLeftMovement, bool isRightMovement, bool isUpMovement, bool isDownMovement
	public void HandleCollision(PlayerMovement player) {
		if(levelmanager==null) {
			GameObject scripts = GameObject.FindGameObjectWithTag("Scripts");
			levelmanager = scripts.GetComponent<LevelManager>();

		}
      
		//TODO THIS SHIT NEEDS A RE-WRITE!!!
		//TODO if moving enemy should not be enough condition
		if (killPlayerOnTouch)
		{
            //levelmanager.KillPlayer();
			bool killed = false;
            //TODO WTF!!!
			//ANTES ESTAVA --> (isMovingEnemy && !player.IsStopped() ) || CannotIgnoreRightCollision(player)
			//e o player stopped só via a velocity, sempre a 0,0,0

			//means player is moving
            if(isMovingEnemy && !player.GetReachedTarget() && CannotIgnoreCollisionWhenMoving(player)) {

                player.ShowDeathSpriteAnimation();
                levelmanager.KillPlayer();
                killed = true;
            }//player is stopped, but object is moving
			else if( (isMovingEnemy && player.GetReachedTarget()) && CannotIgnoreCollisionWhenNotMoving(player) ) {
            
                player.ShowDeathSpriteAnimation();
                levelmanager.KillPlayer();
                killed = true;
            }
            else if ( player.IsMovingRight() && CannotIgnoreRightCollision(player))
			{
                player.ShowDeathSpriteAnimation();
				levelmanager.KillPlayer();
				killed = true;
			}
            else if ( player.IsMovingLeft() && CannotIgnoreLeftCollision(player)  )
			{
				//TODO check this SHIT, not working!!!
				//if (CannotIgnoreLeftCollision(player) || (isMovingEnemy && ( CannotIgnoreUpCollision(player) || CannotIgnoreDownCollision(player) || CannotIgnoreRightCollision(player) ) ) ) {
                    player.ShowDeathSpriteAnimation();
					levelmanager.KillPlayer();
					killed = true;
				//}
				
			}
            else if ( player.IsMovingUp() && CannotIgnoreUpCollision(player))
			{
                player.ShowDeathSpriteAnimation();
				levelmanager.KillPlayer();
				killed = true;
			}
            else if ( player.IsMovingDown() && CannotIgnoreDownCollision(player))
			{
				//Debug.Log("KILLEDDDDDDDDDDDDD DOWN");
                player.ShowDeathSpriteAnimation();
				levelmanager.KillPlayer();
				killed = true;
			}

            //also stop the enemy movement, but restart it after 2 secs
            if(isMovingEnemy && killed) {
                Debug.Log("KILLEDDDDDDDDDDDDD");
				MoveWayPoint move = GetComponent<MoveWayPoint>();
				if(move!=null) {
					//StopMovement
					move.PauseMovement();
                    move.RestartMovementAfterPause(2f);

                }
			}
		}
		else if (shrinkPlayer)
		{
			MakeShrinkEnemy shrinkScript = GetComponent<MakeShrinkEnemy>();
			//Debug.Log("DO SHRNK IT?????");
			if (shrinkScript != null)
			{
                bool applyShrink = false;

				if (player.IsMovingUp() && CannotIgnoreUpCollision(player))
				{
					player.collidedTop();
                    applyShrink = true;

                }
				else if (player.IsMovingDown() && CannotIgnoreDownCollision(player))
				{
					player.collidedBottom();
                    applyShrink = true;
                }
				else if (player.IsMovingRight() && CannotIgnoreRightCollision(player))
				{
					player.collidedRight();
                    applyShrink = true;
                }
				else if (player.IsMovingLeft() && CannotIgnoreLeftCollision(player))
				{
					player.collidedLeft();
                    applyShrink = true;
                }
                if(applyShrink)
                {
                    shrinkScript.enabled = true;
                    shrinkScript.SetObjectToShrink(player, levelmanager, this);
                    //otherwise ignore it
                }
				
			}
		}
		else Debug.Log("DO NOTHING???");
	}

	public void CallBack(MakeShrinkEnemy shrink) {
		shrink.enabled = false;
	}

	//TODO this if fucking wrong for sure
	public bool IsIgnoreCollision(PlayerMovement player, bool ignoreMovementDirection) {
		//Debug.Log("@IS IGNORE COLLISION? " + IsIgnoreCollisionInternal(transform, true));
        
        bool ignore = IsIgnoreCollisionInternal(player, ignoreMovementDirection);
        Debug.Log("@IS IGNORE COLLISION? " + ignore);
        return ignore;
	}

    private bool IsIgnoreCollisionInternal(PlayerMovement player, bool ignoreMovementDirection)
    {
        Renderer playerBoxRenderer = player.playerBoxRenderer;
       

        float playerWidth = playerBoxRenderer.bounds.size.x;
        float playerHeight = playerBoxRenderer.bounds.size.y;
        float playerLeft = playerBoxRenderer.bounds.center.x - (playerWidth / 2);
        float playerRight = playerBoxRenderer.bounds.center.x + (playerWidth / 2);
        float playerTop = playerBoxRenderer.bounds.center.y - (playerHeight / 2);
        float playerBottom = playerBoxRenderer.bounds.center.y + (playerHeight / 2);
        

        Renderer thisRenderer = transform.GetComponent<Renderer>();
        if (thisRenderer == null)
        {
            thisRenderer = transform.GetComponentInChildren<Renderer>();
        }

        if (thisRenderer != null && playerBoxRenderer != null)
        {

            float otherWidth = thisRenderer.bounds.size.x;
            float otherHeight = thisRenderer.bounds.size.y;
            float otherLeft = thisRenderer.bounds.center.x - (otherWidth / 2);
            float otherRight = thisRenderer.bounds.center.x + (otherWidth / 2);
            float otherTop = thisRenderer.bounds.center.y - (otherHeight / 2);
            float otherBottom = thisRenderer.bounds.center.y + (otherHeight / 2);

            if (player.IsMovingRight() && !ignoreMovementDirection)
            {

                //player right must be bigger than enemy left
                if (((playerRight + player.ignoreCollisionInterval) > otherLeft) &&
                    (playerBoxRenderer.bounds.center.y > otherTop) &&
                    (playerBoxRenderer.bounds.center.y < otherBottom))
                {

                    return false;
                }
            }
            else if (player.IsMovingLeft() && !ignoreMovementDirection)
            {

                //Debug.Log("playerLeft: " + playerLeft + "otherLeft: " + otherLeft + " other right: " + otherRight); 
                //Debug.Log( (playerLeft - ignoreCollisionInterval) < otherRight);
                //Debug.Log("boxRenderer.bounds.center.y: " + boxRenderer.bounds.center.y + "> otherBottom?: " + otherBottom); 

                if (((playerLeft - player.ignoreCollisionInterval) < otherRight) &&
                    (playerBoxRenderer.bounds.center.y > otherTop) &&
                    (playerBoxRenderer.bounds.center.y < otherBottom))
                {

                    return false;
                }

            }
            else if (player.IsMovingUp() && !ignoreMovementDirection)
            {

                if (((playerTop - player.ignoreCollisionInterval) < otherBottom) &&
                    (playerBoxRenderer.bounds.center.x > otherLeft) &&
                    (playerBoxRenderer.bounds.center.x < otherRight))
                {

                    return false;
                }
            }
            else if (player.IsMovingDown() && !ignoreMovementDirection)
            {

                if (((playerBottom + player.ignoreCollisionInterval) > otherTop) &&
                    (playerBoxRenderer.bounds.center.x > otherLeft) &&
                    (playerBoxRenderer.bounds.center.x < otherRight))
                {

                    return false;
                }
            }
            //TODO NEW NEEDS REFACTORING
            else if ((!player.IsMovingInAnyDirection() || player.GetReachedTarget() && ignoreMovementDirection))
            {

                if (((playerRight + player.ignoreCollisionInterval) > otherLeft) &&
                    (playerBoxRenderer.bounds.center.y > otherTop) &&
                    (playerBoxRenderer.bounds.center.y < otherBottom))
                {

                    return false;
                }

                else if (((playerLeft - player.ignoreCollisionInterval) < otherRight) &&
                    (playerBoxRenderer.bounds.center.y > otherTop) &&
                    (playerBoxRenderer.bounds.center.y < otherBottom))
                {

                    return false;
                }
                else if (((playerTop - player.ignoreCollisionInterval) < otherBottom) &&
                    (playerBoxRenderer.bounds.center.x > otherLeft) &&
                    (playerBoxRenderer.bounds.center.x < otherRight))
                {

                    return false;
                }

                else if (((playerBottom + player.ignoreCollisionInterval) > otherTop) &&
                    (playerBoxRenderer.bounds.center.x > otherLeft) &&
                    (playerBoxRenderer.bounds.center.x < otherRight))
                {

                    return false;
                }

            }


        }
        return true;
        //return !(Mathf.Abs(otherPosition - playerPosition) < ignoreCollisionInterval);
    }

}
