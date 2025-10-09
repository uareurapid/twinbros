using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Achievements : MonoBehaviour
{
    // Start is called before the first frame update

    public Sprite achievementDone;
    public Sprite achievementNotDone;

    public LevelStatus[] achievements;
    void Start()
    {
        // test
        // PlayerPrefs.SetInt(GameConstants.ACHIEVEMENT_STAGE_STR + "1", 1);

        foreach (LevelStatus level in achievements)
        {
            int num = PlayerPrefs.GetInt(GameConstants.ACHIEVEMENT_STAGE_STR + level.levelNumber.ToString(), 0);
            // Debug.Log("For " + GameConstants.ACHIEVEMENT_STAGE_STR + level.levelNumber.ToString() + " value: " + num);
            Sprite currentImage = num > 0 ? achievementDone : achievementNotDone;
            level.GetComponent<Image>().sprite = currentImage;
        }
    }

    // Update is called once per frame
    void Update()
    {
     
    }
}
