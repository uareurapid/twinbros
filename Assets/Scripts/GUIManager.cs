using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class GUIManager : MonoBehaviour {

	public UnityEngine.UI.Image spinningWheelImage;

	public UnityEngine.UI.Image[] bonusImages;
    public UnityEngine.UI.Image backPanelImage;
    //orange box
    public UnityEngine.UI.Image backPanelLevelCompletion;
    public UnityEngine.UI.Image backPanelGameOver;

    public Sprite [] musicSettingsImages;
	public UnityEngine.UI.Image musicSettingsButton;
    public UnityEngine.UI.Image arcadePanelMusicSettingsButton;

	public UnityEngine.UI.Image [] movesImage;
	public UnityEngine.UI.Image [] extraMovesImage;
	public Sprite [] purchaseInfiniteRevivesSprites;
	public Sprite [] purchaseExtraMovesSprites;
	public Sprite [] purchaseRemoveAdsSprites;
	public Sprite [] achievementsSprites;
	public Sprite [] watchRewardVideoImages;
	public Sprite[] restorePurchasesSprites;

	public UnityEngine.UI.Image achievementsButtonImage;
	public UnityEngine.UI.Image leaderboardButtonImage;
	public Sprite [] leaderboardsImages;

	public GameObject settingsPanel;
    public GameObject arcadeModeSettingsPanel;//no purchases btns
    //stage clear stars
    public GameObject[] stageClearStars;

    //holofotes stage 1
    public GameObject[] holofotes;

    //purchase revive (show instead of play button when player dies) where we show the rewards buttons
	public UnityEngine.UI.Image revivesImage;
	public Sprite[] reviveImagesSprites;

	public UnityEngine.UI.Text purchaseRevivesText;
	public UnityEngine.UI.Text purchaseMovesText;
	public UnityEngine.UI.Text purchaseRemoveAdsText;

	public UnityEngine.UI.Text purchaseRevivesPriceText;
	public UnityEngine.UI.Text purchaseMovesPriceText;
	public UnityEngine.UI.Text purchaseRemoveAdsPriceText;

	public UnityEngine.UI.Text currentScoreText;
	public UnityEngine.UI.Text highScoreText;
	public UnityEngine.UI.Text levelText;
	public UnityEngine.UI.Text gameOverText;
	public UnityEngine.UI.Image gameOverImage;
	public UnityEngine.UI.Text restartText;

	public UnityEngine.UI.Image pauseButton;
	public UnityEngine.UI.Image unpauseButton;

	public UnityEngine.UI.Image levelNumImage;
	public UnityEngine.UI.Image[] levelNumChildImages;

	public UnityEngine.UI.Image purchaseRevivesImage;
	public UnityEngine.UI.Image purchaseExtraMovesImage;
	public UnityEngine.UI.Image purchaseRemoveAdsImage;

	//restore in-app
	public UnityEngine.UI.Image restorePurchasesButton;

	//goto to select level
	public UnityEngine.UI.Image gameSettingsButton;
    public Sprite[] gameSettingsImages;

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

	private bool shouldShowInterstitial = false;
	private bool showedInterstitial = false;

	private bool stopTimer = false;

	private TextLocalizationManager translationManager;
    //for Apple arcade or desktop (no in-apps or ads)
    public bool isArcadeOrSubscriptionMode = false;

	private bool isShowingTutorial = false;

    private GameManagerScript gameManager;
	// Use this for initialization
	void Start () {
		
		playPressed = false;
		GameObject scripts = GameObject.FindGameObjectWithTag("Scripts");
		adsScript = scripts.GetComponent<GoogleMobileAdsScript>();
		translationManager = TextLocalizationManager.Instance;
		translationManager.LoadSystemLanguage(Application.systemLanguage);
		LoadAllGUITranslations();
		store = scripts.GetComponent<MyStoreClass>();
		levelManager = scripts.GetComponent<LevelManager>();
        gameManager = scripts.GetComponent<GameManagerScript>();

		currentScoreText.text = "SC: " + levelManager.currentScore.ToString("000000");
		highScoreText.text = "HI: " + levelManager.highScore.ToString("000000");

        //TODO remove me for PROD
        if(levelManager.isTestMode) {
            PlayerPrefs.DeleteAll();
        }

		if(levelManager.stage > 1) {
			Invoke("DoStageTransitionEffect", 2f);
		}

        if(store!=null && !store.IsInitialized() )
        {
			store.InitStore();
        }
	}

    private void Awake()
    {
        if(Application.platform == RuntimePlatform.OSXEditor ||
            Application.platform == RuntimePlatform.IPhonePlayer ||
            Application.platform == RuntimePlatform.Android)
        {
			isArcadeOrSubscriptionMode = false;
        }

	}

    void LoadAllGUITranslations() {
		purchaseMovesText.text = translationManager.GetText(GameConstants.TXT_EXTRA_MOVES_KEY);
		purchaseRevivesText.text = translationManager.GetText(GameConstants.TXT_INFINITE_REVIVES_KEY);
		purchaseRemoveAdsText.text = translationManager.GetText(GameConstants.TXT_REMOVE_ADS_KEY);
		//public const string TXT_LEVEL_KEY = "level";
	}
	// Update is called once per frame
	void Update () {

	 if(levelManager.stage == 1) {

		//title screen at center
		/*MoveWayPoint red = titleScreenRedPart.GetComponent<MoveWayPoint>();
		MoveWayPoint blue = titleScreenRedPart.GetComponent<MoveWayPoint>();
		
		//curtain closed
		MoveWayPoint leftDoor = leftDoorPart.GetComponent<MoveWayPoint>();
		MoveWayPoint rightDoor = rightDoorPart.GetComponent<MoveWayPoint>();*/


        if(HasFinishedIntro() ) {

           holofotes[0].SetActive(false);
           holofotes[1].SetActive(false);

           if (!HasShownTutorial()) {

               StartTutorial();  
		   }
           // TODO check this, i need to know if it started too
           else if(!isShowingTutorial) {

				//Tutorial has been shown already
				if(!gameManager.IsTutorialStarted() || (gameManager.IsTutorialStarted() && gameManager.IsTutorialEnded() )  ) {
					CanShowPlayButton();
				} 
                
           }//else is currently showing
		   else if( (gameManager.IsTutorialStarted() && gameManager.IsTutorialEnded() ) || HasShownTutorial() ) {
			   isShowingTutorial = false;
			   CanShowPlayButton();
		   } 	   	
            
        }
        else if(CanShowHolofotes()) {

		   //Debug.Log("########### SHOW HOLOFOTES");

           holofotes[0].SetActive(true);
           holofotes[1].SetActive(true);
        } 

	 }

	 if (shouldShowInterstitial && !showedInterstitial && !isArcadeOrSubscriptionMode) {
			if (adsScript.IsInterstitialReady())
			{
				stopTimer = true;
				showedInterstitial = true;
				shouldShowInterstitial = false;
				adsScript.ShowInterstitialAd(this);
			}

	 }
		
	}

    public void UpdatePriceForProduct(string productID, string localizedPriceString)
    {
		//Debug.Log("UPDATE PRICE FOR " + productID + " iS " + localizedPriceString);
		if (productID.Equals(GameConstants.PRODUCT_EXTRA_MOVES))
		{
			purchaseMovesPriceText.text = localizedPriceString;
		} else if(productID.Equals(GameConstants.PRODUCT_INFINITE_REVIVES))
        {
			purchaseRevivesPriceText.text = localizedPriceString;
		}
        else
        {
			purchaseRemoveAdsPriceText.text = localizedPriceString;
		}
		//else GameConstants.PRODUCT_REMOVE_ADS);

	}

    public void RestoreBonusImagesOpacity() {
        foreach(UnityEngine.UI.Image image in bonusImages) {
        
            Color color = image.color;
            color.a = 0.2745f;
            image.color = color;
        }
    }
    //0.2745 ->70 out of 255 opacity

	public void disableMove(int num) {

		
	}

    private bool HasFinishedIntro() {
        if (levelManager.stage == 1 && !levelManager.IsPlayerDead() && !levelManager.IsGameStarted())
        {

            //title screen at center
            MoveWayPoint red = titleScreenRedPart.GetComponent<MoveWayPoint>();
            MoveWayPoint blue = titleScreenRedPart.GetComponent<MoveWayPoint>();

            //curtain closed
            MoveWayPoint leftDoor = leftDoorPart.GetComponent<MoveWayPoint>();
            MoveWayPoint rightDoor = rightDoorPart.GetComponent<MoveWayPoint>();

            return blue.IsPaused() && red.IsPaused() && leftDoor.IsPaused() && rightDoor.IsPaused();
        }

        return false;
    }

    private bool CanShowHolofotes()
    {
        if (levelManager.stage == 1 && !levelManager.IsPlayerDead() && !levelManager.IsGameStarted())
        {

            //curtain closed
            MoveWayPoint leftDoor = leftDoorPart.GetComponent<MoveWayPoint>();
            MoveWayPoint rightDoor = rightDoorPart.GetComponent<MoveWayPoint>();



            return leftDoor.IsPaused() && rightDoor.IsPaused() && !holofotes[0].activeSelf && !holofotes[1].activeSelf ;
        }

        return false;
    }

    //the settings definitionsbuttons
    public void ShowMainGameOptions() {
    
		SoundEffectsHelper.Instance.PlayReplaySound();
        gameSettingsButton.sprite = gameSettingsImages[1];
        
        StartCoroutine(RestoreGameSettingsImage());
        
		SceneLoader loader = gameSettingsButton.GetComponent<SceneLoader>();
		if(loader!=null) {
			loader.enabled = true;
			loader.LoadNextSceneNoLevelManager();
		}
	}

	//show the game settings (load any level) if the user has made any purchase
	private bool HasDoneAnyPurchase() {

		return (PlayerPrefs.GetInt(GameConstants.PRODUCT_REMOVE_ADS,0) == 1) ||
		(PlayerPrefs.GetInt(GameConstants.PRODUCT_EXTRA_MOVES,0) == 1) ||
		(PlayerPrefs.GetInt(GameConstants.PRODUCT_INFINITE_REVIVES,0) == 1);
	}

	public void PausePressed() {

        SoundEffectsHelper.Instance.PlayReplaySound();
		pauseButton.enabled = false;
		unpauseButton.enabled = true;
        //if showing the revive button, hide it now
		revivesImage.enabled = false;

		gameSettingsButton.enabled = (this.HasDoneAnyPurchase() || levelManager.isTestMode);
        //do not show settings panel on arcade mode
        if(!isArcadeOrSubscriptionMode) {
            ShowSettingsPanel(); 
        } else {
            ShowArcadeSettingsPanel();
        }
		
		Time.timeScale = 0;

        playButton.enabled = false;
	}
    
    public bool IsGamePaused() {
        return Time.timeScale < 1f && unpauseButton.enabled;
    }

	public void UnPausePressed() {
        
		pauseButton.enabled = true;
		unpauseButton.enabled = false;

        //only if not game already happening
        if(!levelManager.IsGameStarted()) {
            playButton.enabled = true;
            //************************
        }
        
        
		gameSettingsButton.enabled = false;

        if (!isArcadeOrSubscriptionMode) {
            HideSettingsPanel();
        } else {
            HideArcadeSettingsPanel();
        }

		if(spinningWheelImage != null)

		{
			spinningWheelImage.gameObject.SetActive(false);
		}

		Time.timeScale = 1f;
        
        if(!levelManager.IsGameStarted() && !levelManager.IsPlayerDead()) {
            ShowPlayButton(0f);
        }
	}

	
	private void ChangeUITextColor2Green(UnityEngine.UI.Text text)
    {
		Color theColor = new Color(99f, 198f, 77f);
		text.color = theColor;
	}

	public void ShowSettingsPanel() {
		//set the music button On/Off
		int musicOff = PlayerPrefs.GetInt("MUSIC_OFF", 0);
		musicSettingsButton.sprite = (musicOff == 1) ? musicSettingsImages[1] : musicSettingsImages[0];

        //hide the buy buttons if i already have that product
        if(gameManager.HasPurchasedExtraMoves())
        {
			purchaseExtraMovesImage.enabled = false;
			//show in green color
			ChangeUITextColor2Green(purchaseMovesText);
			
		}
		if (gameManager.HasPurchasedInfiniteRevives())
		{
			purchaseRevivesImage.enabled = false;
			ChangeUITextColor2Green(purchaseRevivesText);
		}
		if (gameManager.HasPurchasedRemoveAds())
		{
			purchaseRemoveAdsImage.enabled = false;
			ChangeUITextColor2Green(purchaseRemoveAdsText);
		}

        if(Application.platform == RuntimePlatform.IPhonePlayer)
        {
			restorePurchasesButton.enabled = true;
        } else
        {
			restorePurchasesButton.enabled = false;
		}
		backPanelImage.enabled = true;
		settingsPanel.SetActive(true);
	}

    public void ShowArcadeSettingsPanel()
    {
        //set the music button On/Off
        int musicOff = PlayerPrefs.GetInt("MUSIC_OFF", 0);
        arcadePanelMusicSettingsButton.sprite = (musicOff == 1) ? musicSettingsImages[1] : musicSettingsImages[0];
        
        backPanelImage.enabled = true;
        arcadeModeSettingsPanel.SetActive(true);
    }

	public void HideSettingsPanel() {
        backPanelImage.enabled = false;

		restorePurchasesButton.enabled = false;
		
		settingsPanel.SetActive(false);
	}

    public void HideArcadeSettingsPanel()
    {
        backPanelImage.enabled = false;
        arcadeModeSettingsPanel.SetActive(false);
    }

	public void PlayPressed() {
        //TODO if show stage do not show level before stage image
        SoundEffectsHelper.Instance.PlayReplaySound();


		GameObject scripts = GameObject.FindGameObjectWithTag("Scripts");
		levelManager = scripts.GetComponent<LevelManager>();


		//still counting time?
		if (IsShowingCountDown() && !playPressed) {

			//Debug.Log("############## DEBUG STILL COUNTIN OK RESPANW ON DYING? " + levelManager.ShouldRespawnOnDyingLevel());

			playPressed = true;

			CancelInvoke("IncreaseTimer");
			HideContinueImageAndClearTimer();
			if( gameManager.HasPurchasedInfiniteRevives() || levelManager.ShouldRespawnOnDyingLevel() || levelManager.GetIsTestMode() ) {

				//Debug.Log("########## DEBUG SHOULD BE RESTART OK HERE  ##############");

				playButton.sprite = playButtonImages[1];
				StartCoroutine(HidePlayButton());
				levelManager.RestartFromDyingLevel();
            } else {
				//normal restart
                //Debug.Log("########## DEBUG NORMAL RESTART ##############");
				levelManager.StartGame();
            }
			
		}
		//only if the button is opaque
		else if(!playPressed) {

			//Debug.Log("########## DEBUG NOT GOOD TIMER IS ");

			playPressed = true;
			currentScoreText.text = "SC: " + levelManager.currentScore.ToString("000000");
			highScoreText.text = "HI: " + levelManager.highScore.ToString("000000");

			playButton.sprite = playButtonImages[1];
            //TODO check removed this one
			//levelManager.respawnOnDyingLevel = false;
			StartCoroutine(StartGameRoutine());
		} else if(!levelManager.IsGameStarted()) {

			playPressed = true;
			StartCoroutine(StartGameRoutine());
		}	

	}

	public void UpdateCurrentScore(int pts) {

		currentScoreText.text = "SC: " + pts.ToString("000000");
	}

	public void UpdateCurrentHighScore(int pts) {

		highScoreText.text = "HI: " + pts.ToString("000000");
	}

    //only when the courtain opens and the titles hae finished
    public void CanShowPlayButton()
    {

        if ( (!playButton.enabled && !IsGamePaused() ) || 
			(!levelManager.IsGameStarted() && !IsGamePaused() && !levelManager.IsPlayerDead()) ){

            StartCoroutine(ShowPlayButton(0f));
        } 

    }

    private void StartTutorial() {

        //Debug.Log("################# START TUTORIAL ############ ");
		isShowingTutorial = true;
        PlayerPrefs.SetInt(GameConstants.HAS_SHOWN_TUTORIAL, 1);
        gameManager.ShowTutorial();
        //StartCoroutine(ShowPlayButton(6f));
    }

    private bool HasShownTutorial() {
        return PlayerPrefs.GetInt(GameConstants.HAS_SHOWN_TUTORIAL, 0) == 1;
    }

    IEnumerator ShowPlayButton(float delay)
    {
        playButton.enabled = true;
		yield return new WaitForSecondsRealtime(delay);
		playButton.GetComponent<MoveWayPoint>().enabled = true;
		playButton.GetComponent<FadeSprite>().FadeSpriteNow(true);
		levelManager.StartButtonVisible(true);
    }

	IEnumerator HidePlayButton() {
		yield return new WaitForSeconds(0.4f);
		playButton.enabled = false;
		playButton.sprite = playButtonImages[0]; //restore for later usage
        levelManager.StartButtonVisible(false);
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
        yield return new WaitForSeconds(1.0f);
        levelManager.ShowLevelNum();
	}

	//call this when loading a new screen
	public void DoStageTransitionEffect() {
		StartCoroutine(HandleStageImageTransition());
		levelManager.StartGame();
	}

	public void SetLevelText(int level) {
        int stageLocal = levelManager.stage;

        if(level == 10)
        {
            level = 0;

        }
        else if(level < 10)
        {
            stageLocal = levelManager.stage - 1;
        } else
        {
            stageLocal = levelManager.stage;
        }

        levelText.text = string.Format("Level: {0}{1}", stageLocal, level);//also show stage num
	}

	public void ResetScore() {
		currentScoreText.text = "SC: 000000";
	}

	/*
	public void IncreaseHighScore(int points, int currentHighScore) {

		int total = currentHighScore + points;
		highScoreText.text = total.ToString("000000");
		
	}*/

	public void SetMovesText(int remainining, bool hasExtraMoves, bool hasBonusMove) {
		if( (hasExtraMoves || hasBonusMove) && remainining >=10) {
        
            UnityEngine.UI.Image img = extraMovesImage[remainining - 10];
            img.color = new Color(img.color.r,img.color.b,img.color.g,0.3f);
			img.enabled = true; //was false
		}
		else if(remainining >= 0) {
            UnityEngine.UI.Image img = movesImage[remainining];
            img.color = new Color(img.color.r,img.color.b,img.color.g,0.3f);
			img.enabled = true; //was false
		}
		
	}

	public void UpdateMovesText(int remainining, bool hasExtraMoves, bool hasBonusMove) {
		if(remainining >= 0 && remainining < 10 + 2) {
            UnityEngine.UI.Image img = movesImage[remainining];
            img.color = new Color(img.color.r,img.color.b,img.color.g,1);
			movesImage[remainining].enabled = true;
		}
		
	}
	//the default ones
	public void ResetRegularMoves() {
		foreach(UnityEngine.UI.Image img in movesImage) {
			img.enabled = true;
            img.color = new Color(img.color.r,img.color.b,img.color.g,1);
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
    
    public void DisableExtraMoves() {
        foreach (UnityEngine.UI.Image img in extraMovesImage)
        {
            img.color = new Color(img.color.r,img.color.b,img.color.g,0.3f);
            img.enabled = true;
        }
    }
    //just one
    public void ResetBonusMoves() {
    
        Color c = extraMovesImage[0].color;
        extraMovesImage[0].color = new Color(c.r,c.b,c.g,1);
        extraMovesImage[0].enabled = true;

    }

	public void ShowGameOver() {

		if(gameOverImage!=null) {
            ShowBackPanelGameOver();
            gameOverImage.enabled = true;
		}

        //by default say NO
		levelManager.respawnOnDyingLevel = false;

		//ONLY AFTER GAME OVER, AND IN CASE WE HAVE A VIDEO READY? (or also in app purchase???)
		continueTimer = 0;

		playPressed = false;

		if(continueImage!=null) {
			continueImage.enabled = true;
			UpdateCountdownImage();
			countdownImage.enabled = true;

            //start countdown timer
			InvokeRepeating("IncreaseTimer", 1.0f, 1.0f);
		}

		if(levelManager.isTestMode || levelManager.isDebugMode || isArcadeOrSubscriptionMode) {

            //show play button even while counting down
			StartCoroutine(ShowRestartText(1.0f));
		}
		//check if purchased infite revives
		else if (gameManager.HasPurchasedInfiniteRevives())
		{

			// show the option to continue right away (the play button)
			levelManager.respawnOnDyingLevel = true;
			StartCoroutine(ShowRestartText(1.0f));
		}
		//if not purchased product and is time for ads
		else if( !gameManager.HasPurchasedRemoveAds() && adsScript.IsInterstitialReady() && 
                adsScript.DecideIfShowInterstitial() && adsScript.GetIsAdsSupportingPlatform() && !isArcadeOrSubscriptionMode )  {

			shouldShowInterstitial = true;

            //stop timer while watching AD, if watched then i can continue
			//TODO CHECK if i stop the timer here counter is still at 0 probably
			stopTimer = true;
			
			showedInterstitial = true;

			// avoid show it again
			shouldShowInterstitial = false;
			adsScript.ShowInterstitialAd(this);
			gameManager.StopMusic();
			
		}
        // preferably show ads
        else if(adsScript.IsRewardVideoReady() && adsScript.GetIsAdsSupportingPlatform() && !gameManager.HasPurchasedRemoveAds() && !isArcadeOrSubscriptionMode ) {

			ShowVideoRewardToEnableContinue();

		} else if(!gameManager.HasPurchasedInfiniteRevives() && !IsGamePaused() )
        {
			//will not pause the game for this
			//will also stop the countdown when i press the button
			ShowReviveImage();
        }
		
	}

	public void ShowLevelNumImages(int stage, int level) {
        // pos[0] = 9, pos[9] = 0
        //
        //continueTimeImages [0]9 [1]8 [2]7 [3]6 [4]5 [5]4 [6]3 [7]2 [8]1 [9]0

        if(stage == 1)
        {
            levelNumChildImages[0].sprite = continueTimeImages[9]; //show 0

            switch (level)
            {
                case 1: levelNumChildImages[1].sprite = continueTimeImages[8]; break; //show 1
                case 2: levelNumChildImages[1].sprite = continueTimeImages[7]; break; //show 2
                case 3: levelNumChildImages[1].sprite = continueTimeImages[6]; break; //show 3
                case 4: levelNumChildImages[1].sprite = continueTimeImages[5]; break; //
                case 5: levelNumChildImages[1].sprite = continueTimeImages[4]; break;
                case 6: levelNumChildImages[1].sprite = continueTimeImages[3]; break;
                case 7: levelNumChildImages[1].sprite = continueTimeImages[2]; break;
                case 8: levelNumChildImages[1].sprite = continueTimeImages[1]; break;
                case 9: levelNumChildImages[1].sprite = continueTimeImages[0]; break;
                case 10: levelNumChildImages[1].sprite = continueTimeImages[9]; //show 0

                    levelNumChildImages[0].sprite = continueTimeImages[8]; //show 1 -> 10
                    break;
                default: levelNumChildImages[1].sprite = continueTimeImages[8]; break;
            }
        } else
        {

            switch(stage)
            {
                case 2: levelNumChildImages[0].sprite = (level < 10 ? continueTimeImages[8] : continueTimeImages[7]); break; //show 1 / 2
                case 3: levelNumChildImages[0].sprite = (level < 10 ? continueTimeImages[7] : continueTimeImages[6]); break; // 2 / 3
                case 4: levelNumChildImages[0].sprite = (level < 10 ? continueTimeImages[6] : continueTimeImages[5]); break; // 3 / 4
                    //TODO next stage 5
                default: levelNumChildImages[0].sprite = continueTimeImages[4]; break;
            }
            


            switch (level)
            {
                case 1: levelNumChildImages[1].sprite = continueTimeImages[8]; break; //show 1
                case 2: levelNumChildImages[1].sprite = continueTimeImages[7]; break; //show 2
                case 3: levelNumChildImages[1].sprite = continueTimeImages[6]; break; //show 3
                case 4: levelNumChildImages[1].sprite = continueTimeImages[5]; break; //
                case 5: levelNumChildImages[1].sprite = continueTimeImages[4]; break;
                case 6: levelNumChildImages[1].sprite = continueTimeImages[3]; break;
                case 7: levelNumChildImages[1].sprite = continueTimeImages[2]; break;
                case 8: levelNumChildImages[1].sprite = continueTimeImages[1]; break; //show 8
                case 9: levelNumChildImages[1].sprite = continueTimeImages[0]; break; //show 9
                case 10: levelNumChildImages[1].sprite = continueTimeImages[9]; break;//show 0
                default: levelNumChildImages[1].sprite = continueTimeImages[9]; break;
            }
        }

        levelNumChildImages[1].enabled = true;//UPDATE 10/02/2020 show stage num too (level > 9);
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

	public void LeaderboardsPressed()
	{
		//TODO CHECK add another sound???
		SoundEffectsHelper.Instance.PlaySettingsSound();

		leaderboardButtonImage.sprite = leaderboardsImages[1];
		SocialAPI.Instance.AuthenticateAndShowLeaderboards();
		StartCoroutine(RestoreLeaderBoardsImage());
	}

	public void AchievementsPressed()
	{
		SoundEffectsHelper.Instance.PlaySettingsSound();

		achievementsButtonImage.sprite = achievementsSprites[1];
		SocialAPI.Instance.AuthenticateAndShowAchievements();
		StartCoroutine(RestoreAchievementsImage());
	}
	
	IEnumerator RestoreAchievementsImage() {
		//WaitForSecondsRealtime is not affected by timescale 0
		yield return new WaitForSecondsRealtime(1f);
		achievementsButtonImage.sprite = achievementsSprites[0];
	}

	IEnumerator RestoreLeaderBoardsImage() {
		//WaitForSecondsRealtime is not affected by timescale 0
		yield return new WaitForSecondsRealtime(1f);
		leaderboardButtonImage.sprite = leaderboardsImages[0];
	}
    
    IEnumerator RestoreGameSettingsImage() {
        //WaitForSecondsRealtime is not affected by timescale 0
        yield return new WaitForSecondsRealtime(1f);
        gameSettingsButton.sprite = gameSettingsImages[0];
    }

	//TODO on load panel set the correct image
	public void MusicSettingsPressed() {

		SoundEffectsHelper.Instance.PlaySettingsSound();

		int musicOff = PlayerPrefs.GetInt("MUSIC_OFF", 0);
		if(musicOff == 0) {
			musicOff = 1;
			levelManager.DisableMusic();
		}
		else {
			musicOff = 0;
			levelManager.EnableMusic();
			
		}
		PlayerPrefs.SetInt("MUSIC_OFF", musicOff);
        if(isArcadeOrSubscriptionMode) {
            arcadePanelMusicSettingsButton.sprite = (musicOff == 1) ? musicSettingsImages[1] : musicSettingsImages[0];
        } else {
            musicSettingsButton.sprite = (musicOff == 1) ? musicSettingsImages[1] : musicSettingsImages[0];  
        }
		
		
	}

	public void RewardVideoPressed() {

		SoundEffectsHelper.Instance.PlaySettingsSound();

		rewardVideoImage.sprite = watchRewardVideoImages[1];
		if (adsScript != null && adsScript.IsRewardVideoReady())
		{
			//stop it while watching it, but only add if i watch it
			stopTimer = true;
			adsScript.ShowRewardVideo(this);
		}

		StartCoroutine(HideRewardedVideoImageRoutine());
		
	}
	
	void UpdateCountdownImage() {
		if(continueTimer < continueTimeImages.Length) {
			countdownImage.sprite = continueTimeImages[continueTimer];
		}
		
	}

	void IncreaseTimer() {
		if(!stopTimer) {

				if(continueTimer <= 9) {
					continueTimer += 1;
					UpdateCountdownImage();
				}
				else {
					//more than 9
					shouldShowInterstitial = false;
					showedInterstitial = false;
					CancelInvoke("IncreaseTimer");
					HideContinueImageAndClearTimer();
					CanShowPlayButton();
					purchaseRevivesImage.enabled = false;
					rewardVideoImage.enabled = false;
				    HideReviveImage();
				}
		}
		
		
		
	}

	void HideContinueImageAndClearTimer() {
		continueImage.enabled = false;
		countdownImage.enabled = false;
		continueTimer = 0;
	}

    //still showing the counter to continue?
    private bool IsShowingCountDown()
    {
        return (continueTimer!=0) || (countdownImage.enabled && continueImage.enabled);
    }

	
	IEnumerator ShowRestartText(float wait) {
		yield return new WaitForSeconds(wait);
		CanShowPlayButton();
	}

	public void HideGameOver() {
		if(gameOverImage!=null) {
            
			gameOverImage.enabled = false;
            HideBackPanelGameOver();
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
        ShowBackLevelCompletion();
		levelClearedImage.enabled = true;
		UnityEngine.UI.Image[] imgs = levelClearedImage.gameObject.GetComponentsInChildren<UnityEngine.UI.Image>();
		foreach(UnityEngine.UI.Image img in imgs) {
			img.enabled = true;
		}
	}

	public void HideLevelClearedImage() {
        HideBackLevelCompletion();
        levelClearedImage.enabled = false;
		UnityEngine.UI.Image[] imgs = levelClearedImage.gameObject.GetComponentsInChildren<UnityEngine.UI.Image>();
		foreach(UnityEngine.UI.Image img in imgs) {
			img.enabled = false;
		}
	}

	public void ShowStageClearedImage() {
		stageClearedImage.color = new Color(stageClearedImage.color.r,stageClearedImage.color.b,stageClearedImage.color.g,0);
		stageClearedImage.enabled = true;
		//fade in
		stageClearedImage.GetComponent<FadeSprite>().FadeSpriteNow(true);

        foreach(GameObject stars in stageClearStars) {
            stars.SetActive(true);
        }
	}

	public void HideStageClearedImage() {
		// fade out
		stageClearedImage.GetComponent<FadeSprite>().FadeSpriteNow(false);
	}

	public void DisableStageClearedImage() {
		stageClearedImage.enabled = false;
		stageClearedImage.color = new Color(stageClearedImage.color.r,stageClearedImage.color.b,stageClearedImage.color.g,0);

        foreach (GameObject stars in stageClearStars)
        {
            stars.SetActive(false);
        }
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

	public void ResetMoves(bool hasExtraMoves, bool hasBonus) {
		ResetRegularMoves();
		if(hasExtraMoves) {
			ResetExtraMoves();
		} else if(hasBonus) {
            ResetBonusMoves();
        }
	}

	public void PurchaseInfiniteRevivesPressed() {

		if(gameManager.HasPurchasedInfiniteRevives()) {
			return;
		}
		SoundEffectsHelper.Instance.PlaySettingsSound();

		purchaseRevivesImage.sprite = purchaseInfiniteRevivesSprites[1];
		//Debug.Log("PurchaseInfiniteRevivesPressed clicked");
		StartCoroutine(PressDownPurchaseInfiniteRevives());
		if(store!=null && store.IsInitialized() ) {

            
			if (spinningWheelImage != null)
			{
				spinningWheelImage.gameObject.SetActive(true);
			}

			stopTimer = true;
			//Debug.Log("TRY TO PURCHASE PurchaseInfiniteRevives ");
			store.PurchaseProduct(GameConstants.PRODUCT_INFINITE_REVIVES, this);
		} else if(!store.IsInitialized())
        {
			store.InitStore();
        }
	}

    

	IEnumerator PressDownPurchaseInfiniteRevives() {
		yield return new WaitForSecondsRealtime(1f);
		purchaseRevivesImage.sprite = purchaseInfiniteRevivesSprites[0];
	}

    //restore sprite, revive button
	IEnumerator PressDownRevive()
	{
		yield return new WaitForSecondsRealtime(1f);
		revivesImage.sprite = reviveImagesSprites[0];
	}

	public void PurchaseRemoveAdsPressed() {

		if(gameManager.HasPurchasedRemoveAds()) {
			return;
		}

		SoundEffectsHelper.Instance.PlaySettingsSound();

		purchaseRemoveAdsImage.sprite = purchaseRemoveAdsSprites[1];
		//Debug.Log("PRODUCT_REMOVE_ADS clicked");
		StartCoroutine(PressDownPurchaseRemoveAds());
		if(store!=null && store.IsInitialized() ) {

			if (spinningWheelImage != null)
			{
				spinningWheelImage.gameObject.SetActive(true);
			}

			stopTimer = true;
			//Debug.Log("TRY TO PURCHASE PRODUCT_REMOVE_ADS ");
			store.PurchaseProduct(GameConstants.PRODUCT_REMOVE_ADS, this);
		}
		else if (!store.IsInitialized())
		{
			store.InitStore();
		}
	}

	IEnumerator PressDownPurchaseRemoveAds() {
		yield return new WaitForSecondsRealtime(1f);
		purchaseRemoveAdsImage.sprite = purchaseRemoveAdsSprites[0];
	}

    //will start the purchase flow (does not show price or anything)
    //check the logic of the reward video button pressed
    public void RevivePressed()
    {
		if (gameManager.HasPurchasedInfiniteRevives())
		{
			return;
		}
		SoundEffectsHelper.Instance.PlaySettingsSound();
        //stop the counter
		stopTimer = true;

		revivesImage.sprite = reviveImagesSprites[1];
        //change sprite, simulate press down
		StartCoroutine(PressDownRevive());
		if (store != null && store.IsInitialized())
		{


			if (spinningWheelImage != null)
			{
				spinningWheelImage.gameObject.SetActive(true);
			}

			stopTimer = true;
			store.PurchaseProduct(GameConstants.PRODUCT_INFINITE_REVIVES, this);
		}
		else if (!store.IsInitialized())
		{
			store.InitStore();
		}
	}

	public void PurchaseExtraMovesPressed() {

		if(gameManager.HasPurchasedExtraMoves()) {
			return;
		}

		SoundEffectsHelper.Instance.PlaySettingsSound();

		purchaseExtraMovesImage.sprite = purchaseExtraMovesSprites[1];
		//Debug.Log("PRODUCT_EXTRA_MOVES clicked");
		if(store!=null && store.IsInitialized() ) {

			if (spinningWheelImage != null)
			{
				spinningWheelImage.gameObject.SetActive(true);
			}
			stopTimer = true;
			//Debug.Log("TRY TO PURCHASE PRODUCT_EXTRA_MOVES ");
			store.PurchaseProduct(GameConstants.PRODUCT_EXTRA_MOVES, this);
		}
		else if (!store.IsInitialized())
		{
			store.InitStore();
		}
	}

	IEnumerator PressDownPurchaseExtraMoves() {
		yield return new WaitForSecondsRealtime(1f);
		purchaseExtraMovesImage.sprite = purchaseExtraMovesSprites[0];
	}

	//called when the video was watched or closed
	public void WatchedRewardedVideo(bool watched)
	{

		//will continue the time
		stopTimer = false;

		if (watched)
		{

			levelManager.respawnOnDyingLevel = true;

			StartCoroutine(ShowRestartText(1.0f));
		}
		else
		{
			levelManager.respawnOnDyingLevel = false;
		}
		//else no reward, timer will continue as usual
		rewardVideoImage.enabled = false;

		gameManager.StarMusic();


	}

	//after an interstitial ad
	public void AdFinished() {

        levelManager.respawnOnDyingLevel = true;

		//continue the countdown
		stopTimer = false;
		//TODO CHECK
		StartCoroutine(ShowRestartText(1.0f));
		gameManager.StarMusic();
	}

	public void PurchaseCompleted(string productID) {


		PlayerPrefs.SetInt(productID, 1);

		if (spinningWheelImage != null)
		{
			spinningWheelImage.gameObject.SetActive(false);
		}

		//continue countdown
		stopTimer = false;
        if(IsShowingCountDown())
        {
			StartCoroutine(ShowRestartText(1.0f));
        }

		if (productID == GameConstants.PRODUCT_INFINITE_REVIVES)
		{

			// hide the purchase button & the continue button
			HidePurchaseRevivesImage();
			HideReviveImage();
			ChangeUITextColor2Green(purchaseRevivesText);

		}
		else if (productID == GameConstants.PRODUCT_REMOVE_ADS)
		{
			// DO NOTHING
			HidePurchaseRemoveAdsImage();
			HideReviveImage();
			ChangeUITextColor2Green(purchaseRemoveAdsText);
		}
		else if (productID == GameConstants.PRODUCT_EXTRA_MOVES)
		{
			// TODO unlock the 2 extra moves
			HidePurchaseExtraMovesImage();
			HideReviveImage();//if this one is on we hide it
			ChangeUITextColor2Green(purchaseMovesText);
		}
		
		
		
	}

	public void PurchaseFailed() {
		//Debug.Log("PURCHASE PurchaseFailed");

		HideReviveImage();

		if (spinningWheelImage != null)
		{
			spinningWheelImage.gameObject.SetActive(false);
		}
		//continue the countdown
		stopTimer = false;
		if (IsShowingCountDown())
		{
			StartCoroutine(ShowRestartText(1.0f));
		}
	}

	IEnumerator HideRewardedVideoImageRoutine() {
		yield return new WaitForSeconds(1.2f);
		gameManager.StopMusic();
		HideRewardedVideoImage();
	}

	private void HideRewardedVideoImage() {
		rewardVideoImage.enabled = false;
		rewardVideoImage.sprite = watchRewardVideoImages[0];
	}

	IEnumerator HidePurchaseRevivesImageRoutine() {
		//Debug.Log("HidePurchaseRevivesImage CALLED");
		yield return new WaitForSeconds(1.2f);
		HidePurchaseRevivesImage();
		
	}

	private void HidePurchaseRevivesImage() {

		purchaseRevivesImage.sprite = purchaseInfiniteRevivesSprites[0];
		purchaseRevivesImage.enabled = false;
	}

    //show revive button, to allow purchase and and then hide it when the purchase either is OK or NOk
    private void ShowReviveImage()
    {
        //allow clock on this button
		revivesImage.sprite = reviveImagesSprites[0];
		revivesImage.enabled = true;
	}

    private void HideReviveImage()
    {
		revivesImage.sprite = reviveImagesSprites[0];
		revivesImage.enabled = false;

		if (stopTimer)
		{
			stopTimer = false;
		}
	}

	IEnumerator HidePurchaseExtraMovesImageRoutine() {

		//Debug.Log("HidePurchaseExtraMovesImage CALLED");
		yield return new WaitForSeconds(1.2f);
		HidePurchaseExtraMovesImage();
	}	

	private void HidePurchaseExtraMovesImage() {
		purchaseExtraMovesImage.sprite = purchaseExtraMovesSprites[0];
		purchaseExtraMovesImage.enabled = false;
	}

	IEnumerator HidePurchaseRemoveAdsImageRoutine() {
		//Debug.Log("HidePurchaseRemoveAdsImage CALLED");
		yield return new WaitForSeconds(1.2f);
		HidePurchaseRemoveAdsImage();
	}

	//needs to be outside a couroutine so i can called when timescale = 0
	private void HidePurchaseRemoveAdsImage() {
		purchaseRemoveAdsImage.sprite = purchaseRemoveAdsSprites[0];
		purchaseRemoveAdsImage.enabled = false;
	}

    public void ShowBackLevelCompletion()
    {
        backPanelLevelCompletion.enabled = true;
    }

    public void HideBackLevelCompletion()
    {
        backPanelLevelCompletion.enabled = false;
    }

    public void ShowBackPanelGameOver()
    {
        backPanelGameOver.enabled = true;
    }

    public void HideBackPanelGameOver()
    {
        backPanelGameOver.enabled = false;
    }

    public void RestorePurchasesPressed()
    {
		SoundEffectsHelper.Instance.PlaySettingsSound();
		restorePurchasesButton.sprite = restorePurchasesSprites[1];
		StartCoroutine(RestorePurchasesImage());

        if(store!=null && store.IsInitialized())
        {
			store.RestorePurchases(this);
			if (spinningWheelImage != null)
			{
				spinningWheelImage.gameObject.SetActive(true);
			}
		}

    }


	IEnumerator RestorePurchasesImage()
	{
		yield return new WaitForSeconds(1.2f);
		restorePurchasesButton.sprite = restorePurchasesSprites[0];
	}

}
