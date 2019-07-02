using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//sets the original state of the attached MonoBehaviour scripts (either active or not)
public class ResetableBehaviours : MonoBehaviour, ResetBehaviourScript
	{
		List<MonoBehaviour> list;
		//saves teh original state (active or not) for each attached script
		Dictionary<string, bool> originalStates;
		// Use this for initialization
		void Start()
		{
			list = new List<MonoBehaviour>();
			originalStates = new Dictionary<string, bool>();
			//LevelManager.Instance.AddResetableBehaviourObject(this);
            
             StartCoroutine(AddToResetableList());
			
		}

		// Update is called once per frame
		void Update()
		{
			if(list.Count == 0) {
				//cannot do on start, not available yet
				foreach (MonoBehaviour behaviour in gameObject.GetComponents<MonoBehaviour>()) {
					string scriptName = behaviour.GetType().ToString();
					//Debug.Log("DEBUG: adding script with name : " + scriptName + " original state: " + behaviour.enabled);
					originalStates.Add(scriptName, behaviour.enabled);
                    list.Add(behaviour);
				}
			}
		}

		public void ResetOriginalBehaviour() {

			if (list.Count == 0) return;

			//get all the behaviours and set the sate to the same state saved at beginning
			foreach (MonoBehaviour behaviour in gameObject.GetComponents<MonoBehaviour>()) {

				string scriptName = behaviour.GetType().ToString();

				foreach (MonoBehaviour behaviourSaved in list) {

					string savedScriptName = behaviourSaved.GetType().ToString();
					
					if(scriptName.Equals(savedScriptName)) {

						
						bool savedValue = false;
						//if the key is present get the saved value for this script, otherwise get the current behaviour state
						if (originalStates.TryGetValue(savedScriptName, out savedValue))
						{
						
							
							//Debug.Log("FOUND ONE: " + scriptName + " now is? " + behaviour.enabled  + " but inititally was: " + savedValue);
							behaviour.enabled = savedValue;
							break;
						}
						
						
					}
				}
			}
			
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


