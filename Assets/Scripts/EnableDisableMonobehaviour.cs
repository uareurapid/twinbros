using UnityEngine;
using System.Collections;
/**
* Enables/Disabled a given script
*/
public class EnableDisableMonobehaviour : MonoBehaviour, ResetBehaviourScript {

	public MonoBehaviour behaviour;
	public float delay = 0f;
	public float interval = 0f;

	public bool oneShot = false; //only do it once?

	public bool onlyOnCollision = false; //only start after a collision?
	public string collisionTag = "Player"; //used in conjuction with the above

	private bool isEnabled = false;
	private bool originalStatus = false;
	// Use this for initialization
	void Start () {
	  isEnabled = (behaviour!=null && behaviour.enabled);
	  originalStatus = isEnabled;

		Debug.Log("is ENABLED: -----> " + isEnabled + " this->" + gameObject.name);
	  //we wanto to reset the MonoBehaviour to the original status as well 
	  //the monobehaviour script must the the 1st on the list if the object has multiple scripts attached
	  //LevelManager.Instance.AddResetableBehaviourObject(this);

	  StartActions();
	  
	}

	void StartActions() {
	  if(!onlyOnCollision) {
		if(oneShot) {
			Invoke("EnableOrDisable", delay);
		}
		else {
			InvokeRepeating("EnableOrDisable", delay, interval);
		}
	  }
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void EnableOrDisable() {
     if(behaviour!=null) {
		if(isEnabled) {

			isEnabled = false;
			behaviour.enabled = false;
		}
		else {
			isEnabled = true;
			behaviour.enabled = true;
		}
     }
	}


	void OnTriggerEnter2D(Collider2D other) {
Debug.Log("FUCK ME OnTriggerEnter2D THIS IS THE ONE " + other.gameObject.tag);
        if(other.gameObject.CompareTag(collisionTag)) {
			Debug.Log("FUCK ME OnTriggerEnter2D THIS IS THE ONE " + other.gameObject.tag);
			PerformEnterActions();
        }

    }

	void OnCollisionEnter2D(Collision2D other) {
Debug.Log("FUCK ME OnCollisionEnter2D THIS IS THE ONE " + other.gameObject.tag);
        if(other.gameObject.CompareTag(collisionTag)) {
			Debug.Log("FUCK ME OnCollisionEnter2D THIS IS THE ONE " + other.gameObject.tag);
			PerformEnterActions();
        }
    }

	void PerformEnterActions() {
		if(oneShot) {
			Invoke("EnableOrDisable", delay);
		}
		else {
			InvokeRepeating("EnableOrDisable", delay, interval);
		}
	}

	public void ResetOriginalBehaviour() {
		CancelInvoke("EnableOrDisable");
		isEnabled = originalStatus;
		StartActions(); //start all over again
	}
}
