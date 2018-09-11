using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour {

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


	public void HandleTileCollisions(PlayerMovement movement) {


			Debug.Log("################# TIle HandleTileCollisions ################## " + gameObject.ToString());


			if(blockUpMovement) {
				Debug.Log("################# BLOCK UP ##################");
				if(movement.IsMovingUp()) {
					movement.collidedTop();
				}
				else {
					movement.canMoveUp = false;
				}
				
			}
			if(blockDownMovement) {
				Debug.Log("################# BLOCK DOWN ##################");
				if(movement.IsMovingDown()) {
					movement.collidedBottom();
				}
				else {
					movement.canMoveDown = false;
				}
				
			
			}
			if(blockLeftMovement) {
				Debug.Log("################# BLOCK LEFT ##################");
				if(movement.IsMovingLeft()) {
					movement.collidedLeft();
				}
				else {
					movement.canMoveLeft = false;
				}
				
				
			}
			if(blockRightMovement) {
				Debug.Log("################# BLOCK RIGHT ##################");
				if(movement.IsMovingRight()) {
					movement.collidedRight();
				}
				else {
					movement.canMoveRight = false;
				}
				
			}
	}

	public void HandleTileExitCollisions(PlayerMovement movement) {


			Debug.Log("################# TIle HandleTileExitCollisions ################## " + gameObject.ToString());


			if(blockUpMovement) {
				Debug.Log("################# ALLOW UP ##################");
				movement.canMoveUp = true;
			}
			if(blockDownMovement) {
				Debug.Log("################# ALLOW DOWN ##################");
				movement.canMoveDown = true;
			
			}
			if(blockLeftMovement) {
				Debug.Log("################# ALLOW LEFT ##################");
				movement.canMoveLeft = true;
				
			}
			if(blockRightMovement) {
				Debug.Log("################# ALLOW RIGHT ##################");

				movement.canMoveRight = true;
			}
	}

}
