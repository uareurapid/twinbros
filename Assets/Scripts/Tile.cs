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

	void OnCollisionEnter2D(Collision2D other)
	{

		/*if(other.transform.CompareTag("Player"))
		Debug.Log("TILE COLLIDED WITH SOMETHING");
		if(blockUpMovement) {
				Debug.Log("################# BLOCK UP ##################");
				movement.canMoveUp = false;
				if(movement.IsMovingUp()) {
					movement.collidedTop();
				}
			}
			if(blockDownMovement) {
				movement.canMoveDown = false;
				Debug.Log("################# BLOCK DOWN ##################");
				if(movement.IsMovingDown()) {
					movement.collidedBottom();
				}
			}
			if(blockLeftMovement) {
				movement.canMoveLeft = false;
				Debug.Log("################# BLOCK LEFT ##################");
				if(movement.IsMovingLeft()) {
					movement.collidedLeft();
				}
			}
			if(blockRightMovement) {
			Debug.Log("################# BLOCK RIGHT ##################");
				movement.canMoveRight = false;
				if(movement.IsMovingRight()) {
					movement.collidedRight();
				}
			}*/
		
		
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
					movement.collidedTop();
					ignoredCollision = false;
				}
				else {
					movement.canMoveUp = false;
				}
				
			}
			else if(blockDownMovement) {
				//Debug.Log("################# BLOCK DOWN ##################");
				if(movement.IsMovingDown()) {
					movement.collidedBottom();
					//if(enemy!=null) {
					//	enemy.HandlePlayerCollision();
					//}
					ignoredCollision = false;
				}
				else {
					movement.canMoveDown = false;
				}
				
			
			}
			else if(blockLeftMovement) {
				//Debug.Log("################# BLOCK LEFT ##################");
				if(movement.IsMovingLeft()) {
					movement.collidedLeft();
					ignoredCollision = false;
				}
				else {
					movement.canMoveLeft = false;
				}
				
				
			}
			else if(blockRightMovement) {
				//Debug.Log("################# BLOCK RIGHT ##################");
				if(movement.IsMovingRight()) {
					movement.collidedRight();
					ignoredCollision = false;
				}
				else {
					movement.canMoveRight = false;
				}
				
			}

		if(!ignoredCollision) {
			SpecialEffectsHelper.Instance.PlayBoxCollisionEffect(movement.transform.position);
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


			if(blockUpMovement) {
				//Debug.Log("################# ALLOW UP ##################");
				movement.canMoveUp = true;
			}
			if(blockDownMovement) {
				//Debug.Log("################# ALLOW DOWN ##################");
				movement.canMoveDown = true;
			
			}
			if(blockLeftMovement) {
				//Debug.Log("################# ALLOW LEFT " + movement.isLeftTwin + "##################");
				movement.canMoveLeft = true;
				
			}
			if(blockRightMovement) {
				//Debug.Log("################# ALLOW RIGHT ##################");

				movement.canMoveRight = true;
			}
	}

}
