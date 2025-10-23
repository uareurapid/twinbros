using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class StageCompletion : MonoBehaviour
{
    public Sprite[] levelImages;
    public GameObject firstLevelImageOne; // the number image
    public GameObject firstLevelImageTwo; // the number image
    public GameObject lastLevelImageOne;

    public GameObject lastLevelImageTwo;
    public int stageNum = 0;
    public int numLevelsOnStage = GameConstants.NUM_LEVELS_PER_STAGE;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }
    
    public void setLevelCompletionImage(int level)
    {
        if(level < GameConstants.NUM_LEVELS_PER_STAGE) // less than 10
        {
            firstLevelImageOne.GetComponent<Image>().sprite = levelImages[0];
            firstLevelImageTwo.GetComponent<Image>().sprite = levelImages[level];
        }
        else
        {
            firstLevelImageOne.GetComponent<Image>().sprite = levelImages[1]; // 1
            firstLevelImageTwo.GetComponent<Image>().sprite = levelImages[0]; // 0
        }
    }
}
