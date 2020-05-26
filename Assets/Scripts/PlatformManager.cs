using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformManager : MonoBehaviour
{

    public bool isGooglePlayAndroid = false;
    public bool isHuaweiAndroid = true;
    public bool isIOS = false;
    public bool isMacOS = false;

    // Start is called before the first frame update
    void Start()
    {
      
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
}
