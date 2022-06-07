using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CoinUI : MonoBehaviour
{
    public Text coinText;
    
    private void OnEnable() 
    {
        UpdateUI();

        UserDataManager.Instance.OnDataChanged -= OnDataChanged;
        UserDataManager.Instance.OnDataChanged += OnDataChanged;
    }

    private void OnDataChanged()
    {
        UpdateUI();
    }

    private void OnDisable() 
    {
        if (UserDataManager.Instance == null) return;
        UserDataManager.Instance.OnDataChanged -= OnDataChanged;
    }

    private void UpdateUI()
    {
        var coin = UserDataManager.Instance.UserData.coin;
        coinText.text = $"${coin}";
    }
}
