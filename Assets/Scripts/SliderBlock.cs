using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SliderBlock : MonoBehaviour, HandlePlayerCollision {

	public bool slideLeft = false;
	public bool slideRight = false;
	public bool slideUp = false;
	public bool slideDown = false;
    //only applies if 2 sliders
    public bool isDoubleSlider = false;
    public SliderBlock firstSlider; // the first is where it starts
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void HandleCollision(PlayerMovement player)
    {
        Slide(player);
    }

    public void HandleExitCollision(PlayerMovement player) {
        ReEnableCollider();
    }

    private void Slide(PlayerMovement player) {

        Collider2D col = GetComponent<Collider2D>();
        if (col != null)
        {
            col.enabled = false;
        }

        player.SetIsMovingBetweenTeleportPoints(true);
        player.StopMovementVelocity();
        //move it to the center of the stop sign
        Vector3 pos = transform.position;
        Vector3 playerPos = player.transform.position;
        player.transform.position = new Vector3(pos.x, pos.y, playerPos.z);

        /*if (player.IsMovingLeft())
        {

            player.collidedLeft();
        }
        else if (player.IsMovingRight())
        {

            player.collidedRight();
        }
        else if (player.IsMovingUp())
        {

            player.collidedTop();
        }
        else if (player.IsMovingDown())
        {

            player.collidedBottom();
        }
       
		if(slideRight) {
			player.canMoveRight = true;
			player.SlideRight();
		}
        else if (slideLeft)
        {
            player.canMoveLeft = true;
            player.SlideLeft();
        }
        else if (slideUp)
        {
            player.canMoveUp = true;
            player.SlideUp();
        }
        else if (slideDown)
        {
            player.canMoveDown = true;
            player.SlideDown();
        }*/
	

    StartCoroutine(RestartMovement(
            slideLeft,
            slideRight,
            slideUp,
            slideDown,
            player

        ));
    }

    IEnumerator RestartMovement(bool left, bool right, bool up, bool down, PlayerMovement player)
    {
        yield return new WaitForSeconds(0.5f);

        Debug.Log("left: " + left + " right " + right + " up " + up + " down " + down + " player left: " + player.isLeftTwin);
        //continue the movement
        player.SetIsMovingBetweenTeleportPoints(false);
        if (left)
        {
            player.canMoveLeft = true;
            player.SlideLeft();
        }
        else if (right)
        {
            Debug.Log("SLIDE RIGHT");
            player.canMoveRight = true;
            player.SlideRight();
        }
        else if (up)
        {
            player.canMoveUp = true;
            player.SlideUp();
        }
        else if (down)
        {
            player.canMoveDown = true;
            player.SlideDown();
        }

        //in this case, if i disable the collider the OnColliderExit2D of the handle is not called
        //so it needs to be done here
        yield return new WaitForSeconds(0.5f);
        ReEnableCollider();

    }

    private void ReEnableCollider()
    {

        Collider2D col = GetComponent<Collider2D>();
        if (col != null)
        {
            col.enabled = true;
        }
    }
}	
