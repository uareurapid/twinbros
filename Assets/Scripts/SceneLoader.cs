using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class SceneLoader : MonoBehaviour {

	public MoveWayPoint[] courtineDoors;
	public GameObject loadingDotsImage;

	public string nextScene = "";
	AsyncOperation asyncOperation;

    //load something automatic instead
    public bool automaticLoad = false;
    //used only if above is also set
    public float delay = 4f;
	//after how many passages move on?
	public int numDoorPassages = 1;
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
			StartCoroutine(LoadScene(levelManager));
		}
		
	}

    public void LoadNextSceneNoLevelManager()
    {
        if (nextScene != null && nextScene != "")
        {
            StartCoroutine(LoadSceneNoLevelManager());
        }

    }

	IEnumerator LoadScene(LevelManager levelManager) {

		loadingDotsImage.SetActive(true);
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
                
				if(courtineDoors[0].GetNumPassages()==numDoorPassages && courtineDoors[1].GetNumPassages()==numDoorPassages) {
					//Activate the Scene
					//scene is ready now
					Debug.Log("####################  scene is ready now");
                    asyncOperation.allowSceneActivation = true;
				}
                    
            }
			yield return null;
		}

		loadingDotsImage.SetActive(false);

        if(levelManager!=null) {
            levelManager.StartNextStage(); 
        }
		
		
	}


    IEnumerator LoadSceneNoLevelManager()
    {

		loadingDotsImage.SetActive(true);
        asyncOperation = SceneManager.LoadSceneAsync(nextScene);
        asyncOperation.allowSceneActivation = false;
		bool isDone = false;

        while (!asyncOperation.isDone && !isDone/* && !courtinesDone*/)
        {

            // Check if the load has finished
            if (asyncOperation.progress >= 0.9f)
            {
				isDone = true;
            }
            yield return null;
        }
		
        AudioSource audioS = GetComponent<AudioSource>();
		if(audioS) {
			audioS.Play();
		}
	
		loadingDotsImage.SetActive(false);

		yield return new WaitForSeconds(delay);
        asyncOperation.allowSceneActivation = true;

    }

	
}
