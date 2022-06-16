using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WinScreen : Tiny.Controller
{
    public Text coinText;
    public HorizontalLayoutGroup horizontalLayout;
    public bool isScreenActive = false;
    private Data popupData;
    
    public override void OnActived(object data)
    {
        if (data is Data)
        {
            popupData = data as Data;

            var coin = popupData.coin;
            if (popupData.bonus > 1) coin = coin * popupData.bonus;

            coinText.text = $"+{coin}";

            LayoutRebuilder.ForceRebuildLayoutImmediate(horizontalLayout.GetComponent<RectTransform>());

            if (coin > 0)
            {
                UserDataManager.Instance.AddCoin(coin);
            }
        }
        isScreenActive = true;
        StartCoroutine(DisableWinScreen());
        base.OnActived(data);
    }
    private IEnumerator DisableWinScreen()
    {
        yield return new WaitForSeconds(GameManager.Instance.disableScreenTime);
        if (popupData != null) popupData.OnContinue?.Invoke();
    }

    public override void OnDeactived()
    {
        base.OnDeactived(); 
        isScreenActive = false;
    }

    public void OnContinueClicked()
    {
        //Tiny.AdManager.Instance.ShowInterstitial();

        if (popupData != null) popupData.OnContinue?.Invoke();
    }

    public class Data
    {
        public Action OnContinue;
        public int coin;
        public int bonus;
    }
}
