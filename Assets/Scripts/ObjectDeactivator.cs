using UnityEngine;
using System.Collections;
//using MoreMountains.CorgiEngine;

public class ObjectDeactivator : MonoBehaviour {

    public GameObject[] objectsToDeactivate;
    public bool completelyDestroyObjects = false;
	public bool onlyStopMovement = false;
	public bool onlyDeactivate = false;
	public bool disableEnemyScript =  false;
	public bool disableCollider2D =  false;

    //remove kinematic setting
	//TODO do teh same for object activator
	public bool disableKinematic =  false;

	//the collider object tag we are listening for 
	public string colliderObjectTag = "Player";
	// Use this for initialization

	public bool onlyOnCollision = false;

	public float delay = 0f;
	void Start () {
		if(delay > 0f && !onlyOnCollision) {
			PerformEnterActions();
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void PerformExitActions() {

	}

	void PerformEnterActions() {
	    /*if(GetComponent<SwitchAnimation>()!=null) {
	      GetComponent<SwitchAnimation>().Activate();
	    }*/

		foreach(GameObject obj in objectsToDeactivate) {

            if(obj!=null) {

              /*if(onlyStopMovement) {
                PathMovement mov = obj.GetComponent<PathMovement>();
                if(mov!=null) {
						Debug.Log("DEACTIVATE movement");		
                  mov.stopMovement = true;
                }
              }*/

              /*if(disableEnemyScript) {
               ObstacleScript obst =obj.GetComponent<ObstacleScript>();
               if(obst!=null) {
                 obst.enabled = false;
               }
              }*/

			  if(disableCollider2D) {
               Collider2D col =obj.GetComponent<Collider2D>();
               if(col!=null) {
                 col.enabled = false;
               }
              }

			  if(onlyDeactivate) {
                obj.SetActive(false);
              }

			  if(completelyDestroyObjects) {
                Destroy(obj);
              }

			  if(disableKinematic) {
					Rigidbody2D body = obj.GetComponent<Rigidbody2D>();
					if(body!=null) {
						body.isKinematic = false;
					}
			  }

		    }//end if !=null


          }//end foreach
	}

	void OnTriggerEnter2D(Collider2D other) {
        if(other.gameObject.CompareTag(colliderObjectTag)) {

			if(delay > 0f){
				StartCoroutine(StartActionAfterDelay());
			}
			else {
				PerformEnterActions();
			}
			
        }

    }

	void OnCollisionEnter2D(Collision2D other) {
        if(other.gameObject.CompareTag(colliderObjectTag)) {
			if(delay > 0f){
				StartCoroutine(StartActionAfterDelay());
			}
			else {
				PerformEnterActions();
			}
        }
    }

	IEnumerator StartActionAfterDelay() {
		yield return new WaitForSeconds(delay);
		PerformEnterActions();
	}
}
