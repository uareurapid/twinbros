using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GUIManager : MonoBehaviour {


	public UnityEngine.UI.Image [] movesImage;
	public UnityEngine.UI.Image [] extraMovesImage;
	public Sprite [] purchaseInfiniteRevivesImage;
	public Sprite [] watchRewardVideoImages;

	public UnityEngine.UI.Text levelText;
	public UnityEngine.UI.Text gameOverText;
	public UnityEngine.UI.Image gameOverImage;
	public UnityEngine.UI.Text restartText;

	public UnityEngine.UI.Image levelNumImage;
	public UnityEngine.UI.Image[] levelNumChildImages;

	public UnityEngine.UI.Image purchaseRevivesImage;
	public UnityEngine.UI.Image rewardVideoImage;
	public UnityEngine.UI.Image stageClearedImage;
	public UnityEngine.UI.Image levelClearedImage;
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

		if(!levelManager.isPlayerDead() && !levelManager.IsGameStarted() && blue.IsPaused() && red.IsPaused() && leftDoor.IsPaused() && rightDoor.IsPaused()) {
			CanShowPlayButton();
		}

	 }
		
	}

	public void disableMove(int num) {

		
	}

	public void PlayPressed() {

		//still counting time?
		if(continueTimer != 0 && !playPressed) {
			CancelInvoke("IncreaseTimer");
			HideContinueImageAndClearTimer();
			if(PlayerPrefs.GetInt(GameConstants.PRODUCT_INFINITE_REVIVES,0) == 1) {
				playPressed = true;
				playButton.sprite = playButtonImages[1];
				StartCoroutine(HidePlayButton());
				levelManager.RestartFromDyingLevel();
			}
			
		}
		//only if the button is opaque
		else if(!playPressed) {
			playPressed = true;
			playButton.sprite = playButtonImages[1];
			levelManager.respawnOnDyingLevel = false;
			StartCoroutine(StartGameRoutine());
		}
		
	}

	IEnumerator HidePlayButton() {
		yield return new WaitForSeconds(0.4f);
		playButton.enabled = false;
		playButton.sprite = playButtonImages[0]; //restore for later usage
	}

	IEnumerator StartGameRoutine() {

		yield return new WaitForSeconds(0.4f);
		//same as HidePlayButton
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
		levelManager.ShowLevelNum();
	}

	//call this when loading a new screen
	public void DoStageTransitionEffect() {
		StartCoroutine(HandleStageImageTransition());
	}

	public void SetLevelText(int level) {
		levelText.text = string.Format("Level: {0}", level);
	}

	public void SetMovesText(int remainining, bool hasExtraMoves) {
		if(hasExtraMoves && remainining >=10) {
			extraMovesImage[remainining-10].enabled = false;
		}
		else if(remainining >= 0) {
			movesImage[remainining].enabled = false;
		}
		
	}
	//the default ones
	public void ResetRegularMoves() {
		foreach(UnityEngine.UI.Image img in movesImage) {
			img.enabled = true;
		}
		
	}
	//and the extra ones
	public void ResetExtraMoves() {

		foreach (UnityEngine.UI.Image img in extraMovesImage)
		{
			img.color = new Color(img.color.r,img.color.b,img.color.g,1);
			img.enabled = true;
		}
	}

	public void ShowGameOver() {

		if(gameOverImage!=null) {
			gameOverImage.enabled = true;
		}

		//ONLY AFTER GAME OVER, AND IN CASE WE HAVE A VIDEO READY? (or also in app purchase???)
		continueTimer = 0;

		playPressed = false;

		if(continueImage!=null) {
			continueImage.enabled = true;
			UpdateCountdownImage();
			countdownImage.enabled = true;
			InvokeRepeating("IncreaseTimer", 0, 1.0f);
		}

		//check if purchased infite revives
		if(PlayerPrefs.GetInt(GameConstants.PRODUCT_INFINITE_REVIVES,0) == 1) {
			// show the option to continue
			StartCoroutine(ShowRestartText());
		}// preferably show ads
		else if(adsScript!=null && adsScript.IsRewardVideoReady() && PlayerPrefs.GetInt(GameConstants.PRODUCT_REMOVE_ADS,0)!=1 ) {
			ShowVideoRewardToEnableContinue();
		}//otherwise show purchase option
		else {
			//TODO when show the moves purchase or ads removal? (add on settings only)
			ShowPurchaseRevivesButton();
		}
		
	}

	public void ShowLevelNumImages(int level) {
		// pos[0] = 9, pos[9] = 0
		switch(level) {
			case 1: levelNumChildImages[0].sprite = continueTimeImages[8]; break;
			case 2: levelNumChildImages[0].sprite = continueTimeImages[7]; break;
			case 3: levelNumChildImages[0].sprite = continueTimeImages[6]; break;
			case 4: levelNumChildImages[0].sprite = continueTimeImages[5]; break;
			case 5: levelNumChildImages[0].sprite = continueTimeImages[4]; break;
			case 6: levelNumChildImages[0].sprite = continueTimeImages[3]; break;
			case 7: levelNumChildImages[0].sprite = continueTimeImages[2]; break;
			case 8: levelNumChildImages[0].sprite = continueTimeImages[1]; break;
			case 9: levelNumChildImages[0].sprite = continueTimeImages[0]; break;
			default: levelNumChildImages[0].sprite = continueTimeImages[8]; break;
		}
		if(level > 9) {
			levelNumChildImages[1].sprite = continueTimeImages[9];
		}
		//only show the 2nd image if on level 10
		levelNumChildImages[1].enabled = (level > 9);
		levelNumChildImages[0].enabled = true;
		levelNumImage.enabled = true;
		
	}

	public void HideLevelNumImages() {
		levelNumChildImages[1].enabled = false;
		levelNumChildImages[0].enabled = false;
		levelNumImage.enabled = false;
	}

	public void ShowPurchaseRevivesButton() {
		//TODO animation
		purchaseRevivesImage.enabled = true;
		
	}

	public void ShowVideoRewardToEnableContinue() {
		rewardVideoImage.enabled = true;	
	}

	public void RewardVideoPressed() {
		rewardVideoImage.sprite = watchRewardVideoImages[1];
		if (adsScript != null && adsScript.IsRewardVideoReady())
		{
			Time.timeScale = 0;
			adsScript.ShowRewardVideo(this);
		}

		StartCoroutine(HideRewardedVideoImage());
		
	}
	//called when the video was watched or closed
	public void WatchedRewardedVideo(bool watched) {

		Time.timeScale = 1.0f;

		if(watched) {
			Debug.Log("YES REWARD, Saw the video, otherwise, not");

			if(continueTimer != 0) {
			   CancelInvoke("IncreaseTimer");
			   HideContinueImageAndClearTimer();
			   levelManager.RestartFromDyingLevel();
			}
			
		}
		else {
			Debug.Log("NO REWARD FOR YOU");
		}
		rewardVideoImage.enabled = false;	
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
			HideContinueImageAndClearTimer();
			CanShowPlayButton();
			purchaseRevivesImage.enabled = false;
			rewardVideoImage.enabled = false;
		}
		
		
	}

	void HideContinueImageAndClearTimer() {
		continueImage.enabled = false;
		countdownImage.enabled = false;
		continueTimer = 0;
	}

	//only when the courtain opens and the titles hae finished
	public void CanShowPlayButton() {

		playButton.enabled = true;
		playButton.GetComponent<FadeSprite>().FadeSpriteNow(true);
	}

	IEnumerator ShowRestartText() {
		yield return new WaitForSeconds(2f);
		CanShowPlayButton();
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

	public void ShowLevelClearedImage() {
		levelClearedImage.enabled = true;
	}

	public void HideLevelClearedImage() {
		levelClearedImage.enabled = false;
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
	//show it
	public void ShowStageImage() {
		stageLevelImage.color = new Color(stageLevelImage.color.r,stageLevelImage.color.b,stageLevelImage.color.g,0);
		stageLevelImage.enabled = true;
		//fade in
		stageLevelImage.GetComponent<FadeSprite>().FadeSpriteNow(true);
	}

	// fade out
	public void HideStageImage() {
		stageLevelImage.GetComponent<FadeSprite>().FadeSpriteNow(false);
	}
	//disable it
	void DisableStageImage() {
		stageLevelImage.enabled = false;
		stageLevelImage.color = new Color(stageLevelImage.color.r,stageLevelImage.color.b,stageLevelImage.color.g,0);
	}

	public void ResetMoves(bool hasExtraMoves) {
		ResetRegularMoves();
		if(hasExtraMoves) {
			ResetExtraMoves();
		}
	}

	public void PurchaseInfiniteRevivesPressed() {

		purchaseRevivesImage.sprite = purchaseInfiniteRevivesImage[1];
		Debug.Log("PurchaseInfiniteRevivesPressed clicked");
		if(store!=null && store.IsInitialized() ) {
			Time.timeScale = 0;
			Debug.Log("TRY TO PURCHASE PurchaseInfiniteRevives ");
			store.PurchaseProduct(GameConstants.PRODUCT_INFINITE_REVIVES, this);
		}
	}

	public void PurchaseCompleted(string productID) {


		PlayerPrefs.SetInt(productID, 1);

		if(productID == GameConstants.PRODUCT_INFINITE_REVIVES) {

			Debug.Log("PURCHASE completed for ID " + productID);
			// hide the purchase button & the continue button
			Time.timeScale = 1.0f;
			StartCoroutine(HidePurchaseRevivesImage());
			if(continueTimer != 0) {
				CancelInvoke("IncreaseTimer");
				HideContinueImageAndClearTimer();
				levelManager.RestartFromDyingLevel();
			}
			
		}
		
		
	}

	public void PurchaseFailed() {
		Debug.Log("PURCHASE PurchaseFailed");
		Time.timeScale = 1.0f;
	}

	IEnumerator HideRewardedVideoImage() {
		yield return new WaitForSeconds(1.2f);
		rewardVideoImage.enabled = false;
		rewardVideoImage.sprite = watchRewardVideoImages[0];
	}

	IEnumerator HidePurchaseRevivesImage() {
		Debug.Log("HidePurchaseRevivesImage CALLED");
		yield return new WaitForSeconds(1.2f);
		purchaseRevivesImage.sprite = purchaseInfiniteRevivesImage[0];
		purchaseRevivesImage.enabled = false;
	}
}
