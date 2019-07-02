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
        //Debug.Log("CannotIgnoreCollisionWhenNotMoving() ? " + CannotIgnoreCollisionWhenNotMoving(player));
		//Debug.Log("CannotIgnoreCollisionWhenMoving() ? " + CannotIgnoreCollisionWhenMoving(player));
		//Debug.Log("isMovingEnemy ? " + isMovingEnemy);
		//Debug.Log("player.GetReachedTarget() ? " + player.GetReachedTarget());
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
                levelmanager.KillPlayer();
                killed = true;
            }//player is stopped, but object is moving
			if( (isMovingEnemy && player.GetReachedTarget()) && CannotIgnoreCollisionWhenNotMoving(player) ) {
                levelmanager.KillPlayer();
                killed = true;
            }
            else if ( player.IsMovingRight() && CannotIgnoreRightCollision(player))
			{
				levelmanager.KillPlayer();
				killed = true;
			}
            else if ( player.IsMovingLeft() && CannotIgnoreLeftCollision(player)  )
			{
				//TODO check this SHIT, not working!!!
				//if (CannotIgnoreLeftCollision(player) || (isMovingEnemy && ( CannotIgnoreUpCollision(player) || CannotIgnoreDownCollision(player) || CannotIgnoreRightCollision(player) ) ) ) {
					levelmanager.KillPlayer();
					killed = true;
				//}
				
			}
            else if ( player.IsMovingUp() && CannotIgnoreUpCollision(player))
			{

				levelmanager.KillPlayer();
				killed = true;
			}
            else if ( player.IsMovingDown() && CannotIgnoreDownCollision(player))
			{
				Debug.Log("KILLEDDDDDDDDDDDDD DOWN");
				levelmanager.KillPlayer();
				killed = true;
			}

            //also stop the enemy movement
            if(isMovingEnemy && killed) {
                Debug.Log("KILLEDDDDDDDDDDDDD");
				MoveWayPoint move = GetComponent<MoveWayPoint>();
				if(move!=null) {
					//StopMovement
					move.PauseMovement();
				}
			}
		}
		else if (shrinkPlayer)
		{
			MakeShrinkEnemy shrinkScript = GetComponent<MakeShrinkEnemy>();
			Debug.Log("DO SHRNK IT?????");
			if (shrinkScript != null)
			{

				if (player.IsMovingUp())
				{
					player.collidedTop();
				}
				else if (player.IsMovingDown())
				{
					player.collidedBottom();
				}
				else if (player.IsMovingRight())
				{
					player.collidedRight();
				}
				else if (player.IsMovingLeft())
				{
					player.collidedLeft();
				}
				shrinkScript.enabled = true;
				shrinkScript.SetObjectToShrink(player, levelmanager, this);
			}
		}
		else Debug.Log("DO NOTHING???");
	}

	public void CallBack(MakeShrinkEnemy shrink) {
		shrink.enabled = false;
	}

	//TODO this if fucking wrong for sure
	public bool IsIgnoreCollision(PlayerMovement player, bool ignoreMovementDirection) {
		Debug.Log("@IS IGNORE COLLISION? " + player.IsIgnoreCollision(transform, true));
        
        bool ignore = player.IsIgnoreCollision(transform, ignoreMovementDirection);
        return ignore;
	}

}
