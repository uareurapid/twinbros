using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SideCollider : MonoBehaviour {


	public bool isLeft = false;
	public bool isRight = false;
	public bool isTop = false;
	public bool isBottom = false;

	private PlayerMovement movement;

	public Level associatedLevel; //can only move on the associated Level
	public LevelManager levelManager;
	// Use this for initialization
	void Start () {
		movement = GetComponentInParent<PlayerMovement>();
		associatedLevel = GetComponentInParent<Level>();
		GameObject scripts = GameObject.FindGameObjectWithTag("Scripts");
		levelManager = scripts.GetComponent<LevelManager>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnTriggerEnter2D(Collider2D other)
	{

		//only affects if on the same level
		if(levelManager.currentLevel == associatedLevel.level) {
				//portal collision
				if(other.transform.CompareTag("Portal")) {

					if(movement.GetIsMovingBetweenLevels()) {
						//ignore this collision
						return;
					}
					else {
						//TODO keep coding me
						levelManager.TwinCollidedWithPortal(transform.parent.gameObject);
						Portal portal = other.GetComponent<Portal>();
						portal.MoveToNextLevel();
					}
				
				}

				Tile tile = other.gameObject.GetComponent<Tile>();
				//if(tile!=null) {
				//	tile.HandleTileCollisions(isTop, isRight, isLeft, isBottom,movement);
				//}
				
				/*if(other.transform.CompareTag("Enemy") || other.GetComponent<EnemyBox>()!=null) {
					
					EnemyBox enemy = other.GetComponent<EnemyBox>();
					enemy.HandlePlayerCollision(isTop, isRight, isLeft, isBottom, movement);
					movement.SetIsTouchingEnemy(enemy);					
					
				}*/

				HandleCollisions();
		}

		
		
		
	}

	void HandleCollisions() {

		//ignore the sided collision if we are not moving on that direction
		if(isLeft && movement.IsMovingLeft()) {
			movement.collidedLeft();
		}
		if(isRight && movement.IsMovingRight()) {
			movement.collidedRight();
		}
		if(isTop && movement.IsMovingUp()) {
			movement.collidedTop();
		}
		if(isBottom && movement.IsMovingDown()) {
			movement.collidedBottom();
		}

		//return ignoreCollision;
	}

	void OnTriggerExit2D(Collider2D other) {

		if(movement.GetIsTouchingEnemy()) {

			movement.SetIsTouchingEnemy(null);
		}

		if(isLeft) {
			movement.AllowLeftMovementAgain();
		}
		else if(isRight) {
			movement.AllowRightMovementAgain();
		}
		else if(isTop) {
			movement.AllowUpMovementAgain();
		}
		else if(isBottom) {
			movement.AllowDownMovementAgain();
		}
	}
}
