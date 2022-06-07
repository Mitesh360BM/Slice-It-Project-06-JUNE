using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Settings : Tiny.Controller
{
    public ToggleButton soundBtn;
    public ToggleButton hapticBtn;

    public override void OnActived(object data)
    {
        base.OnActived(data);

        soundBtn.IsOn = UserDataManager.Instance.UserData.soundOption;
        soundBtn.OnValueChanged -= OnSoundChanged;
        soundBtn.OnValueChanged += OnSoundChanged;

        hapticBtn.IsOn = UserDataManager.Instance.UserData.hapticOption;
        hapticBtn.OnValueChanged -= OnHapticChanged;
        hapticBtn.OnValueChanged += OnHapticChanged;
    }

    private void OnHapticChanged(bool isOn)
    {
        UserDataManager.Instance.SetHapticOption(isOn);
    }

    private void OnSoundChanged(bool isOn)
    {
        UserDataManager.Instance.SetSoundOption(isOn);
    }

    public void OnClose()
    {
        Tiny.SceneManager.Instance.Close();
    }
}
