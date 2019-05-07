using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBox : MonoBehaviour, HandlePlayerCollision {

	public LevelManager levelmanager;
	public bool killPlayerOnTouch = true; //if not just dcrease a move or something

	public bool shrinkPlayer = false;
	private Tile associatedTile;
	//if moving do not count for the collision marging
	public bool isMovingEnemy = false;
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

	bool CannotIgnoreRightCollision(PlayerMovement player) {

		return (player.IsMovingRight() && !player.IsIgnoreCollision(transform.position.y, player.transform.position.y) && transform.position.x >= player.transform.position.x);
	}

	bool CannotIgnoreLeftCollision(PlayerMovement player)
	{
		return (player.IsMovingLeft() && !player.IsIgnoreCollision(transform.position.y, player.transform.position.y) && transform.position.x <= player.transform.position.x);
	}

	bool CannotIgnoreUpCollision(PlayerMovement player) {
		return (player.IsMovingUp() && !player.IsIgnoreCollision(transform.position.x, player.transform.position.x) && transform.position.y >= player.transform.position.y);
	}

	bool CannotIgnoreDownCollision(PlayerMovement player)
	{
		return (player.IsMovingDown() && !player.IsIgnoreCollision(transform.position.x, player.transform.position.x) && transform.position.y <= player.transform.position.y);
	}

	public void HandleExitCollision(PlayerMovement player) {

	}

	/*void OnCollisionEnter2D(Collision2D other) {

		if(other.gameObject.CompareTag("Player")) {
			Debug.Log("COLLLLLLLLLLLLLLLL " + gameObject.tag);
			HandleCollision(other.gameObject.GetComponent<PlayerMovement>());
		}
	}*/
	//bool isLeftMovement, bool isRightMovement, bool isUpMovement, bool isDownMovement
	public void HandleCollision(PlayerMovement player) {
		if(levelmanager==null) {
			GameObject scripts = GameObject.FindGameObjectWithTag("Scripts");
			levelmanager = scripts.GetComponent<LevelManager>();

		}

		//TODO if moving enemy should not be enough condition
		if (killPlayerOnTouch)
		{

			bool killed = false;
			//TODO WTF!!!

			if ( (isMovingEnemy && !player.IsStopped() ) || CannotIgnoreRightCollision(player))
			{
				levelmanager.KillPlayer();
				killed = true;
			}
			else if ( (isMovingEnemy && !player.IsStopped() ) || CannotIgnoreLeftCollision(player))
			{
				levelmanager.KillPlayer();
				killed = true;
			}
			else if ( (isMovingEnemy && !player.IsStopped() ) || CannotIgnoreUpCollision(player))
			{

				//if (!IsIgnoreCollision(other.transform.position.x, transform.position.x) && other.transform.position.y >= transform.position.y)
				levelmanager.KillPlayer();
				killed = true;
			}
			else if ( (isMovingEnemy && !player.IsStopped() ) || CannotIgnoreDownCollision(player))
			{
				levelmanager.KillPlayer();
				killed = true;
			}

			if(isMovingEnemy && killed) {
				MoveWayPoint move = GetComponent<MoveWayPoint>();
				if(move!=null) {
					//StopMovement
					move.PauseMovement();
				}
			}
		}
		else if (shrinkPlayer)
		{
			MakeShrinkEnemy shrinkScript = GetComponent<MakeShrinkEnemy>();
			Debug.Log("DO SHRNK IT?????");
			if (shrinkScript != null)
			{

				if (player.IsMovingUp())
				{
					player.collidedTop();
				}
				else if (player.IsMovingDown())
				{
					player.collidedBottom();
				}
				else if (player.IsMovingRight())
				{
					player.collidedRight();
				}
				else if (player.IsMovingLeft())
				{
					player.collidedLeft();
				}
				shrinkScript.enabled = true;
				shrinkScript.SetObjectToShrink(player, levelmanager, this);
			}
		}
		else Debug.Log("DO NOTHING???");
	}

	public void CallBack(MakeShrinkEnemy shrink) {
		shrink.enabled = false;
	}

}
