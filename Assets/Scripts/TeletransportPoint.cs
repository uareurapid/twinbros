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

			Debug.Log("%%%%%%%%%%%%%%%%%%% TELETRANSPORT $$$$$$$$$$$$$$$$$$$$");
			Collider2D col = destination.GetComponent<Collider2D>();
			if(col!=null) {
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
				player.SlideLeft();
				player.AllowAllMovementsAgainV2();
			}
			else if(wasMovingRight) {
				player.SetIsMovingBetweenTeleportPoints(false);
				player.SlideRight();
				player.AllowAllMovementsAgain();
			}
			else if(wasMovingDown) {
				player.SetIsMovingBetweenTeleportPoints(false);
				player.SlideDown();player.AllowAllMovementsAgain();
			}
			else if(wasMovingUp) {
				player.SetIsMovingBetweenTeleportPoints(false);
				player.SlideUp();
				player.AllowAllMovementsAgain();
			}

		}
		
		

	}
}
