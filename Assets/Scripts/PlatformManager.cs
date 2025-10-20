using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformManager : MonoBehaviour
{

       //for Apple arcade or desktop (no in-apps or ads)
    public bool isArcadeOrSubscriptionMode = false;
    public bool isGooglePlayAndroid = false;
    public bool isHuaweiAndroid = false;
    public bool isIOS = false;
    public bool isMacOS = false;

    // Start is called before the first frame update
    void Start()
    {

    }
    
    void Awake() {
        if (
			Application.platform == RuntimePlatform.LinuxPlayer ||
			Application.platform == RuntimePlatform.WindowsPlayer ||
			Application.platform == RuntimePlatform.OSXPlayer
		)
		{
			isArcadeOrSubscriptionMode = true;
		}    
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public bool IsAdsSupportingPlatform()
    {
        return !isMacOS && !isHuaweiAndroid;
    }

    public bool IsLeaderboardsSupportingPlatform()
    {
        return !isHuaweiAndroid;
    }

    public bool IsPurchasesSupportingPlatform()
    {
        return !isMacOS && !isHuaweiAndroid;
    }

    //does not support any of these extra things
    public bool IsBasicSupportedPlatform()
    {
        return !IsAdsSupportingPlatform() && !IsLeaderboardsSupportingPlatform() && !IsPurchasesSupportingPlatform();
    }

    public bool hasInAppPurchasesSupport()
    {
        return IsMobilePlatform();
    }

    public bool IsMobilePlatform()
    {

        return Application.platform == RuntimePlatform.IPhonePlayer || Application.platform == RuntimePlatform.Android;
    }

    public bool isWebVersion()
    {
        return Application.platform == RuntimePlatform.WebGLPlayer;
    }

    public bool isEditorVersion()
    {
        return Application.platform == RuntimePlatform.LinuxEditor ||
                Application.platform == RuntimePlatform.OSXEditor || 
                Application.platform == RuntimePlatform.WindowsEditor;
    }
}
