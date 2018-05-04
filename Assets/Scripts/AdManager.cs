using GoogleMobileAds.Api;

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdManager : MonoBehaviour {
    public static AdManager Instance;
    public static int countRestart;
    public static int nextShowAd;


    string respawnAdId = "ca-app-pub-1962167994065434/1026527205";
    string gameOverAdId = "ca-app-pub-1962167994065434/5399272086";

    InterstitialAd gameOverAd;
    RewardBasedVideoAd rewardAd;

    private void Awake()
    {
        if (Instance == null)
        {
            countRestart = 0;
            nextShowAd = Random.Range(2, 4);
            Instance = this;
            rewardAd = RewardBasedVideoAd.Instance;
            LoadRewardAd();
            //rewardAd.OnAdLoaded += HandleOnRewardAdLoaded;

            gameOverAd = new InterstitialAd(gameOverAdId);
            LoadGameOverAd();
            //gameOverAd.OnAdLoaded += HandleOnGameOverAdLoaded;
            DontDestroyOnLoad(gameObject);
        } else
        {
            Destroy(gameObject);
        }
        
    }
    
    private void Start()
    {
       
        /*
        rewardAd.OnAdFailedToLoad += HandleOnAdFailedToLoad;
        
        rewardAd.OnAdStarted += HandleOnAdStarted;
        
        rewardAd.OnAdOpening += HandleOnAdOpening;
        rewardAd.OnAdLeavingApplication += HandleOnAdLeavingApplication;*/
    }
    public void ShowRewardAd(System.EventHandler<Reward> HandleOnAdRewarded, System.EventHandler<System.EventArgs> HandleOnAdClosed)
    {
        if (!PurchaseManager.isDisabledAd)
        {
            rewardAd.OnAdClosed += HandleOnAdClosed;
            rewardAd.OnAdRewarded += HandleOnAdRewarded;
            StopAllCoroutines();
            StartCoroutine(ShowRewardAdAfterLoaded());
        }
    }
    public void ShowGameOverAd(System.EventHandler<System.EventArgs> HandleOnAdClosed)
    {
        if(!PurchaseManager.isDisabledAd)
        {
            gameOverAd.OnAdClosed += HandleOnAdClosed;
            StopAllCoroutines();
            StartCoroutine(ShowGameOverAdAfterLoaded());
        }
        
    }

    IEnumerator ShowGameOverAdAfterLoaded()
    {
        while (!gameOverAd.IsLoaded())
        {
            yield return new WaitForSecondsRealtime(.1f);
        }
        gameOverAd.Show();
    }
    IEnumerator ShowRewardAdAfterLoaded()
    {
        while (!rewardAd.IsLoaded())
        {
            yield return new WaitForSecondsRealtime(.1f);
        }
        rewardAd.Show();
    }
    public void LoadGameOverAd()
    {
        if (!PurchaseManager.isDisabledAd)
        {
            gameOverAd.LoadAd(new AdRequest.Builder().Build());
        }  
    }
    public void LoadRewardAd()
    {
        if (!PurchaseManager.isDisabledAd)
        {
            rewardAd.LoadAd(new AdRequest.Builder().Build(), respawnAdId);
        }   
    }
    //Handlers
    /*
    void HandleOnGameOverAdLoaded(object sender, System.EventArgs args)
    {
        isGameOverAdReady = true;
    }
    void HandleOnRewardAdLoaded(object sender, System.EventArgs args)
    {
        isRewardAdReady = true;
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

    }*/
}
