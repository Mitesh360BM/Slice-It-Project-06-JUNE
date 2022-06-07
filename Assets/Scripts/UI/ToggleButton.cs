using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ToggleButton : MonoBehaviour
{
    public Action<bool> OnValueChanged;

    public Button button;
    public Image icon;
    public Sprite onSprite;
    public Sprite offSprite;

    private bool isOn = false;
    public bool IsOn
    {
        get { return isOn; }
        set
        {
            if (value == isOn) return;

            isOn = value;
            OnValueChanged?.Invoke(isOn);
            UpdateUI();
        }
    }

    private void Start()
    {
        UpdateUI();

        button.onClick.RemoveAllListeners();
        button.onClick.AddListener(() =>
        {
            IsOn = !isOn;
        });
    }

    private void UpdateUI()
    {
        icon.sprite = isOn ? onSprite : offSprite;
    }
}
