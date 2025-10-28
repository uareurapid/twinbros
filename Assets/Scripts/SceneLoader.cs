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
	//for auto load only
	public float automaticDelay = 0; 
	//after how many passages move on?
	public int numDoorPassages = 1;

	public AudioClip soundToPlayOnLoad;

	public GameObject[] disableObjectsBeforeLoad;

    public bool isLastStage = false;

	// Use this for initialization
	void Start () {

		if(soundToPlayOnLoad!=null) {
			SoundEffectsHelper.Instance.PlayGenericSound(soundToPlayOnLoad);
		}
        if(automaticLoad && delay > 0f) {

			if(automaticDelay > 0) {
				Invoke("LoadNextSceneNoLevelManager", automaticDelay);
			}
			else {
				LoadNextSceneNoLevelManager();
			}
            
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

	public void LoadAchievementsUI()
	{
		Debug.Log("INSIDE SceneLoader LoadAchievementsUI()");
		SceneSwitcher.LoadSceneOnTop("Achievements");
	}

	public void LoadLevelSelectionUI()
	{
		Debug.Log("INSIDE SceneLoader LoadAchievementsUI()");
		SceneSwitcher.LoadSceneOnTop("LevelSelection");
	}

	public void LoadLeaderboardsUI()
	{
		SceneSwitcher.LoadSceneOnTop("LeaderBoards");
	}

	public void CloseCurrentScene()
	{
		Debug.Log("CloseCurrentScene()");
		SceneSwitcher.UnLoadCurrentSceneFromTop();
    }
	

	IEnumerator LoadScene(LevelManager levelManager) {

		//used on credits mostly
		if(disableObjectsBeforeLoad.Length > 0) {
			foreach(GameObject obj in disableObjectsBeforeLoad) {
				obj.SetActive(false);
			}
		}
		if(loadingDotsImage!=null) {
			loadingDotsImage.SetActive(true);
		}
		
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
					//Debug.Log("####################  scene is ready now");
                    asyncOperation.allowSceneActivation = true;
				}
                    
            }
			yield return null;
		}

		if (loadingDotsImage != null)
		{
			loadingDotsImage.SetActive(false);
		}

        if(levelManager!=null) {
            levelManager.StartNextStage(); 
        }
		
		
	}


	IEnumerator LoadSceneNoLevelManager()
	{
		//TODO check on other objects the usage, changed here for credits only
		yield return new WaitForSeconds(delay);

		//if we are on the credits screen remove this image to show the loading dots one
		GameObject credits = GameObject.FindGameObjectWithTag("Credits");
		if (credits != null)
		{
			credits.SetActive(false);
		}


		if (loadingDotsImage != null)
		{
			loadingDotsImage.SetActive(true);
		}
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
		if (audioS)
		{
			audioS.Play();
		}

		if (loadingDotsImage != null)
		{
			loadingDotsImage.SetActive(false);
		}
		asyncOperation.allowSceneActivation = true;
		//yield return new WaitForSeconds(delay);

	}


	public IEnumerator LoadSceneByName(string sceneName, int fromLevel) {

		if (loadingDotsImage != null)
		{
			loadingDotsImage.SetActive(true);
		}

		PlayerPrefs.SetInt(GameConstants.CURRENT_STAGE_OR_LEVEL, fromLevel);
		asyncOperation = SceneManager.LoadSceneAsync(sceneName);
		asyncOperation.allowSceneActivation = false;
	

		while(!asyncOperation.isDone) {

			// Check if the load has finished
            if (asyncOperation.progress >= 0.9f)
            {

				//Activate the Scene
				//scene is ready now
				//Debug.Log("####################  scene is ready now");
                asyncOperation.allowSceneActivation = true;
				 
            }
			yield return null;
		}

		if (loadingDotsImage != null)
		{
			loadingDotsImage.SetActive(false);
		}

		
	}
	
}
