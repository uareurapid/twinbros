using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialController : MonoBehaviour {

	public GameObject partOne;
	public GameObject partTwo;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void showPartOne() {
		partOne.SetActive(true);
		StartCoroutine(showPartTwo());
		//TODO show text
	}
	
	IEnumerator showPartTwo() {
		yield return new WaitForSecondsRealtime(2f);
		partTwo.SetActive(true);
		yield return new WaitForSecondsRealtime(3f);
		partOne.SetActive(false);
		partTwo.SetActive(false);

	}
}
