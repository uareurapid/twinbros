using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBox : MonoBehaviour {

	public LevelManager levelmanager;
	public bool killPlayerOnTouch = true; //if not just dcrease a move or something

	private Tile associatedTile;
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

	public void HandlePlayerCollision(bool isTopCollider,bool isRightCollider, bool isLeftCollider, bool isBottomCollider, PlayerMovement movement) {
		if(levelmanager==null) {
			GameObject scripts = GameObject.FindGameObjectWithTag("Scripts");
			levelmanager = scripts.GetComponent<LevelManager>();

		}

		//Debug.Log("---># ####### ENEMY COLLISION with isTop? " + isTopCollider + " isRight?" + isRightCollider + " isLeft?" + isLeftCollider + " isBottom?" + isBottomCollider); 
		if(killPlayerOnTouch && (isTopCollider && movement.IsMovingUp() ) || (isBottomCollider && movement.IsMovingDown() ) ||

		(isLeftCollider && movement.IsMovingLeft()) || (isRightCollider && movement.IsMovingRight()) ) {
			//TODO i am killing disregard of the direction  of movmenet CHECKE!!!!!!
			levelmanager.KillPlayer();
		}
	}

	/*
	void OnCollisionEnter2D(Collision2D other)
	{

		if(other.transform.CompareTag("Player") && killPlayerOnTouch && levelmanager!=null &&
			!levelmanager.isPlayerDead()) {

			levelmanager.KillPlayer();
		}
		
	}*/
}
