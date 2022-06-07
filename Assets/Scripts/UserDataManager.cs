using System;
using System.Collections;
using System.Collections.Generic;
using Tiny;
using UnityEngine;

public class UserDataManager : Singleton<UserDataManager>
{
    public Action OnDataChanged;
    
    private UserData userData;
    public UserData UserData { get { return userData; } }

    private void Awake() 
    {
        DontDestroyOnLoad(this);
    }

    public void Init()
    {
        var dataJson = PlayerPrefs.GetString("USER_DATA_KEY", string.Empty);
        if (!string.IsNullOrEmpty(dataJson))
        {
            var data = JsonUtility.FromJson<UserData>(dataJson);
            if (data != null) 
            {
                userData = data;
            }
            else
            {
                CreateDefaultData();
            }
        }
        else
        {
            CreateDefaultData();
        }
    }

    private void CreateDefaultData()
    {
        userData = new UserData()
        {
            level = 0,
            coin = 0
        };

        Save();
    }

    public void Save()
    {
        if (userData == null) return;

        PlayerPrefs.SetString("USER_DATA_KEY", JsonUtility.ToJson(userData));
        PlayerPrefs.Save();
    }

    public void NextLevel()
    {
        if (userData == null) CreateDefaultData();

        userData.level += 1;
        Save();

        OnDataChanged?.Invoke();
    }

    public void AddCoin(int num)
    {
        if (userData == null) CreateDefaultData();

        userData.coin += num;
        Save();

        OnDataChanged?.Invoke();
    }

    public void UseCoin(int num)
    {
        if (userData == null) return;
        if (userData.coin < num) return;

        userData.coin -= num;
        Save();

        OnDataChanged?.Invoke();
    }

    public void SetSoundOption(bool opt)
    {
        if (userData == null) return;

        userData.soundOption = opt;
        Save();

        OnDataChanged?.Invoke();
    }

    public void SetHapticOption(bool opt)
    {
        if (userData == null) return;

        userData.hapticOption = opt;
        Save();

        OnDataChanged?.Invoke();
    }
}
