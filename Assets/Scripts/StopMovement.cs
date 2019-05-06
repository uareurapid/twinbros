using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StopMovement : MonoBehaviour, HandlePlayerCollision {

	//for how long does it stop movement?
	public float duration = 2f;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	private void ReEnableCollider() {

		Collider2D col = GetComponent<Collider2D>();
		if(col!=null) {
			col.enabled = true;
		}
	}
	public void HandleExitCollision(PlayerMovement player) {
		ReEnableCollider();
	}

	public void HandleCollision(PlayerMovement player)
	{
		Collider2D col = GetComponent<Collider2D>();
		if(col!=null) {
			col.enabled = false;
		}

		player.SetIsMovingBetweenTeleportPoints(true);
		player.StopMovementVelocity();
		//move it to the center of the stop sign
		Vector3 pos = transform.position;
		Vector3 playerPos = player.transform.position;
		player.transform.position = new Vector3(pos.x, pos.y, playerPos.z);

		StartCoroutine(RestartMovement(
			player.IsMovingLeft(),
			player.IsMovingRight(),
			player.IsMovingUp(),
			player.IsMovingDown(),
			player
		));
	}

	IEnumerator RestartMovement(bool left, bool right, bool up, bool down, PlayerMovement player) {
		yield return new WaitForSeconds(duration);
		//continue the movement
		player.SetIsMovingBetweenTeleportPoints(false);
		if(left) {
			player.canMoveLeft = true;
			player.SlideLeft();
		}
		else if(right) {
			player.canMoveRight = true;
			player.SlideRight();
		}
		else if(up) {
			player.canMoveUp = true;
			player.SlideUp();
		}
		else if(down) {
			player.canMoveDown = true;
			player.SlideDown();
		}

		//in this case, if i disable the collider the OnColliderExit2D of the handle is not called
		//so it needs to be done here
		yield return new WaitForSeconds(1f);
		ReEnableCollider();
		
	}
}
