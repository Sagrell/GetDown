using GoogleMobileAds.Api;

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdManager : MonoBehaviour {
    public static AdManager Instance;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        DontDestroyOnLoad(gameObject);
    }
    InterstitialAd view;
    RewardBasedVideoAd rewardAd;
    private void Start()
    {
        rewardAd = RewardBasedVideoAd.Instance;
        /*
        rewardAd.OnAdFailedToLoad += HandleOnAdFailedToLoad;
        
        rewardAd.OnAdStarted += HandleOnAdStarted;
        
        rewardAd.OnAdOpening += HandleOnAdOpening;
        rewardAd.OnAdLeavingApplication += HandleOnAdLeavingApplication;*/
    }
    public void ShowRewardAd(string adId, System.EventHandler<Reward> HandleOnAdRewarded, System.EventHandler<System.EventArgs> HandleOnAdClosed)
    {
        rewardAd.OnAdClosed += HandleOnAdClosed;
        rewardAd.OnAdLoaded += HandleOnRewardAdLoaded;
        rewardAd.OnAdRewarded += HandleOnAdRewarded;
        //rewardAd.LoadAd(new AdRequest.Builder().AddTestDevice(AdRequest.TestDeviceSimulator).AddTestDevice("4B5697810FA0ACD7").Build(), adId);
        rewardAd.LoadAd(new AdRequest.Builder().Build(), adId);     
    }
    public void ShowFullscreenAd(string adId)
    {
        view = new InterstitialAd(adId);
        //AdRequest req = new AdRequest.Builder().AddTestDevice(AdRequest.TestDeviceSimulator).AddTestDevice("4B5697810FA0ACD7").Build();
        AdRequest req = new AdRequest.Builder().Build();
        view.LoadAd(req);
        view.OnAdLoaded += HandleOnAdLoaded;
    }
    void HandleOnAdLoaded(object sender, System.EventArgs args)
    {
            view.Show();
    }
    void HandleOnRewardAdLoaded(object sender, System.EventArgs args)
    {
            rewardAd.Show();
    }
    
    
    void HandleOnAdFailedToLoad(object sender, AdFailedToLoadEventArgs args)
    {

    }
    void HandleOnAdOpening(object sender, System.EventArgs args)
    {

    }
    
    void HandleOnAdStarted(object sender, System.EventArgs args)
    {

    }
    void HandleOnAdLeavingApplication(object sender, System.EventArgs args)
    {

    }
}
