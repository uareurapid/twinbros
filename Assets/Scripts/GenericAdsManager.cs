/**
Implementation within games
Download and import the .unitypackage into your game (download here).

Drag the prefab "GameDistribution" into your scene.

Copy your GAME_KEY in your GameDistribution developer's control panel (in the 'Upload' tab), at developer.gamedistribution.com

Open the prefab and replace the GAME_KEY value with your own key.

Use GameDistribution.Instance.ShowAd() to show an advertisement.

Use GameDistribution.Instance.ShowRewardedAd() to show a rewarded advertisement.

Use GameDistribution.Instance.PreloadRewardedAd() to preload a rewarded advertisement

Make use of the events GameDistribution.OnResumeGame and GameDistribution.OnPauseGame for resuming/pausing your game in between ads.

Make use of the event GameDistribution.OnPreloadRewardedVideo for checking the availability of rewarded advertisement after called GameDistribution.Instance.PreloadRewardedAd()

Correct ad-placement is key for a higher revenue potential of your game. Before you submit, make sure that your game includes a pre-roll and mid-rolls:

Pre-roll: an ad shown before the user starts playing the game.
What you basically want to do is display the pre-roll as fast as possible to the user, so they don’t have the time to change their minds and close the game.
Best practice: placing the pre-roll on buttons in the loading/splash screen (Start, Play, Continue).

Mid-roll: an ad shown in between game sessions.
Ideally placed on all non-gameplay buttons, to spread the chance that users will see ads.
Best practice: placing the mid-rolls on each button in the Game Over/Win screen (Replay, Next, Menu).

To get the most out of your game revenue and to maintain a user friendly experience, we ask you to keep these requirements in mind when deciding on the placement of the ads:

Ads only display upon user input, e.g. when clicking a button
Ads display outside of the gameplay only, to not disrupt the game experience
Game audio is muted when the ad is displayed
The game pauses when the ad is displayed
Don’t worry about spamming users with ads by placing ad-calls on too many buttons: we regulate the ad-interval through the SDK, so users will only see an ad when the set time-frame has passed.
*/
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
public abstract class GenericAdsManager : MonoBehaviour
{

    protected GUIManager guiManager;

    protected bool watchedRewardVideo = false;

    protected bool _isRewardVideoReady = false;

    protected PlatformManager platformManager;

    // Triggered before an ad starts. Pause your game logic and mute any audio to ensure there is no overlap of audio.
    const string EVENT_SDK_GAME_PAUSE = "SDK_GAME_PAUSE";

    // Triggered after the ad finishes. Resume gameplay and unmute audio so the player can continue.
    const string EVENT_SDK_GAME_START = "SDK_GAME_START";

    const string EVENT_SDK_READY = "SDK_READY";

    // watched the reward video
    const string EVENT_SDK_REWARDED_WATCH_COMPLETE = "SDK_REWARDED_WATCH_COMPLETE";

    // Fired when content should be paused. This usually happens right before an ad is about to cover the content.
    const string EVENT_CONTENT_PAUSE_REQUESTED = "CONTENT_PAUSE_REQUESTED";

    //Fired when content should be resumed. This usually happens when an ad finishes or collapses.
    const string EVENT_CONTENT_RESUME_REQUESTED = "CONTENT_RESUME_REQUESTED";
    
    const string EVENT_AD_LOADED = "LOADED";
    public bool isSDKReady = false;
    public bool isAdsReady = false;
    public bool isAdsFinished = false;
    

    void Awake()
    {
        GameObject scripts = GameObject.FindGameObjectWithTag("Scripts");
        if (scripts != null)
        {
            platformManager = scripts.GetComponent<PlatformManager>();
        }
    }
    

    public bool DecideIfShowInterstitial() {
		int rand = UnityEngine.Random.Range(0, 10); //between 0 and 9
		//Debug.Log("Interstitial RAND ?" + rand);
		return rand == 1 || rand == 5 || rand == 9;
	}

    public void OnResumeGame()
    {
        // RESUME MY GAME
    }

    public void OnPauseGame()
    {
        // PAUSE MY GAME
    }

    public void OnRewardGame()
    {
        // REWARD PLAYER HERE
    }

    public void OnRewardedVideoSuccess()
    {
        // Rewarded video succeeded/completed.;
        this.watchedRewardVideo = true;
        MonoBehaviour.print("HandleRewardBasedVideoClosed event received " + this.watchedRewardVideo);
		if(guiManager!=null) {
			guiManager.WatchedRewardedVideo(true);
            //reset this for the next one
            this.watchedRewardVideo = false;
		}
    }

    public void OnRewardedVideoFailure()
    {
        // Rewarded video failed.;
        this.watchedRewardVideo = false;
        MonoBehaviour.print("HandleRewardBasedVideoClosed event received " + this.watchedRewardVideo);
        if(guiManager!=null) {
			guiManager.WatchedRewardedVideo(false);
		}
    }


    public abstract void ShowAd();

    public abstract void ShowRewardedAd();


    public abstract bool IsInterstitialReady();

    public bool GetIsAdsSupportingPlatform()
    {
        return platformManager.isWebVersion() || platformManager.IsMobilePlatform() || !platformManager.isArcadeOrSubscriptionMode ;
    }

    public void ShowInterstitialAd(GUIManager gUIManager)
    {
        this.guiManager = gUIManager; //throw new NotImplementedException();
        if(isAdsReady)
        {
            this.guiManager.setIsShowingAds(true);
            this.ShowAd();
        }
        
    }

    public abstract bool IsRewardVideoReady();

    public abstract bool IsSDKReady();

    public void ShowRewardVideo(GUIManager gUIManager)
    {
        this.guiManager = gUIManager;
        this.ShowRewardedAd(); //throw new NotImplementedException();
    }
}