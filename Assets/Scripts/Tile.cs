using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour, HandlePlayerCollision {

	public Tile left;
	public Tile right;
	public Tile top;
	public Tile bottom;

	public bool blockLeftMovement = true;
	public bool blockRightMovement = true;
	public bool blockDownMovement = true;
	public bool blockUpMovement = true;
	public bool isWalkable = true;

	public bool hasPlayer = false;

	public float bounceAdjustment = 0.045f; //half of player bounce = 0.09

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public bool hasLeftTile() {
		return left != null;
	}

	public bool hasRightTile() {
		return right != null;
	}

	public bool hasTopTile() {
		return top != null;
	}

	public bool hasBottomTile() {
		return bottom != null;
	}

	public void HandleCollision(PlayerMovement movement) {

		bool ignoredCollision = true; 

			EnemyBox enemy = gameObject.GetComponent<EnemyBox>();
			if(enemy!=null) {
				enemy.HandleCollision(movement);
			}
						 
			
			if(blockUpMovement) {
				//Debug.Log("################# BLOCK UP ##################");
				if(movement.IsMovingUp()) {
					movement.collidedTop(this.gameObject);
					ignoredCollision = false;
				}
				else {
					movement.canMoveUp = false;
				}
				
			}
			else if(blockDownMovement) {
				//Debug.Log("################# BLOCK DOWN ##################");
				if(movement.IsMovingDown()) {
					movement.collidedBottom(this.gameObject);
					ignoredCollision = false;
				}
				else {
					movement.canMoveDown = false;
				}
				
			
			}
			else if(blockLeftMovement) {
				//Debug.Log("################# BLOCK LEFT ##################");
				if(movement.IsMovingLeft()) {
					movement.collidedLeft(this.gameObject);
					ignoredCollision = false;
				}
				else {
					movement.canMoveLeft = false;
				}
				
				
			}
			else if(blockRightMovement) {
				//Debug.Log("################# BLOCK RIGHT ##################");
				if(movement.IsMovingRight()) {
					movement.collidedRight(this.gameObject);
					ignoredCollision = false;
				}
				else {
					movement.canMoveRight = false;
				}
				
			}

		if(!ignoredCollision) {
			SpecialEffectsHelper.Instance.PlayImpactEffect(movement.transform.position);
		}

	}	

	public void HandleExitCollision(PlayerMovement movement) {


			/*if(isWalkable) {

				Collider2D col = GetComponent<Collider2D>();
				if(col!=null) {
					col.enabled = true;
				}
			}*/

			//Debug.Log("################# TIle HandleTileExitCollisions ################## " + gameObject.ToString());


			if(blockUpMovement && movement.IsMovingDown()) {
				//Debug.Log("################# ALLOW UP ##################");
				movement.canMoveUp = true;
			}
			if(blockDownMovement && movement.IsMovingUp()) {
				//Debug.Log("################# ALLOW DOWN ##################");
				movement.canMoveDown = true;
			
			}
			if(blockLeftMovement && movement.IsMovingRight()) {
				//Debug.Log("################# ALLOW LEFT " + movement.isLeftTwin + "##################");
				movement.canMoveLeft = true;
				
			}
			if(blockRightMovement && movement.IsMovingLeft()) {
				//Debug.Log("################# ALLOW RIGHT ##################");

				movement.canMoveRight = true;
			}
	}

}
