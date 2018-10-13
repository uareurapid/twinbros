using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GUIManager : MonoBehaviour {


	public UnityEngine.UI.Image [] movesImage;

	public UnityEngine.UI.Text levelText;
	public UnityEngine.UI.Text gameOverText;
	public UnityEngine.UI.Image gameOverImage;
	public UnityEngine.UI.Text restartText;
	public GameObject titleScreenRedPart;
	public GameObject titleScreenBluePart;

	public GameObject leftDoorPart;
	public GameObject rightDoorPart;

	public UnityEngine.UI.Image playButton;
	public Sprite[] playButtonImages;

	public UnityEngine.UI.Image countdownImage;
	public UnityEngine.UI.Image continueImage;
	public Sprite[] continueTimeImages;

	GoogleMobileAdsScript adsScript;

	private LevelManager levelManager;
	private MyStoreClass store;

	private int continueTimer = 0;
	// Use this for initialization
	void Start () {
		GameObject scripts = GameObject.FindGameObjectWithTag("Scripts");
		adsScript = scripts.GetComponent<GoogleMobileAdsScript>();
		store = scripts.GetComponent<MyStoreClass>();
		levelManager = scripts.GetComponent<LevelManager>();
	}
	
	// Update is called once per frame
	void Update () {

		//title screen at center
		MoveWayPoint red = titleScreenRedPart.GetComponent<MoveWayPoint>();
		MoveWayPoint blue = titleScreenRedPart.GetComponent<MoveWayPoint>();
		
		//curtain closed
		MoveWayPoint leftDoor = leftDoorPart.GetComponent<MoveWayPoint>();
		MoveWayPoint rightDoor = rightDoorPart.GetComponent<MoveWayPoint>();

		if(!levelManager.IsGameStarted() && blue.IsPaused() && red.IsPaused() && leftDoor.IsPaused() && rightDoor.IsPaused()) {
			CanShowPlayButton();
		}
	}

	public void disableMove(int num) {

		
	}

	public void PlayPressed() {

		//only if the button is opaque
		//if(playButton.GetComponent<SpriteRenderer>().color.a > 0.80f) {
			playButton.sprite = playButtonImages[1];
			StartCoroutine(StartGameRoutine());
		//}
		
	}

	IEnumerator StartGameRoutine() {

		yield return new WaitForSeconds(1.2f);
		playButton.sprite = playButtonImages[0]; //restore
		playButton.enabled = false;
		titleScreenRedPart.SetActive(false);
		titleScreenBluePart.SetActive(false); 
		levelManager.StartGame();
	}

	public void SetLevelText(int level) {
		levelText.text = string.Format("Level: {0}", level);
	}

	public void SetMovesText(int remainining) {
		if(remainining >= 0) {
			movesImage[remainining].enabled = false;
		}
		
	}

	public void ResetAllMovesText() {
		foreach(UnityEngine.UI.Image img in movesImage) {
			img.enabled = true;
		}
		
	}

	public void ShowGameOver() {

		if(gameOverImage!=null) {
			gameOverImage.enabled = true;
		}

		//ONLY AFTER GAME OVER, AND IN CASE WE HAVE A VIDEO READY? (or also in app purchase???)
		continueTimer = 0;

		if(continueImage!=null) {
			continueImage.enabled = true;
			UpdateCountdownImage();
			countdownImage.enabled = true;
			InvokeRepeating("IncreaseTimer", 0, 1.0f);
		}
		

		if(adsScript!=null && adsScript.IsRewardVideoReady()) {

			adsScript.ShowRewardVideo();
		}
		/*if(gameOverText!=null) {
			gameOverText.text = "Game Over!";
			gameOverText.GetComponent<EnableDisableMonobehaviour>().enabled = true;
			gameOverText.enabled = true;
		}*/
		
	}

	void UpdateCountdownImage() {
		if(continueTimer < continueTimeImages.Length) {
			countdownImage.sprite = continueTimeImages[continueTimer];
		}
		
	}

	

	void IncreaseTimer() {

		if(continueTimer <= 9) {
			continueTimer += 1;
			UpdateCountdownImage();
		}
		else {
			CancelInvoke("IncreaseTimer");
			continueImage.enabled = false;
			countdownImage.enabled = false;
			continueTimer = 0;
			StartCoroutine(ShowRestartText());
			
		}
		
		
	}

	//only when the courtain opens and the titles hae finished
	public void CanShowPlayButton() {

		playButton.enabled = true;
		playButton.GetComponent<FadeSprite>().FadeSpriteNow(true);
	}

	IEnumerator ShowRestartText() {
		yield return new WaitForSeconds(2f);
		//restartText.enabled = true;
		playButton.enabled = true;
	}

	public void HideGameOver() {
		if(gameOverImage!=null) {
			gameOverImage.enabled = false;
		}
		/*if(gameOverText!=null) {
			gameOverText.GetComponent<EnableDisableMonobehaviour>().enabled = false;
			gameOverText.text = "";
			gameOverText.enabled = false;
		}*/
		//restartText.enabled = false;
		playButton.GetComponent<FadeSprite>().FadeSpriteNow(true);
		playButton.enabled = false;
		
		
		
	}

	public void ResetMoves() {
		foreach(UnityEngine.UI.Image image in movesImage) {
			image.enabled = true;
		}
	}

	public void PurchaseInfiniteRevives() {

		Debug.Log("TRY TO PURCHASE PurchaseInfiniteRevives ");
		if(store!=null) {
			store.PurchaseProduct(GameConstants.PRODUCT_INFINITE_REVIVES);
		}
	}
}
