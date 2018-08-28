using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GUIManager : MonoBehaviour {


	public UnityEngine.UI.Image [] movesImage;

	public UnityEngine.UI.Text levelText;
	//public UnityEngine.UI.Text movesText;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void disableMove(int num) {

		
	}

	public void SetLevelText(int level) {
		levelText.text = string.Format("Level: {0}", level);
	}

	public void SetMovesText(int remainining) {
		if(remainining >= 0) {
			movesImage[remainining].enabled = false;
		}
		
	}
}
