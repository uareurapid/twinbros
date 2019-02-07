using UnityEngine;
using System.Collections;

public class ObjectActivator : MonoBehaviour {

    public GameObject[] objectsToActivate;
	public bool onlyStartMovement = false;
	public bool onlyActivate = false;
	public bool enableEnemyScript =  false;
	public bool enableCollider2D =  false;
	public bool enableGravityScale =  false;

	public bool isNextLevelActivator = false;


	public bool enableImmediatellyAfterDelay = false;

	public float delay = 0f;

	public bool deactivateItself = false;

	public bool onlyEnableAnimation = false;

	public bool onlyMakeShake = false;
	//the collider object tag we are listening for 
	public string colliderObjectTag = "Player";
	// Use this for initialization
	void Start () {
		if(enableImmediatellyAfterDelay && delay > 0f) {
			Invoke("PrepareEnterActions", delay);
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void PerformExitActions() {

	}


	void PerformEnterActions()
	{
		//for the door triggers
		if (deactivateItself )
		{
			Collider2D col = GetComponent<Collider2D>();
			if(col!=null) {
				col.enabled = false;
			}

		}
	  foreach(GameObject obj in objectsToActivate) {

            if(obj!=null) {

			 

             

			 

              /*if(enableEnemyScript) {
				//GiveDamageToPlayer obst =obj.GetComponent<GiveDamageToPlayer>();
                //if(obst!=null) {
                //  obst.enabled = true;
                } 
              }*/

			  if(enableCollider2D) {
               Collider2D col =obj.GetComponent<Collider2D>();
               if(col!=null) {	
                 col.enabled = true;
               }
              }

			  if(onlyActivate) {
                obj.SetActive(true);
				if(obj.GetComponent<ParticleSystem>()!=null) {
					//for particles need to call play
					obj.GetComponent<ParticleSystem>().Play();
				}
              }

              if(enableGravityScale) {

                Rigidbody2D body = obj.GetComponent<Rigidbody2D>();
                if(body!=null) {
                  body.gravityScale = 1f;
					Debug.Log("ENABLE gravity scale: make door fall");
                }

				
              }

			

            

			 if(onlyEnableAnimation) {
				Animator anim = obj.GetComponent<Animator>();
				//play on enable
				anim.enabled = true;
				
			 }


		    }//end if !=null


          }//end foreach
	}
    
    IEnumerator DoPauseBeforeAction() {
		yield return new WaitForSeconds(delay);
		PerformEnterActions();
	}

	void PrepareEnterActions() {


		if(delay > 0f) {
			//make a delay first
			StartCoroutine(DoPauseBeforeAction());
		}
		else {
			//do immediately
			PerformEnterActions();
		}

		/*if(isNextLevelActivator && onlyActivate && objectsToActivate.Length==2) {
			if(GameController.Instance.CanActivateNextLevelDoor()) {
				GameObject obj = objectsToActivate[0];
				obj.SetActive(true);	
			}
			else {
				//object 1 -> question mark , enable as well 
				GameObject obj = objectsToActivate[1];
				obj.SetActive(true);
			}

			return;
		}*/

	    /*if(GetComponent<SwitchAnimation>()!=null) {
	      GetComponent<SwitchAnimation>().Activate();
	    }*/

		
	}

	void OnTriggerEnter2D(Collider2D other) {
		Debug.Log("THIS TAG: " + colliderObjectTag + " OTHER TAG: " + other.gameObject.tag);
        if(other.gameObject.CompareTag(colliderObjectTag)) {
			PrepareEnterActions();
        }

    }

	void OnCollisionEnter2D(Collision2D other) {
		//Debug.Log("THIS TAG: " + colliderObjectTag + " OTHER TAG: " + other.gameObject.tag);
        if(other.gameObject.CompareTag(colliderObjectTag)) {
			PrepareEnterActions();
        }
    }
}
