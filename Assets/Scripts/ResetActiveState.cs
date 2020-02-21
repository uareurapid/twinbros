using System.Collections;
using System.Collections.Generic;
using UnityEngine;


	public class ResetActiveState : MonoBehaviour, ResetBehaviourScript
	{
		public bool initiallyActive = true;

		// Use this for initialization
		void Start()
		{

			StartCoroutine(AddToResetableList());
			
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

		public void ResetOriginalBehaviour()
		{
			if(gameObject!=null) {
				gameObject.SetActive(initiallyActive);

				//TODO this reset the bomb to initial state and animation,
				//but should be done with a reset bomb script or something
				if(gameObject.GetComponent<Bomb>()!=null) {
					gameObject.GetComponent<Bomb>().ResetOriginalBehaviour();
				}
			}
		}
	}


