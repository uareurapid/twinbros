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
	}



	/**
	 * Pause the mover
	 * 
	 */
	void Pause()
	{
		isWaiting = !isWaiting;
	}


	public void ContinueMovement() {
		isPaused = false;
		numPassages = 0;
		StartMovement();
	}
	
	/**
	 * Move the object towards the selected waypoint
	 * 
	 */
	private void MoveTowardsWaypoint()
	{
		// Get the moving objects current position
		Vector3 currentPosition = this.transform.position;
		
		// Get the target waypoints position
		Vector3 targetPosition = currentWaypoint.transform.position;
		
		// If the moving object isn't that close to the waypoint
		if(Vector3.Distance(currentPosition, targetPosition) > .1f) {

			// Get the direction and normalize
			Vector3 directionOfTravel = targetPosition - currentPosition;
			directionOfTravel.Normalize();
			
			//scale the movement on each axis by the directionOfTravel vector components
			this.transform.Translate(
				directionOfTravel.x * speed * Time.deltaTime,
				directionOfTravel.y * speed * Time.deltaTime,
				directionOfTravel.z * speed * Time.deltaTime,
				Space.World
			);
		} else {

			numPassages++;
			if( (justOnce  && currentIndex == wayPoints.Length - 1 )  || 
									(numPassages == stopAfterXPassages && stopAfterXPassages > 0) ) {
				isPaused = true;
				return;
			}

			//On wave point now
			
			// If the waypoint has a pause amount then wait a bit
			if(currentWaypoint.waitSeconds > 0) {
				Pause();
				Invoke("Pause", currentWaypoint.waitSeconds);
			}

			// If the current waypoint has a speed change then change to it
			if(currentWaypoint.speedOut > 0) {
				speedStorage = speed;
				speed = currentWaypoint.speedOut;
			} else if(speedStorage != 0) {
				speed = speedStorage;
				speedStorage = 0;
			}

			NextWaypoint();
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

		currentWaypoint = wayPoints[currentIndex];
	}

	public int GetNumPassages() {
		return numPassages;
	}
}