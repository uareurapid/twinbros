using UnityEngine;

public class FollowThePath : MonoBehaviour {

    // Array of waypoints to walk from one to the next one
    [SerializeField]
    private Transform[] waypoints;

    // Walk speed that can be set in Inspector
    [SerializeField]
    private float moveSpeed = 2f;

    // Index of current waypoint from which Enemy walks
    // to the next one
    private int waypointIndex = 0;

	public bool isCircular = true;
	// Use this for initialization
	private void Start () {

        // Set position of Enemy as position of the first waypoint
        transform.position = waypoints[waypointIndex].transform.position;
	}
	
	// Update is called once per frame
	private void Update () {

        // Move Enemy
        Move();
	}

    // Method that actually make Enemy walk
    private void Move()
    {
        // If Enemy didn't reach last waypoint it can move
        // If enemy reached last waypoint then it stops
        if (waypointIndex <= waypoints.Length - 1)
        {

            Transform nextWayPoint = waypoints[waypointIndex];
            WayPoint info = nextWayPoint.GetComponent<WayPoint>();
            if(info!=null) {
                //use the way point speed to either accelerate or slow down
                transform.position = Vector2.MoveTowards(transform.position, waypoints[waypointIndex].transform.position, info.speedOut * Time.deltaTime);
                
            } else {
                //normal movement
                // Move Enemy from current waypoint to the next one
                // using MoveTowards method
                transform.position = Vector2.MoveTowards(transform.position, waypoints[waypointIndex].transform.position, moveSpeed * Time.deltaTime);
               
            }
            

            // If Enemy reaches position of waypoint he walked towards
            // then waypointIndex is increased by 1
            // and Enemy starts to walk to the next waypoint
            if (transform.position == waypoints[waypointIndex].transform.position)
            {
                waypointIndex += 1;
            }
        } else if(isCircular) {

			waypointIndex = 0;
		}
    }
}
