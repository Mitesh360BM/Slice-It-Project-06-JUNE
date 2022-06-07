using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelUI : MonoBehaviour
{
    public Text levelText;
    
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
        var lv = UserDataManager.Instance.UserData.level + 1;
        levelText.text = $"LEVEL {lv}";
    }
}
