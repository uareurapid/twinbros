using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class SceneLoader : MonoBehaviour {

	public MoveWayPoint[] courtineDoors;

	public string nextScene = "";
	AsyncOperation asyncOperation;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void LoadNextScene(LevelManager levelManager) {
		if (nextScene != null && nextScene != "")
		{
			StartCoroutine(LoadScene(nextScene,levelManager));
		}
		
	}

	IEnumerator LoadScene(string scene,LevelManager levelManager) {

		asyncOperation = SceneManager.LoadSceneAsync(nextScene);
		asyncOperation.allowSceneActivation = false;
	
		foreach(MoveWayPoint point in courtineDoors) {
			point.stopAfterXPassages = 2;
			point.justOnce = false;
			point.ContinueMovement();
		}
		while(!asyncOperation.isDone) {

			// Check if the load has finished
            if (asyncOperation.progress >= 0.9f)
            {
                
				if(courtineDoors[0].GetNumPassages()==1 && courtineDoors[1].GetNumPassages()==1) {
					//Activate the Scene
					//scene is ready now
					Debug.Log("####################  scene is ready now");
                    asyncOperation.allowSceneActivation = true;
				}
                    
            }
			yield return null;
		}
		
		
	}

	
}
