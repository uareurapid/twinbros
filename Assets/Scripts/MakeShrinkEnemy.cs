using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
* this is like a pithall, it spins the player and shrinks it before killing it
*/
public class MakeShrinkEnemy : MonoBehaviour {

	public float percentage = 25f;
	public float speed = 0.12f;
	// Use this for initialization
	private Vector3 currentScale;
	private Vector3 initialScale;
	private PlayerMovement playerToShrink;
	private LevelManager levelManager;
	private AutoRotate rotatePlayerScript;
	private Quaternion initialRotation;
	private EnemyBox enemyScript;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if(playerToShrink!=null && levelManager!=null && !levelManager.IsPlayerDead()) {

			currentScale = playerToShrink.gameObject.transform.localScale;
		
			//shrink
			if (currentScale.x  * (1.0f + (percentage/100f) ) > initialScale.x)
			{
				levelManager.SetIsDying(true);
				Shrink();
		
			}
			else {
				rotatePlayerScript.enabled = false;
                playerToShrink = null;
				//no more actions here
				levelManager.KillPlayer();
				levelManager = null;
				enemyScript.CallBack(this);
				GetComponent<Collider2D>().enabled = true;
				return;
			}
			playerToShrink.gameObject.transform.localScale = currentScale;
		}
		
	}

	public void SetObjectToShrink(PlayerMovement player, LevelManager levelManager, EnemyBox enemy) {

		Debug.Log("################ SetObjectToShrink ##################");
		playerToShrink = player;
		enemyScript = enemy;
		playerToShrink.SetReachTargetPosition(gameObject.transform.position);
		playerToShrink.DisableAllMovements();
		playerToShrink.StopMovementVelocity();
		GetComponent<Collider2D>().enabled = false;

		//attact it to the middle of the pit
		Vector3 newPosition = new Vector3(transform.position.x, transform.position.y, player.transform.position.z);
		playerToShrink.transform.position = newPosition;// transform.position;
		initialRotation = playerToShrink.transform.localRotation;
		initialScale = playerToShrink.transform.localScale;
		currentScale = initialScale;
		this.levelManager = levelManager;
		rotatePlayerScript = playerToShrink.GetComponent<AutoRotate>();
	}

	void Shrink() {
	  currentScale.x = currentScale.x - (currentScale.x * Time.deltaTime) * speed;
	  currentScale.y = currentScale.y - (currentScale.y * Time.deltaTime) * speed;

	  if(rotatePlayerScript!=null && !rotatePlayerScript.enabled) {
		rotatePlayerScript.enabled = true;
	  }
	}
}
