using System;
using System.Collections;
using System.Collections.Generic;
using GoogleMobileAds.Api;
using Tiny;
using UnityEngine;

public class GameOver : Tiny.Controller
{
    private Data popupData;

    public override void OnActived(object data)
    {
        if (data is Data) popupData = data as Data;
        base.OnActived(data);
    }

    //public void OnReplayClick()
    //{
    //    Tiny.AdManager.Instance.ShowInterstitial();
    //    if (popupData != null) popupData.OnReplay?.Invoke();
    //}

    //public void OnSkipClick()
    //{
    //    AdManager.Instance.OnRewarded -= OnRewarded;
    //    AdManager.Instance.OnRewarded += OnRewarded;

    //    AdManager.Instance.ShowReward();
    //}

    //private void OnRewarded(Reward reward)
    //{
    //    AdManager.Instance.OnRewarded -= OnRewarded;

    //    DoOnMainThread.Do(() => 
    //    {
    //        if (popupData != null) popupData.OnSkip?.Invoke();
    //    });
    //}

    public class Data
    {
        public Action OnReplay;
        public Action OnSkip;
    }
}
