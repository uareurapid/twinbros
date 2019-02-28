using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeletransportPoint : MonoBehaviour {


	public Transform destination;
	public bool continueMovingSameDirection = true;
	//if set to true and exitDirection is valid, use that one instead
	public bool overridePreviousDirection = false;
	public string exitDirection = GameConstants.LEFT_DIRECTION;
	
	public void HandlePlayerCollision(PlayerMovement player) {

		if(destination!=null) {

			player.SetIsMovingBetweenTeleportPoints(true);
			player.StopMovementVelocity();

			//Debug.Log("%%%%%%%%%%%%%%%%%%% TELETRANSPORT $$$$$$$$$$$$$$$$$$$$");
			Collider2D col = destination.GetComponent<Collider2D>();
			if(col!=null) {
			//	Debug.Log("disable the collider on the destination, so it can be on top of it");
				col.enabled = false; 
				//disable the collider on the destination, so it can be on top of it
			}

			//player.DisableColliders();
	
			//grab the last movement direction
			bool wasMovingLeft = player.IsMovingLeft();
			bool wasMovingRight = player.IsMovingRight();
			bool wasMovingUp = player.IsMovingUp();
			bool wasMovingDown = player.IsMovingDown();
			
			player.SetReachTargetPosition(destination.position);
			
	
			if(wasMovingLeft) {

				
				Debug.Log("########################### YES WAS MOVING LEFT #############################");
				Debug.Log("TELETRANSPORT SLIDE LEFT");
				
				player.SetIsMovingBetweenTeleportPoints(false);

				if(!overridePreviousDirection) {
					player.canMoveLeft = true;
					player.SlideLeft();
				}
				else {
					player.canMoveLeft = false;
					SlidePlayerIntoDirection(player);
				}

			}
			else if(wasMovingRight) {
				Debug.Log("########################### YES WAS MOVING RIGHT #############################");
				Debug.Log("TELETRANSPORT SLIDE RIGHT");
				player.SetIsMovingBetweenTeleportPoints(false);

				if (!overridePreviousDirection)
				{
					player.canMoveRight = true;
					player.SlideRight();

				}
				else {
					player.canMoveRight = false;
					SlidePlayerIntoDirection(player);
				}
	
			}
			else if(wasMovingDown) {
				Debug.Log("########################### YES WAS MOVING DOWN #############################");
				Debug.Log("TELETRANSPORT SLIDE DOWN");
				player.SetIsMovingBetweenTeleportPoints(false);

				if (!overridePreviousDirection) {
					player.canMoveDown = true;
					player.SlideDown();
				}
				else {
					player.canMoveDown = false;
					SlidePlayerIntoDirection(player);
				}
				
			}
			else if(wasMovingUp) {
				Debug.Log("########################### YES WAS MOVING UP #############################");
				Debug.Log("TELETRANSPORT SLIDE UP");
				player.SetIsMovingBetweenTeleportPoints(false);

				if (!overridePreviousDirection)
				{
					player.canMoveUp = true;
					player.SlideUp();
				}
				else {
					player.canMoveUp = false;
					SlidePlayerIntoDirection(player);
				}
				
			}

		}
		
		

	}

	//slide into exist direction
	private void SlidePlayerIntoDirection(PlayerMovement player) {
		if(exitDirection.Equals(GameConstants.LEFT_DIRECTION)) {
			player.canMoveLeft = true;
			player.SlideLeft();
		}
		else if(exitDirection.Equals(GameConstants.RIGHT_DIRECTION)) {
			player.canMoveRight = true;
			player.SlideRight();
		}
		else if(exitDirection.Equals(GameConstants.UP_DIRECTION)) {
			player.canMoveUp = true;
			player.SlideUp();
		}
		else if(exitDirection.Equals(GameConstants.DOWN_DIRECTION)) {
			player.canMoveDown = true;
			player.SlideDown();
		}
	}
}
