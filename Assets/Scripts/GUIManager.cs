using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GUIManager : MonoBehaviour {


	public UnityEngine.UI.Image [] movesImage;

	public UnityEngine.UI.Text levelText;
	public UnityEngine.UI.Text gameOverText;
	public UnityEngine.UI.Image gameOverImage;
	public UnityEngine.UI.Text restartText;

	public UnityEngine.UI.Image stageClearedImage;
	public UnityEngine.UI.Image stageLevelImage;
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

	private bool playPressed = false;
	private int continueTimer = 0;
	// Use this for initialization
	void Start () {
		playPressed = false;
		GameObject scripts = GameObject.FindGameObjectWithTag("Scripts");
		adsScript = scripts.GetComponent<GoogleMobileAdsScript>();
		store = scripts.GetComponent<MyStoreClass>();
		levelManager = scripts.GetComponent<LevelManager>();

		if(levelManager.stage > 1) {
			Invoke("DoStageTransitionEffect", 2f);
		}
	}
	
	// Update is called once per frame
	void Update () {

	 if(levelManager.stage == 1) {

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
		
	}

	public void disableMove(int num) {

		
	}

	public void PlayPressed() {

		//only if the button is opaque
		if(!playPressed) {
			playPressed = true;
			playButton.sprite = playButtonImages[1];
			StartCoroutine(StartGameRoutine());
		}
		
	}

	IEnumerator StartGameRoutine() {

		yield return new WaitForSeconds(0.4f);
		playButton.enabled = false;
		playButton.sprite = playButtonImages[0]; //restore for later usage
		titleScreenRedPart.SetActive(false);
		titleScreenBluePart.SetActive(false);
		StartCoroutine(HandleStageImageTransition());
		levelManager.StartGame();
	}

	IEnumerator HandleStageImageTransition() {
		//start fade out
		ShowStageImage();
		yield return new WaitForSeconds(2.0f);
		HideStageImage();
		yield return new WaitForSeconds(2.0f);
		DisableStageImage();
	}

	//call this when loading a new screen
	public void DoStageTransitionEffect() {
		StartCoroutine(HandleStageImageTransition());
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

	public void ShowStageClearedImage() {
		stageClearedImage.color = new Color(stageClearedImage.color.r,stageClearedImage.color.b,stageClearedImage.color.g,0);
		stageClearedImage.enabled = true;
		//fade in
		stageClearedImage.GetComponent<FadeSprite>().FadeSpriteNow(true);
	}

	public void HideStageClearedImage() {
		// fade out
		stageClearedImage.GetComponent<FadeSprite>().FadeSpriteNow(false);
	}

	public void DisableStageClearedImage() {
		stageClearedImage.enabled = false;
		stageClearedImage.color = new Color(stageClearedImage.color.r,stageClearedImage.color.b,stageClearedImage.color.g,0);
	}

	public void ShowStageImage() {
		stageLevelImage.color = new Color(stageLevelImage.color.r,stageLevelImage.color.b,stageLevelImage.color.g,0);
		stageLevelImage.enabled = true;
		//fade in
		stageLevelImage.GetComponent<FadeSprite>().FadeSpriteNow(true);
	}

	public void HideStageImage() {
		// fade out
		stageLevelImage.GetComponent<FadeSprite>().FadeSpriteNow(false);
	}

	void DisableStageImage() {
		stageLevelImage.enabled = false;
		stageLevelImage.color = new Color(stageLevelImage.color.r,stageLevelImage.color.b,stageLevelImage.color.g,0);
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
