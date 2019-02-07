using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBox : MonoBehaviour {

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

	//bool isLeftMovement, bool isRightMovement, bool isUpMovement, bool isDownMovement
	public void HandlePlayerCollision(PlayerMovement player) {
		if(levelmanager==null) {
			GameObject scripts = GameObject.FindGameObjectWithTag("Scripts");
			levelmanager = scripts.GetComponent<LevelManager>();

		}

		//TODO this is duplicated
		if (killPlayerOnTouch)
		{
			if (isMovingEnemy || CannotIgnoreRightCollision(player))
			{
				//(player.IsMovingRight() && !player.IsIgnoreCollision(transform.position.y, player.transform.position.y) && transform.position.x >= player.transform.position.x) 
				levelmanager.KillPlayer();
			}
			else if (isMovingEnemy || CannotIgnoreLeftCollision(player))
			{
				levelmanager.KillPlayer();
			}
			else if (isMovingEnemy || CannotIgnoreUpCollision(player))
			{

				//if (!IsIgnoreCollision(other.transform.position.x, transform.position.x) && other.transform.position.y >= transform.position.y)
				levelmanager.KillPlayer();
			}
			else if (isMovingEnemy || CannotIgnoreDownCollision(player))
			{
				levelmanager.KillPlayer();
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
