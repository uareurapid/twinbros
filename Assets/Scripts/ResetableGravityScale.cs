using UnityEngine;
using System.Collections;

public class ResetableGravityScale : MonoBehaviour,ResetBehaviourScript {

    float initialGravityScale;
	public bool alsoOnBecameInvisible = false;//set back to start pos?
	private bool isVisible;
	// Use this for initialization
	void Start () {
	  initialGravityScale = GetComponent<Rigidbody2D>().gravityScale;
	  StartCoroutine(AddToResetableList());
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void ResetOriginalBehaviour() {
		GetComponent<Rigidbody2D>().gravityScale = initialGravityScale;
	}

	void OnBecameVisible() {
		isVisible = true;
	}

	void OnBecameInvisible() {
		if(isVisible && alsoOnBecameInvisible) {
			ResetOriginalBehaviour();
		}
		isVisible = false;
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
