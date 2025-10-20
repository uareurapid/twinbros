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
using UnityEngine;
public class GameDistributionAds : MonoBehaviour
{

    private GUIManager guiManager;

    private bool watchedRewardVideo = false;

    private bool _isRewardVideoReady = false;

    void Awake()
    {
        GameDistribution.OnResumeGame += OnResumeGame;
        GameDistribution.OnPauseGame += OnPauseGame;
        GameDistribution.OnPreloadRewardedVideo += OnPreloadRewardedVideo;
        GameDistribution.OnRewardedVideoSuccess += OnRewardedVideoSuccess;
        GameDistribution.OnRewardedVideoFailure += OnRewardedVideoFailure;
        GameDistribution.OnRewardGame += OnRewardGame;

        this.PreloadRewardedAd();
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

    public void OnPreloadRewardedVideo(int loaded)
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
    }

    public void ShowAd()
    {
        GameDistribution.Instance.ShowAd();
    }

    public void ShowRewardedAd()
    {
        GameDistribution.Instance.ShowRewardedAd();
    }

    public void PreloadRewardedAd()
    {
        GameDistribution.Instance.PreloadRewardedAd();
    }

    public bool IsInterstitialReady()
    {
        return true; //throw new NotImplementedException();
    }

    public bool GetIsAdsSupportingPlatform()
    {
        return true; //throw new NotImplementedException();
    }

    public void ShowInterstitialAd(GUIManager gUIManager)
    {
        this.guiManager = gUIManager; //throw new NotImplementedException();
        this.ShowAd();
    }

    public bool IsRewardVideoReady()
    {
        return this._isRewardVideoReady || true; //throw new NotImplementedException();
    }

    internal void ShowRewardVideo(GUIManager gUIManager)
    {
        this.guiManager = gUIManager;
        this.ShowRewardedAd(); //throw new NotImplementedException();
    }
}