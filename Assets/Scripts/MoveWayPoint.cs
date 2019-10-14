using UnityEngine;
using System.Collections;

public class MoveWayPoint : MonoBehaviour
{
	public WayPoint[] wayPoints;
	public float speed = 3f;
	public bool isCircular;
	// Always true at the beginning because the moving object will always move towards the first waypoint
	public bool inReverse = true;

	private WayPoint currentWaypoint;
	private int currentIndex   = 0;
	private bool isWaiting     = false;
	private float speedStorage = 0;

	private int numPassages = 0;
	public int stopAfterXPassages = 0;

	//do it just once?
	public bool justOnce = false;
	public bool isPaused = false;

	public float delay = 0f;

	public bool doMoveOnlyOnEnable = false;
	/**
	 * Initialisation
	 * 
	 */
	void Start () {
		numPassages = 0;

		if(!doMoveOnlyOnEnable) {
			if(delay > 0f) {
				Invoke("StartMovement", delay);
			}
			else {
				StartMovement();
			}
		}

		
		
	}

	private void StartMovement() {

		currentIndex = 0;
		if(wayPoints.Length > 0) {
			currentWaypoint = wayPoints[currentIndex];
			isPaused = false;
			//Debug.Log("START MOVEMENT!!!");
		}
	}
	
	public bool IsPaused() {

		return isPaused;
	}

	void OnEnable() {
		if(doMoveOnlyOnEnable) {
			if(delay > 0f) {
				Invoke("StartMovement", delay);
			}
			else {
				StartMovement();
			}
		}
	}

	/**
	 * Update is called once per frame
	 * 
	 */
	void Update()
	{
		if(currentWaypoint != null && !isWaiting && !isPaused) {
			MoveTowardsWaypoint();
		}
		else if(isPaused) {
			transform.Translate(new Vector3(0,0,0),Space.World);
		}
	}


	public void PauseMovement() {
		Debug.Log("PauseMovement CALLED %%%%%%%");
		isPaused = true;
		speedStorage = speed;
		speed = 0;
	}
	/**
	 * Pause the mover
	 * 
	 */
	public void Wait()
	{
		isWaiting = !isWaiting;
	}


	public void ContinueMovement() {
        if(!isPaused)
        {
            return; //already movinng
        }
		Debug.Log("ContinueMovement");
		numPassages = 0;
		isPaused = false;
		speed = speedStorage;
		StartMovement();
	}

    public void RestartMovementAfterPause(float delayBeforeRestart)
    {
        if (isPaused)
        {
            Invoke("ContinueMovement", delayBeforeRestart);
        }
        
    }
	
	/**
	 * Move the object towards the selected waypoint
	 * 
	 */
	private void MoveTowardsWaypoint()
	{
		//Debug.Log("MoveTowardsWaypoint");

	 if(!isPaused) {
			// Get the moving objects current position
		Vector3 currentPosition = transform.position;
		
		// Get the target waypoints position
		Vector3 targetPosition = currentWaypoint.transform.position;

		float distance = Vector3.Distance(currentPosition, targetPosition);
		// If the moving object isn't that close to the waypoint
		if( distance > 0.1f) { //TODO was 0.05

			// Get the direction and normalize
			Vector3 directionOfTravel = targetPosition - currentPosition;
			directionOfTravel.Normalize();
			//the multiplier is to avoid big jumps
			float multiplier = 1f;
			if(distance < 0.05) {
				multiplier = 0.01f;
			}
			//scale the movement on each axis by the directionOfTravel vector components
			if(!isPaused) {
				transform.Translate(
				directionOfTravel.x * speed * Time.deltaTime * multiplier,
				directionOfTravel.y * speed * Time.deltaTime * multiplier,
				directionOfTravel.z * speed * Time.deltaTime * multiplier,
				Space.World);
			}
			else {
				//stop movement if paused
				transform.Translate(new Vector3(0,0,0));
			}
		} else {

			numPassages++;
			//	Debug.Log("num passages: " + numPassages);
			if( (justOnce  && currentIndex == wayPoints.Length - 1 )  || (numPassages == stopAfterXPassages && stopAfterXPassages > 0) ) {
				isPaused = true;
				return;
            } 
            else if (wayPoints[currentIndex].isDirectTeleport)
            {
                //pause it
                isPaused = true;
                transform.Translate(new Vector3(0, 0, 0));
                transform.position = wayPoints[currentIndex].transform.position; //put on the other one
                NextWaypoint();
                Debug.Log("CURRENT TELEPORT? " + currentWaypoint.isDirectTeleport);
                isPaused = false;
            }

			//On wave point now


			// If the waypoint has a pause amount then wait a bit
			if(currentWaypoint.waitSeconds > 0f) {
				Wait();
				Invoke("Wait", currentWaypoint.waitSeconds);
			}

			// If the current waypoint has a speed change then change to it
			if(currentWaypoint.speedOut > 0) {
				speedStorage = speed;
				speed = currentWaypoint.speedOut;
			} else if(speedStorage >= 0f) {
				speed = speedStorage;
				speedStorage = 0;
			}

			if(!isPaused) {
				NextWaypoint();
			}
			
		}

	 }
		
	}



	/**
	 * Work out what the next waypoint is going to be
	 * 
	 */
	private void NextWaypoint()
	{
		if(isCircular) {
			
			if(!inReverse) {
				currentIndex = (currentIndex+1 >= wayPoints.Length) ? 0 : currentIndex+1;
			} else {
				currentIndex = (currentIndex == 0) ? wayPoints.Length-1 : currentIndex-1;
			}

		} else {
			
			// If at the start or the end then reverse
			if((!inReverse && currentIndex+1 >= wayPoints.Length) || (inReverse && currentIndex == 0)) {
				inReverse = !inReverse;
			}
			currentIndex = (!inReverse) ? currentIndex+1 : currentIndex-1;

		}

        //normal way
        currentWaypoint = wayPoints[currentIndex];

        //extension possibility to show hide sprite
        if(currentWaypoint.isHiddeSprite){
            Renderer ren = GetComponent<Renderer>(); 
            if(ren!=null){
                ren.enabled = false;  
            }
        }
        else if (currentWaypoint.isShowSprite)
        {
            Renderer ren = GetComponent<Renderer>();
            if (ren != null)
            {
                ren.enabled = true;
            }
        }
        
        if(currentWaypoint.isRevertSprite) {
              Vector3 theScale = transform.localScale;
              theScale.x *= -1;
              transform.localScale = theScale;
        }

		
	}

	public int GetNumPassages() {
		return numPassages;
	}
}