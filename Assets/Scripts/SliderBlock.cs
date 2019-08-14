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
    
        if(isDoubleSlider) {
        
            if(firstSlider!=null) {
               //collided with 2nd slider, just slide normally 
               DoSlideAction(player);
            } else {
                //collided with first slider, need to get the 2nd one and put the player there
                //and after that call the DoSlideAction() on that one
                SliderBlock [] blocks = transform.parent.GetComponentsInChildren<SliderBlock>();
                if(blocks.Length > 1) {
                
                  //put it in the center/same position of the slider he collided with
                  player.transform.position = new Vector3(transform.position.x, transform.position.y, player.transform.position.z);
                
                  //now look for the outside most slider and make it slide from there
                  foreach(SliderBlock block in blocks) {
                  
                    //the 2nd is the one that has first slider set
                    if(block.firstSlider != null) {
                        Debug.Log("DEBUG: DELEGATE TO THE 2ND SLIDER");
                        block.DoSlideAction(player);
                    }
                  }
                } else {
                    Debug.Log("DEBUG: WTF SLIDER NOT DOUBLE?");
                }
            }
          
        } else {
            //proceed normally
            DoSlideAction(player);
            
        }

        
    }
    
    public void DoSlideAction(PlayerMovement player) {
    
            Collider2D col = GetComponent<Collider2D>();
            if (col != null)
            {
                col.enabled = false;
            }
    
            player.SetIsMovingBetweenTeleportPoints(true);
            player.StopMovementVelocity();
            //move it to the center of the stop sign
            Vector3 pos = transform.position;
         
            player.transform.position = new Vector3(pos.x, pos.y, player.transform.position.z);
    
            Debug.Log("DEBUG: PLAYER LOCAL POS: " + player.transform.localPosition + " GLOBAL POS: " + player.transform.position);
            
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

        Debug.Log("DEBUG: SLIDER RestartMovement left: " + left + " right " + right + " up " + up + " down " + down + " player left: " + player.isLeftTwin);
        //continue the movement
        player.SetIsMovingBetweenTeleportPoints(false);
        if (left)
        {
            player.canMoveLeft = true;
            player.SlideLeft();
        }
        else if (right)
        {
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
