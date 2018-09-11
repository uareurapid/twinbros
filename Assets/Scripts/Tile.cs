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

		Debug.Log("TILE COLLIDED WITH SOMETHING");
		
		
	}


	public void HandleTileCollisions(bool isTop, bool isRight, bool isLeft, bool isBottom, PlayerMovement movement) {


			Debug.Log("################# HandleTileCollisions ##################");
Debug.Log("################# is left: " + isLeft +" ##################");
Debug.Log("################# is right: " + isRight +" ##################");
Debug.Log("################# is Top: " + isTop +" ##################");
Debug.Log("################# is Bottom: " + isBottom +" ##################");

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
			}
	}

}
