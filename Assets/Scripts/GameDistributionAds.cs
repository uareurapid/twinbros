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
/*
using UnityEngine;
public class GameDistributionAds : GenericAdsManager
{

    // private GUIManager guiManager;

    // private bool watchedRewardVideo = false;

    // private bool _isRewardVideoReady = false;

    // private PlatformManager platformManager;

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
    // public bool isSDKReady = false;
    // public bool isAdsReady = false;
    // public bool isAdsFinished = false;
    

    void Awake()
    {
        GameObject scripts = GameObject.FindGameObjectWithTag("Scripts");
        if (scripts != null)
        {
            platformManager = scripts.GetComponent<PlatformManager>();
        }
        GameDistribution.OnResumeGame += OnResumeGame;
        GameDistribution.OnPauseGame += OnPauseGame;
        //GameDistribution.OnPreloadRewardedVideo += OnPreloadRewardedVideo;
        GameDistribution.OnRewardedVideoSuccess += OnRewardedVideoSuccess;
        GameDistribution.OnRewardedVideoFailure += OnRewardedVideoFailure;
        GameDistribution.OnRewardGame += OnRewardGame;
        GameDistribution.OnEvent += OnEvent;

        PreloadRewardedAd();
    }
    
    /**
    SDK_GAME_PAUSE
    Triggered before an ad starts. Pause your game logic and mute any audio to ensure there is no overlap of audio.

    SDK_GAME_START
    Triggered after the ad finishes. Resume gameplay and unmute audio so the player can continue.

    Best practices
    Pausing for Interstitial Ads
    When showing an interstitial ad:

    Mute game audio (sound effects, background music).

    Pause the game completely — stop timers, animations, and user input.

    Disable any buttons used to trigger the ad, so players can't click them multiple times.

    Ad timers
    The SDK automatically throttles ad requests to avoid showing too many ads too quickly.
    If your game uses its own ad timer, try to match it with the SDK’s to keep things in sync.
    */

    // public bool DecideIfShowInterstitial() {
	// 	int rand = UnityEngine.Random.Range(0, 10); //between 0 and 9
	// 	//Debug.Log("Interstitial RAND ?" + rand);
	// 	return rand == 1 || rand == 5 || rand == 9;
	// }

    // public void OnResumeGame()
    // {
    //     // RESUME MY GAME
    // }

    // public void OnPauseGame()
    // {
    //     // PAUSE MY GAME
    // }

    // public void OnRewardGame()
    // {
    //     // REWARD PLAYER HERE
    // }*/
/*
    public void OnEvent(string eventData)
    {
        Debug.Log("RECEIVED EVENT: " + eventData);
        if (eventData == EVENT_SDK_READY)
        {
            isSDKReady = true;
            isAdsFinished = isAdsReady = false;

        }
        else if (eventData == EVENT_SDK_GAME_PAUSE)
        {
            isAdsReady = true;
            isAdsFinished = false;
            Debug.Log("ADS WILL START");
        } else if(eventData == EVENT_AD_LOADED)
        {
            isAdsReady = true;
            isAdsFinished = false;
            _isRewardVideoReady = true;
        }
        else if (eventData == EVENT_SDK_GAME_START)
        {
            isAdsFinished = true;
            isAdsReady = false;
            Debug.Log("ADS FINISHED");
            _isRewardVideoReady = false;
            guiManager.InterstitialAdFinished();
        }
        else if(eventData == EVENT_SDK_REWARDED_WATCH_COMPLETE)
        {
            isAdsFinished = true;
            isAdsReady = false;
            Debug.Log("OnRewardedVideoSuccess");
            OnRewardedVideoSuccess();
        } 
    }

    // public void OnRewardedVideoSuccess()
    // {
    //     // Rewarded video succeeded/completed.;
    //     this.watchedRewardVideo = true;
    //     MonoBehaviour.print("HandleRewardBasedVideoClosed event received " + this.watchedRewardVideo);
	// 	if(guiManager!=null) {
	// 		guiManager.WatchedRewardedVideo(true);
    //         //reset this for the next one
    //         this.watchedRewardVideo = false;
	// 	}
    // }

    // public void OnRewardedVideoFailure()
    // {
    //     // Rewarded video failed.;
    //     this.watchedRewardVideo = false;
    //     MonoBehaviour.print("HandleRewardBasedVideoClosed event received " + this.watchedRewardVideo);
    //     if(guiManager!=null) {
	// 		guiManager.WatchedRewardedVideo(false);
	// 	}
    // }

    /*public void OnPreloadRewardedVideo(int loaded)
    {
        // Feedback about preloading ad after called GameDistribution.Instance.PreloadRewardedAd
        // 0: SDK couldn't preload ad
        // 1: SDK preloaded ad
        MonoBehaviour.print("OnPreloadRewardedVideo event received " + loaded);
        if(loaded < 1)
        {
            this._isRewardVideoReady = false;
        } else
        {
            this._isRewardVideoReady = true;
        }
    }*/
/*
    public override void ShowAd()
    {
        GameDistribution.Instance.ShowAd();
    }

    public override void ShowRewardedAd()
    {
        if(this._isRewardVideoReady)
        {
            GameDistribution.Instance.ShowRewardedAd();
        }
        
    }

    public void PreloadRewardedAd()
    {
        GameDistribution.Instance.PreloadRewardedAd();
    }

    public override bool IsInterstitialReady()
    {
        return isAdsReady || isSDKReady; //throw new NotImplementedException();
    }

 

    public override bool IsRewardVideoReady()
    {
        return this._isRewardVideoReady || this.isSDKReady; //throw new NotImplementedException();
    }

    public override bool IsSDKReady()
    {
        return this.isSDKReady;
    }

    // internal void ShowRewardVideo(GUIManager gUIManager)
    // {
    //     this.guiManager = gUIManager;
    //     this.ShowRewardedAd(); //throw new NotImplementedException();
    // }
}
*/