using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine;

public class LevelSelector : MonoBehaviour {

    public MoveWayPoint[] courtineDoors;
    AsyncOperation asyncOperation;

	public Sprite levelLockedImage;
	public Sprite levelUnlockedImage;

	public GameObject loadingDotsImage;

	public LevelStatus[] levels;
	// Use this for initialization
	void Start () {
		CheckLevelStatus();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	//TODO this should be done based on PlayerPrefs
	void CheckLevelStatus() {
		foreach(LevelStatus level in levels) {
			if(level.locked) {
				level.GetComponent<UnityEngine.UI.Image>().sprite = levelLockedImage;
			}
			else {
				level.GetComponent<UnityEngine.UI.Image>().sprite = levelUnlockedImage;
			}
		}
	}

    public void LoadStageOne() {

		if(!levels[0].locked) 
		{
			StartCoroutine(LoadScene("Level1"));
		}
        
    }

    public void LoadStageTwo()
    {
		if (!levels[1].locked)
		{
			StartCoroutine(LoadScene("Level2"));
		}
        
    }

    public void LoadStageThree()
    {
		if (!levels[2].locked)
		{
			StartCoroutine(LoadScene("Level3"));
		}
        
    }

    public void LoadStageFour()
    {
		if (!levels[3].locked)
		{
			StartCoroutine(LoadScene("Level4"));
		}
        
    }

    public void LoadStageFive()
    {

		if (!levels[4].locked)
		{
			StartCoroutine(LoadScene("Level5"));
		}
        
    }

    IEnumerator LoadScene(string scene)
    {
		if (loadingDotsImage != null)
		{
			loadingDotsImage.SetActive(true);
		}

        asyncOperation = SceneManager.LoadSceneAsync(scene);
        asyncOperation.allowSceneActivation = false;

        foreach (MoveWayPoint point in courtineDoors)
        {
            point.stopAfterXPassages = 2;
            point.justOnce = false;
            point.ContinueMovement();
        }
        while (!asyncOperation.isDone)
        {

            // Check if the load has finished
            if (asyncOperation.progress >= 0.9f)
            {

                if (courtineDoors[0].GetNumPassages() == 1 && courtineDoors[1].GetNumPassages() == 1)
                {
                    //Activate the Scene
                    //scene is ready now
                    Debug.Log("####################  scene is ready now");
                    asyncOperation.allowSceneActivation = true;
                }

            }
            yield return null;
        }
		if (loadingDotsImage != null)
		{
			loadingDotsImage.SetActive(false);
		}
		Invoke("Proceed", 1.0f);
    }

	void Proceed() {
		
        asyncOperation.allowSceneActivation = true;
	}
}
