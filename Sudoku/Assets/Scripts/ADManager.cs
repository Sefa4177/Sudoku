using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GoogleMobileAds.Api;

public class ADManager : MonoBehaviour
{
    
    public static ADManager Instance;
    private BannerView _banerview;
    private InterstitialAd _interstitial;
    

    public void Awake() 
    {
        if(Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(this);
        }
        else
        {
            Destroy(this);
        }
    }

    void Start()
    {
        MobileAds.Initialize(initStatus => { });
        this.RequestBanner();
        this.RequestInterstitial();
        this.ShowBanner();
    }

    void Update() 
    {
        
    }

   public void RequestBanner()
    {
#if UNITY_ANDROID
        string AdId = "ca-app-pub-9217006338025564/1675800283";
#elif UNITY_IPHONE
        string AdId = " ";
#else
        string AdId = "Unexpected_Platform";
#endif
       

        _banerview = new BannerView(AdId,AdSize.Banner,AdPosition.Top);
        AdRequest Banner = new AdRequest.Builder().Build();

        _banerview.LoadAd(Banner);
    }

    public void ShowBanner()
    {
        _banerview.Show();
    }

    public void RequestInterstitial()
    {
#if UNITY_ANDROID
        string AdId = "ca-app-pub-9217006338025564/8105477260";
#elif UNITY_IPHONE
        string AdId = " ";
#else
        string AdId = "Unexpected_Platform";
#endif
       
        _interstitial = new InterstitialAd(AdId);
        AdRequest Interstitial = new AdRequest.Builder().Build();
        _interstitial.LoadAd(Interstitial);

    }

    public void ShowInterstitial()
    {
       if(_interstitial.IsLoaded())
         {
            _interstitial.Show();
         }
        RequestInterstitial();
    }
    
}
