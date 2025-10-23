using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Achievements : MonoBehaviour
{
    // Start is called before the first frame update

    public Sprite achievementDone;
    public Sprite achievementNotDone;

    public StageCompletion[] achievements;

    void Start()
    {
        // test
        // PlayerPrefs.SetInt(GameConstants.ACHIEVEMENT_STAGE_STR + "1", 1);

        // CheckAchievements();
    }

    // Update is called once per frame
    void Update()
    {

    }

    void CheckAchievements()
    {
        foreach (StageCompletion stage in achievements)
        {
            int stageNum = stage.stageNum;
            int numLevels = stage.numLevelsOnStage;

            // this is more like Stage Status, TODO REFACTOR LATER
            LevelStatus levelStatus = stage.gameObject.GetComponentInChildren<LevelStatus>();

            int lastLevelUnlocked = 0;
            // if i start backwards, only need to get the first key, which is the highest level on the stage
            for (int index = numLevels; index >= 1; index--)
            {
                string achievementKey = GameConstants.ACHIEVEMENT_STAGE_LEVEL_STR.Replace("{0}", stageNum.ToString()).Replace("{1}", index.ToString());
                int levelCleared = PlayerPrefs.GetInt(achievementKey, 0);
                // if the key exists, the index is the highest level unlocked
                if (levelCleared > 0)
                {
                    lastLevelUnlocked = index;
                    break;
                }
            }

            if(lastLevelUnlocked > 0)
            {
                levelStatus.unlockLevel();
                stage.setLevelCompletionImage(lastLevelUnlocked); // first position is image 0
            } else
            {
                levelStatus.lockLevel();
                stage.setLevelCompletionImage(1); // first position is image 0
            }
        }
    }

    void Awake()
    {
        CheckAchievements();
    }

    public void CloseCurrentScene()
    {
        SoundEffectsHelper.Instance.PlayReplaySound();
        Debug.Log("Achievements CloseCurrentScene()");
        SceneSwitcher.UnLoadCurrentSceneFromTop();
    }

    public void AddAchievement(string achievementName, int value)
    {
        PlayerPrefs.SetInt(achievementName, value);
    }

    public int GetAchievement(string achievementName)
    {
        int value = PlayerPrefs.GetInt(achievementName, 0);
        return value; // 0 means no achievement
    }

    /**public static Achievements Instance
	{

		get
		{
			if (instance == null)
			{
				GameObject scripts = GameObject.FindGameObjectWithTag("Scripts");
				if (scripts != null)
				{
					instance = scripts.GetComponent<Achievements>();
				}
				if (instance == null)
				{
					instance = (Achievements)FindObjectOfType(typeof(Achievements));
					if (instance == null)
					{
						//final chance, just create a new object
						instance = new GameObject("Achievements").AddComponent<Achievements>();
					}

				}


			}
			return instance;
		}
	}*/
}
