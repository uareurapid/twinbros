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

	//bool isLeftMovement, bool isRightMovement, bool isUpMovement, bool isDownMovement
	public void HandlePlayerCollision(PlayerMovement player) {
		if(levelmanager==null) {
			GameObject scripts = GameObject.FindGameObjectWithTag("Scripts");
			levelmanager = scripts.GetComponent<LevelManager>();

		}

		//TODO this is duplicated
		if(killPlayerOnTouch) {
			if(player.IsMovingRight() && !player.IsIgnoreCollision(transform.position.y, player.transform.position.y)) {
				levelmanager.KillPlayer();
			}
			else if(player.IsMovingLeft() && !player.IsIgnoreCollision(transform.position.y, player.transform.position.y)) {
				levelmanager.KillPlayer();
			}
			else if(player.IsMovingUp() && !player.IsIgnoreCollision(transform.position.x, player.transform.position.x)) {
				levelmanager.KillPlayer();
			}
			else if(player.IsMovingDown() && !player.IsIgnoreCollision(transform.position.x, player.transform.position.x)) {
				levelmanager.KillPlayer();
			}
			
		}
	}

}
