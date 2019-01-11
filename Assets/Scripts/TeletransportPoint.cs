using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeletransportPoint : MonoBehaviour {


	public Transform destination;
	public bool continueMovingSameDirection = true;

	public void Teletransport(PlayerMovement player) {

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
				//player.EnableColliders();
				player.SetIsMovingBetweenTeleportPoints(false);
				player.canMoveLeft = true;
				player.SlideLeft();
				//player.AllowAllMovementsAgainV2();
			}
			else if(wasMovingRight) {
				Debug.Log("########################### YES WAS MOVING RIGHT #############################");
				Debug.Log("TELETRANSPORT SLIDE RIGHT");
				player.SetIsMovingBetweenTeleportPoints(false);
				player.canMoveRight = true;
				player.SlideRight();
				//player.AllowAllMovementsAgain();
			}
			else if(wasMovingDown) {
				Debug.Log("########################### YES WAS MOVING DOWN #############################");
				Debug.Log("TELETRANSPORT SLIDE DOWN");
				player.SetIsMovingBetweenTeleportPoints(false);
				player.canMoveDown = true;
				player.SlideDown();
				//player.AllowAllMovementsAgain();
			}
			else if(wasMovingUp) {
				Debug.Log("########################### YES WAS MOVING UP #############################");
				Debug.Log("TELETRANSPORT SLIDE UP");
				player.SetIsMovingBetweenTeleportPoints(false);
				player.canMoveUp = true;
				player.SlideUp();
				//player.AllowAllMovementsAgain();
			}

		}
		
		

	}
}
