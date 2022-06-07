//using System;
//using System.Collections;
//using UnityEngine;

//using GoogleMobileAds.Api;

//namespace Tiny
//{
//    public class AdManager : Singleton<AdManager>
//    {
//        [Serializable]
//        public class AdConfig
//        {
//            public string bannerId;
//            public string interstitialId;
//            public string rewardVideoId;
//        }

//        public AdConfig android;
//        public AdConfig ios;

//        public Action<Reward> OnRewarded;
//        public Action OnRewardAdClosed;
//        public Action OnInterstitialAdClosed;
//        public Action OnRewardAdLoaded;

//        private BannerView _banner;
//        private InterstitialAd _interstitial;
//        private RewardedAd _rewardAd;

//        private string _bannerId;
//        private string _interstitialId;
//        private string _rewardVideoId;

//        private int _numberToRetry = 5;
//        private int _numberToRetryLoadReward = 5;
//        private int _numberToRetryLoadInterstitial = 5;

//        public AdConfig Config
//        {
//            get
//            {
//                #if UNITY_IOS
//                    return ios;
//                #else
//                    return android;
//                #endif
//            }
//        }

//        public IEnumerator Init()
//        {
//            _bannerId = Config.bannerId;
//            _interstitialId = Config.interstitialId;
//            _rewardVideoId = Config.rewardVideoId;

//            MobileAds.Initialize((status) => 
//            {
//            });

//            // banner
//            _banner = new BannerView(_bannerId, AdSize.SmartBanner, AdPosition.Bottom);
//            _banner.OnAdFailedToLoad += onBannerLoadFailed;
//            _banner.OnAdLoaded += onBannerLoaded;
//            _numberToRetry = 5;
//            loadBanner();

//            _interstitial = new InterstitialAd(_interstitialId);
//            _interstitial.OnAdClosed += onInterstitialAdClosed;
//            _interstitial.OnAdLoaded += onInterstitialAdLoaded;
//            _interstitial.OnAdFailedToLoad += onInterstitialAdFailedToLoad;
//            _numberToRetryLoadInterstitial = 5;
//            PreloadInterstitial();

//            _rewardAd = new RewardedAd(_rewardVideoId);

//            _rewardAd.OnAdLoaded += HandleRewardAdLoaded;
//            _rewardAd.OnAdFailedToLoad += HandleRewardAdFailedToLoad;
//            _rewardAd.OnUserEarnedReward += HandleRewardAdVideoRewarded;
//            _rewardAd.OnAdClosed += HandleRewardAdVideoClosed;
//            _numberToRetryLoadReward = 5;
//            PreloadRewardAd();

//            yield return new WaitForEndOfFrame();
//        }

//        private void HandleRewardAdFailedToLoad(object sender, AdFailedToLoadEventArgs e)
//        {
//            Debug.Log("HandleRewardBasedVideoFailedToLoad()");
//            if (_numberToRetryLoadReward > 0)
//            {
//                _numberToRetryLoadReward--;
//                PreloadRewardAd();
//            }
//        }

//        private void HandleRewardAdLoaded(object sender, EventArgs e)
//        {
//            _numberToRetryLoadReward = 5;

//            if (OnRewardAdLoaded != null) OnRewardAdLoaded();
//        }

//        private void onAdLeavingApplication(object sender, EventArgs e)
//        {
//        }

//        public void Destroy()
//        {
//            if (_banner != null) _banner.Destroy();
//            if (_interstitial != null) _interstitial.Destroy();
//            if (_rewardAd != null) _rewardAd.Destroy();
//        }

//        public void ShowBanner()
//        {
//            if (_banner != null) _banner.Show();
//        }

//        public void HideBanner()
//        {
//            if (_banner != null) _banner.Hide();
//        }

//        public void PreloadInterstitial()
//        {
//            if (_interstitial == null) return;

//            AdRequest request = new AdRequest.Builder().Build();
//            _interstitial.LoadAd(request);
//        }

//        public bool IsInterstitialLoaded()
//        {
//            if (_interstitial == null) return false;

//            return _interstitial.IsLoaded();
//        }

//        public void ShowInterstitial()
//        {
//            if (_interstitial == null) return;

//            if (_interstitial.IsLoaded())
//            {
//                _interstitial.Show();
//            }
//        }

//        public void PreloadRewardAd()
//        {
//            if (_rewardAd == null) return;

//            AdRequest request = new AdRequest.Builder().Build();
//            _rewardAd.LoadAd(request);
//        }

//        public bool IsRewardLoaded()
//        {
//            if (_rewardAd == null) return false;
    
//            return _rewardAd.IsLoaded();
//        }

//        public void ShowReward()
//        {
//#if UNITY_EDITOR
//            if (OnRewarded != null) OnRewarded(null);
//            return;
//#endif
//            if (_rewardAd.IsLoaded())
//            {
//                _rewardAd.Show();
//            }
//        }

//        private void loadBanner()
//        {
//            AdRequest request = new AdRequest.Builder().Build();
//            _banner.LoadAd(request);
//        }

//        private void onBannerLoaded(object sender, EventArgs e)
//        {
//            Debug.Log("banner load success.");
//            _numberToRetry = 5;

//            ShowBanner();
//        }

//        private void onBannerLoadFailed(object sender, AdFailedToLoadEventArgs e)
//        {
//            Debug.Log("banner load failed.");

//            // load again
//            if (_numberToRetry > 0)
//            {
//                _numberToRetry--;
//                loadBanner();
//            }
//        }

//        private void HandleRewardAdVideoClosed(object sender, EventArgs e)
//        {
//            if (OnRewardAdClosed != null) OnRewardAdClosed();

//            PreloadRewardAd();
//        }

//        private void HandleRewardAdVideoRewarded(object sender, Reward e)
//        {
//            if (OnRewarded != null) OnRewarded(e);
//        }

//        private void onInterstitialAdClosed(object sender, EventArgs e)
//        {
//            if (OnInterstitialAdClosed != null) OnInterstitialAdClosed();

//            PreloadInterstitial();
//        }

//        private void onInterstitialAdFailedToLoad(object sender, AdFailedToLoadEventArgs e)
//        {
//            if (_numberToRetryLoadInterstitial > 0)
//            {
//                _numberToRetryLoadInterstitial--;
//                PreloadInterstitial();
//            }
//        }

//        private void onInterstitialAdLoaded(object sender, EventArgs e)
//        {
//            _numberToRetryLoadInterstitial = 5;
//        }

//        private void onInterstitialAdClick(object sender, EventArgs e)
//        {
//        }
//    }
//}