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

    public UnityEngine.UI.Text levelLockedText;

    public Achievements achievements;

    public bool isTestMode = false;

    void Awake()
    {
        GameObject scripts = GameObject.FindGameObjectWithTag("Scripts");
        if (scripts != null)
        {
            achievements = scripts.GetComponent<Achievements>();
        }
        CheckLevelStatus();
    }
    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    //TODO this should be done based on PlayerPrefs
    void CheckLevelStatus()
    {
        foreach (LevelStatus level in levels)
        {
            if (level.number == 1)
            {
                level.locked = false;
            }
            // check player prefs
            else if (achievements != null)
            {
                level.locked = achievements.GetAchievement(GameConstants.ACHIEVEMENT_STAGE_GENERIC_ID + level.number) == 0;
            }

            // other logic
            if (level.locked)
            {
                level.GetComponent<UnityEngine.UI.Image>().sprite = levelLockedImage;
            }
            else
            {
                level.GetComponent<UnityEngine.UI.Image>().sprite = levelUnlockedImage;
            }
            
             if(isTestMode)
            {
                level.locked = false;
            }
            
        }
    }

    public void CloseCurrentScene()
    {
        int level = PlayerPrefs.GetInt(GameConstants.CURRENT_STAGE_OR_LEVEL, 1);
        StartCoroutine(LoadScene("Level" + level.ToString()));
    }

    public void LoadStageOne() {

		if(!levels[0].locked) 
		{
			StartCoroutine(LoadScene("Level1"));
		}
        else if(levelLockedText!=null)
        {
            levelLockedText.text = "Stage 1 is locked!";
            levelLockedText.enabled = true;
            StartCoroutine(HideLevelLockedText(1));
        }
    }

    public void LoadStageTwo()
    {
		if (!levels[1].locked)
		{
			StartCoroutine(LoadScene("Level2"));
		}
        else if (levelLockedText != null)
        {
            levelLockedText.text = "Stage 2 is locked!";
            levelLockedText.enabled = true;
            StartCoroutine(HideLevelLockedText(2));
        }

    }

    public void LoadStageThree()
    {
		if (!levels[2].locked)
		{
			StartCoroutine(LoadScene("Level3"));
		}
        else if (levelLockedText != null)
        {
            levelLockedText.text = "Stage 3 is locked!";
            levelLockedText.enabled = true;
            StartCoroutine(HideLevelLockedText(3));
        }

    }

    public void LoadStageFour()
    {
		if (!levels[3].locked)
		{
			StartCoroutine(LoadScene("Level4"));
		}
        else if (levelLockedText != null)
        {
            levelLockedText.text = "Stage 4 is locked!";
            levelLockedText.enabled = true;
            StartCoroutine(HideLevelLockedText(4));
        }

    }

    public void LoadStageFive()
    {

		if (!levels[4].locked)
		{
			StartCoroutine(LoadScene("Level5"));
		}
        else if (levelLockedText != null)
        {
            levelLockedText.text = "Stage 5 is locked!";
            levelLockedText.enabled = true;
            StartCoroutine(HideLevelLockedText(5));
        }

    }

    IEnumerator HideLevelLockedText(int stage)
    {
        yield return new WaitForSeconds(2f);
        if(levelLockedText!=null)
        {
            levelLockedText.enabled = false;
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
