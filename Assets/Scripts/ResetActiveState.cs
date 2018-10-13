using System.Collections;
using System.Collections.Generic;
using UnityEngine;


	public class ResetActiveState : MonoBehaviour, ResetBehaviourScript
	{
		public bool initiallyActive = true;

		// Use this for initialization
		void Start()
		{

			GameObject scripts = GameObject.FindGameObjectWithTag("Scripts");
			if(scripts!=null) {

				LevelManager levelManager = scripts.GetComponent<LevelManager>();
				if(levelManager!=null) {
					levelManager.AddResetableBehaviourObject(this);
				}
				
			}
			
		}

		// Update is called once per frame
		void Update()
		{

		}

		public void ResetOriginalBehaviour()
		{
			gameObject.SetActive(initiallyActive);
		}
	}


