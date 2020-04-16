using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwipeDetector : MonoBehaviour
{
    private Vector2 fingerDown;
    private Vector2 fingerUp;
    public bool detectSwipeOnlyAfterRelease = true;

    public float SWIPE_THRESHOLD = 20f;
	//https://forum.unity.com/threads/swipe-in-all-directions-touch-and-mouse.165416/
	//https://gist.github.com/Fonserbc/ca6bf80b69914740b12da41c14023574

	public bool leftSwipe = false;
	public bool rightSwipe = false;
	public bool upSwipe = false;
	public bool downSwipe = false;
	// Update is called once per frame

	void Start()
	{
		//#if UNITY_ANDROID && !UNITY_EDITOR
		//	SWIPE_THRESHOLD = 15f;
		//#endif
	}
    void FixedUpdate()
    {

		if(Input.touches.Length == 1) {

			Touch touch = Input.touches[0];
        
            if (touch.phase == TouchPhase.Began)
            {
                fingerUp = touch.position;
                fingerDown = touch.position;
            }

            //Detects Swipe while finger is still moving
            if (touch.phase == TouchPhase.Moved && !detectSwipeOnlyAfterRelease)
            {
                fingerDown = touch.position;
                checkSwipe();
               
            }

            //Detects swipe after finger is released
            if (touch.phase == TouchPhase.Ended)
            {
                fingerDown = touch.position;
                checkSwipe();
            }
			else {
				OnNoSwipe();
			}
      }
	  else {
			OnNoSwipe();
	  }	

		
        
    }

    void checkSwipe()
    {
        //Check if Vertical swipe
        if (verticalMove() > SWIPE_THRESHOLD && verticalMove() > horizontalValMove())
        {
            //Debug.Log("Vertical");
            if (fingerDown.y - fingerUp.y > 0)//up swipe
            {
                OnSwipeUp();
            }
            else if (fingerDown.y - fingerUp.y < 0)//Down swipe
            {
                OnSwipeDown();
            }
            fingerUp = fingerDown;
        }

        //Check if Horizontal swipe
        else if (horizontalValMove() > SWIPE_THRESHOLD && horizontalValMove() > verticalMove())
        {
            //Debug.Log("Horizontal");
            if (fingerDown.x - fingerUp.x > 0)//Right swipe
            {
                OnSwipeRight();
            }
            else if (fingerDown.x - fingerUp.x < 0)//Left swipe
            {
                OnSwipeLeft();
            }
            fingerUp = fingerDown;
        }

        //No Movement at-all
        else
        {
            //Debug.Log("No Swipe!");
        }
    }

    float verticalMove()
    {
        return Mathf.Abs(fingerDown.y - fingerUp.y);
    }

    float horizontalValMove()
    {
        return Mathf.Abs(fingerDown.x - fingerUp.x);
    }

    //////////////////////////////////CALLBACK FUNCTIONS/////////////////////////////

	void OnNoSwipe() {
		upSwipe = false;
		downSwipe = false;
		leftSwipe = false;
		rightSwipe = false;
	}
    void OnSwipeUp()
    {
		upSwipe = true;
		downSwipe = false;
		leftSwipe = false;
		rightSwipe = false;
    }

    void OnSwipeDown()
    {
		upSwipe = false;
		downSwipe = true;
		leftSwipe = false;
		rightSwipe = false;
    }

    void OnSwipeLeft()
    {
		upSwipe = false;
		downSwipe = false;
		leftSwipe = true;
		rightSwipe = false;
    }

    void OnSwipeRight()
    {
		upSwipe = false;
		downSwipe = false;
		leftSwipe = false;
		rightSwipe = true;
    }
}
