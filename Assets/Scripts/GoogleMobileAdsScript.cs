using GoogleMobileAds.Api;
using System;
using UnityEngine;

public class GoogleMobileAdsScript : MonoBehaviour {

    //README https://developers.google.com/admob/unity/rewarded-video
	private RewardBasedVideoAd rewardBasedVideo;

	private InterstitialAd interstitial;

	private GUIManager guiManager;

    private bool isAdsSupported = true;
	// Use this for initialization
	public void Start()
    {
        #if UNITY_ANDROID
            string appId = "ca-app-pub-3940256099942544~3347511713";
            isAdsSupported = true;
        #elif UNITY_IPHONE
            string appId = "ca-app-pub-9531252796858598~9251777791";
			//"ca-app-pub-3940256099942544~1458002511";
            isAdsSupported = true;
        #else
            string appId = "unexpected_platform";
            isAdsSupported = false;
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

		//also request interstitial
		this.RequestInterstitialAd();
    }

	public bool IsRewardVideoReady() {
		return rewardBasedVideo.IsLoaded();
	}

    public bool GetIsAdsSupportingPlatform() {
        return isAdsSupported;
    }

	public bool IsInterstitialReady() {
		Debug.Log("IsInterstitialReady????? "+ (this.interstitial == null));
		if(this.interstitial == null) {
			RequestInterstitialAd();
		}
		return this.interstitial!=null && this.interstitial.IsLoaded();
	}

	public bool DecideIfShowInterstitial() {
		int rand = UnityEngine.Random.Range(0, 10); //between 0 and 9
		Debug.Log("Interstitial RAND ?" + rand);
		return (rand == 1 || rand == 5 || rand == 9);
	}
	
	public void ShowRewardVideo(GUIManager guiManager) {

		this.guiManager = guiManager;
		this.rewardBasedVideo.Show();
	}

	public void ShowInterstitialAd(GUIManager guiManager) {

		this.guiManager = guiManager;
		this.interstitial.Show();
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

	//TODO change from test to configured
	public void RequestInterstitialAd() {
        //string testAdsUnitId = "ca-app-pub-3940256099942544/4411468910";
		Debug.Log("RequestInterstitialAd() CALLED");
		#if UNITY_IPHONE
			string appUnitId = "ca-app-pub-9531252796858598/4268535114";
			string testAdsUnitId = "ca-app-pub-3940256099942544/4411468910";
		#else
            string adUnitId = "unexpected_platform";
        #endif

		// Initialize an InterstitialAd.
    		this.interstitial = new InterstitialAd(testAdsUnitId);
			Debug.Log("RequestInterstitialAd() IS IT NULL -> " + (this.interstitial == null) + ":" + testAdsUnitId);

			SetInterstitialEventsHandler();
    		// Create an empty ad request.
    		AdRequest request = new AdRequest.Builder().Build();
    		// Load the interstitial with the request.
    		this.interstitial.LoadAd(request);
	}

	void SetInterstitialEventsHandler() {

		if(this.interstitial!=null) {

			//interstitial events
			// Called when an ad request has successfully loaded.
	    	this.interstitial.OnAdLoaded += HandleOnAdLoaded;
	    	// Called when an ad request failed to load.
	    	this.interstitial.OnAdFailedToLoad += HandleOnAdFailedToLoad;
	    	// Called when an ad is shown.
	    	this.interstitial.OnAdOpening += HandleOnAdOpened;
	    	// Called when the ad is closed.
	    	this.interstitial.OnAdClosed += HandleOnAdClosed;
	    	// Called when the ad click caused the user to leave the application.
	    	this.interstitial.OnAdLeavingApplication += HandleOnAdLeavingApplication;
		}
		
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
		if(guiManager!=null) {
			guiManager.WatchedRewardedVideo(false);
		}
    }

    public void HandleRewardBasedVideoRewarded(object sender, Reward args)
    {
        string type = args.Type;
        double amount = args.Amount;
        MonoBehaviour.print(
            "HandleRewardBasedVideoRewarded event received for "
                        + amount.ToString() + " " + type);
		if(guiManager!=null) {
			guiManager.WatchedRewardedVideo(true);
		}
    }

    public void HandleRewardBasedVideoLeftApplication(object sender, EventArgs args)
    {
        MonoBehaviour.print("HandleRewardBasedVideoLeftApplication event received");
    }

	//events for insterstitial ads
	public void HandleOnAdLoaded(object sender, EventArgs args)
    {
        MonoBehaviour.print("HandleAdLoaded event received");
    }

    public void HandleOnAdFailedToLoad(object sender, AdFailedToLoadEventArgs args)
    {
        MonoBehaviour.print("HandleFailedToReceiveAd event received with message: "
                            + args.Message);
    }

    public void HandleOnAdOpened(object sender, EventArgs args)
    {
        MonoBehaviour.print("HandleAdOpened event received");
    }

    public void HandleOnAdClosed(object sender, EventArgs args)
    {
        MonoBehaviour.print("HandleAdClosed event received");
		if(guiManager!=null) {
			guiManager.AdFinished();
			this.RequestInterstitialAd(); //request anothe rone
		}
    }

    public void HandleOnAdLeavingApplication(object sender, EventArgs args)
    {
        MonoBehaviour.print("HandleAdLeavingApplication event received");
		if(guiManager!=null) {
			guiManager.AdFinished();
		}
    }
}
