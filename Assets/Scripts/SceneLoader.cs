using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class SceneLoader : MonoBehaviour {


	public string nextScene = "";
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void LoadNextScene() {
		if (nextScene != null && nextScene != "")
		{
			StartCoroutine(LoadScene(nextScene));
		}
		
	}

	IEnumerator LoadScene(string scene) {

		AsyncOperation operation = SceneManager.LoadSceneAsync(nextScene); 
		while(!operation.isDone) {
			yield return null;
		}
		
	}
}
