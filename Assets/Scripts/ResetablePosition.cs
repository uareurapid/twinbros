using UnityEngine;
using System.Collections;

public class ResetablePosition : MonoBehaviour, ResetBehaviourScript {
	
	    private Vector3 startPosition;
		private Quaternion startRotation;
		public bool alsoOnBecameInvisible = false;//set back to start pos?
		Vector2 velocity;
		Rigidbody2D body;

		private Transform parent;

		private Vector2 localPositionOnParent;

		private bool isVisible;
		// Use this for initialization
		void Start () {
		 startPosition = transform.position;
		 startRotation = transform.rotation;
		 body = gameObject.GetComponent<Rigidbody2D>();
		 if(body!=null) {
			velocity = body.velocity;	
		 }

		 if(transform.parent != null) {
			parent = transform.parent;
			localPositionOnParent = transform.localPosition;
		 }
	
		 //LevelManager.Instance.AddResetableBehaviourObject(this);
         //TODO CHECK WHY I HAD REMOVED IT ABOVE
         StartCoroutine(AddToResetableList());
         
		}
		
		// Update is called once per frame
		void Update () {
		
		}

		void OnBecameVisible() {
			isVisible = true;
		}

		void OnBecameInvisible() {
			if(isVisible && alsoOnBecameInvisible) {
				isVisible = false;
				ResetOriginalBehaviour();
			}
		}
	
		public void ResetOriginalBehaviour() {
		 if(transform!=null) {
			transform.position = startPosition;
			transform.rotation = startRotation;
		 }	
		 
	     //reset also the velocity if possible
		 if(body!=null) {
			body.velocity = velocity;		
		 }

		 if(parent!=null) {
			transform.parent = parent;
			transform.localPosition = localPositionOnParent;
		 }

		 //enable movement again
		 /*PathMovement movement = GetComponent<PathMovement>();
		 if(movement!=null) {

				
				if(movement.enabled && movement.CanMove == false && movement.GetOriginalEnabledState()) {
					movement.EnableMovement();
				}
				
				//movement.OnEnable();
				
		 }	*/
	
		}
        
        IEnumerator AddToResetableList() {
        yield return new WaitForSecondsRealtime(2f);
        GameObject scripts = GameObject.FindGameObjectWithTag("Scripts");
            if(scripts!=null) {

                LevelManager levelManager = scripts.GetComponent<LevelManager>();
                if(levelManager!=null) {
                    levelManager.AddResetableBehaviourObject(this);
                }
                
            }
        }
	}


