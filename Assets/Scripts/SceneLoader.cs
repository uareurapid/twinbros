using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class SceneLoader : MonoBehaviour {

	public MoveWayPoint[] courtineDoors;

	public string nextScene = "";
	AsyncOperation asyncOperation;

    //load something automatic instead
    public bool automaticLoad = false;
    //used only if above is also set
    public float delay = 2f;
	// Use this for initialization
	void Start () {
        if(automaticLoad && delay > 0f) {
            LoadNextSceneNoLevelManager();
        }
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void LoadNextScene(LevelManager levelManager) {
		if (nextScene != null && nextScene != "")
		{
			levelManager.DisableMusic();
			StartCoroutine(LoadScene(nextScene,levelManager));
		}
		
	}

    private void LoadNextSceneNoLevelManager()
    {
        if (nextScene != null && nextScene != "")
        {
            StartCoroutine(LoadSceneNoLevelManager(nextScene));
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

        if(levelManager!=null) {
            levelManager.StartNextStage(); 
        }
		
		
	}


    IEnumerator LoadSceneNoLevelManager(string scene)
    {

        asyncOperation = SceneManager.LoadSceneAsync(nextScene);
        asyncOperation.allowSceneActivation = false;
        bool courtinesDone = false;

        foreach (MoveWayPoint point in courtineDoors)
        {
            point.stopAfterXPassages = 2;
            point.justOnce = false;
            point.ContinueMovement();
        }
        while (!asyncOperation.isDone && !courtinesDone)
        {

            // Check if the load has finished
            if (asyncOperation.progress >= 0.9f)
            {

                if (courtineDoors[0].GetNumPassages() == 1 && courtineDoors[1].GetNumPassages() == 1)
                {
                    //Activate the Scene
                    //scene is ready now
                    courtinesDone = true;
                }

            }
            yield return null;
        }

        yield return new WaitForSeconds(delay);
        Debug.Log("####################  scene is ready now");
        asyncOperation.allowSceneActivation = true;

    }

	
}
