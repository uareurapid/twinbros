using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialController : MonoBehaviour {

	//public GameObject partOne;
	//public GameObject partTwo;
    public GameObject[] parts;
    public float intervalBetweenParts = 2f; //keep it simple stupid
    private int currentPart = 0;

    private bool ended = false;

    private bool started = false;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void ShowTutorial() {
        started = true;
        ended = false;
        currentPart = 0;
        float increaseFactor = 1f;
        foreach(GameObject part in parts) {
            currentPart++;
            if(currentPart <= 2) {

                StartCoroutine(ShowPartial(part, currentPart-1, intervalBetweenParts, intervalBetweenParts)); 

            }
            else {
                //last 3 stay for longer
                StartCoroutine(ShowPartial(part, currentPart-1, intervalBetweenParts * currentPart, intervalBetweenParts * 2.6f)); 
            }

        }
 
		//TODO show text
	}

    IEnumerator ShowPartial(GameObject nextPart, int index, float interval, float intervalBetween)
    {
        yield return new WaitForSecondsRealtime(interval);
        nextPart.SetActive(true);
        yield return new WaitForSecondsRealtime(intervalBetween);
        nextPart.SetActive(false);
        if (index == parts.Length - 1) {
            ended = true;
        }

	}

    public bool IsTutorialEnded() {
        return ended && started ;
    }

}
