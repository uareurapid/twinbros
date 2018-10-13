using GoogleMobileAds.Api;
using System;
using UnityEngine;

public class GoogleMobileAdsScript : MonoBehaviour {

    //README https://developers.google.com/admob/unity/rewarded-video
	private RewardBasedVideoAd rewardBasedVideo;

	private InterstitialAd interstitialAd;
	// Use this for initialization
	public void Start()
    {
        #if UNITY_ANDROID
            string appId = "ca-app-pub-3940256099942544~3347511713";
        #elif UNITY_IPHONE
            string appId = "ca-app-pub-9531252796858598~9251777791";
			//"ca-app-pub-3940256099942544~1458002511";
        #else
            string appId = "unexpected_platform";
        #endif

        // Initialize the Google Mobile Ads SDK.
        MobileAds.Initialize(appId);

		// Get singleton reward based video ad reference.
        this.rewardBasedVideo = RewardBasedVideoAd.Instance;


		// Called when an ad request has successfully loaded.
        rewardBasedVideo.OnAdLoaded += HandleRewardBasedVideoLoaded;
        // Called when an ad request failed to load.
        rewardBasedVideo.OnAdFailedToLoad += HandleRewardBasedVideoFailedToLoad;
        // Called when an ad is shown.
        rewardBasedVideo.OnAdOpening += HandleRewardBasedVideoOpened;
        // Called when the ad starts to play.
        rewardBasedVideo.OnAdStarted += HandleRewardBasedVideoStarted;
        // Called when the user should be rewarded for watching a video.
        rewardBasedVideo.OnAdRewarded += HandleRewardBasedVideoRewarded;
        // Called when the ad is closed.
        rewardBasedVideo.OnAdClosed += HandleRewardBasedVideoClosed;
        // Called when the ad click caused the user to leave the application.
        rewardBasedVideo.OnAdLeavingApplication += HandleRewardBasedVideoLeftApplication;


		this.RequestRewardBasedVideo();
    }

	public bool IsRewardVideoReady() {
		return rewardBasedVideo.IsLoaded();
	}

	public bool IsInterstitialReady() {
		return interstitialAd.IsLoaded();
	}
	
	public void ShowRewardVideo() {

		rewardBasedVideo.Show();
	}

	public void ShowInterstitialAd() {

		interstitialAd.Show();
	}

	//TODO get the ids for Android (only after trying unique release on IOS)
	private void RequestRewardBasedVideo()
    {
		#if UNITY_ANDROID
            string adUnitId = "ca-app-pub-3940256099942544/5224354917";
		#elif UNITY_IPHONE
			string adUnitId = "ca-app-pub-9531252796858598/3913311894";
		//"ca-app-pub-3940256099942544/1712485313";
        #else
            string adUnitId = "unexpected_platform";
        #endif

        // Create an empty ad request.
        AdRequest request = new AdRequest.Builder().Build();
        // Load the rewarded video ad with the request.
        this.rewardBasedVideo.LoadAd(request, adUnitId);
    }

	public void RequestInterstitialAd() {


		#if UNITY_IPHONE
			string appUnitId = "ca-app-pub-9531252796858598/4268535114";
		#else
            string adUnitId = "unexpected_platform";
        #endif
	}
	
	    public void HandleRewardBasedVideoLoaded(object sender, EventArgs args)
    {
        MonoBehaviour.print("HandleRewardBasedVideoLoaded event received");
    }

    public void HandleRewardBasedVideoFailedToLoad(object sender, AdFailedToLoadEventArgs args)
    {
        MonoBehaviour.print(
            "HandleRewardBasedVideoFailedToLoad event received with message: "
                             + args.Message);
    }

    public void HandleRewardBasedVideoOpened(object sender, EventArgs args)
    {
        MonoBehaviour.print("HandleRewardBasedVideoOpened event received");
    }

    public void HandleRewardBasedVideoStarted(object sender, EventArgs args)
    {
        MonoBehaviour.print("HandleRewardBasedVideoStarted event received");
    }

    public void HandleRewardBasedVideoClosed(object sender, EventArgs args)
    {
        MonoBehaviour.print("HandleRewardBasedVideoClosed event received");
    }

    public void HandleRewardBasedVideoRewarded(object sender, Reward args)
    {
        string type = args.Type;
        double amount = args.Amount;
        MonoBehaviour.print(
            "HandleRewardBasedVideoRewarded event received for "
                        + amount.ToString() + " " + type);
    }

    public void HandleRewardBasedVideoLeftApplication(object sender, EventArgs args)
    {
        MonoBehaviour.print("HandleRewardBasedVideoLeftApplication event received");
    }
}
