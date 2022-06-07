using System;
using System.Collections;
using System.Collections.Generic;
using Tiny;
using UnityEngine;

public class GameplayUI : Tiny.Controller
{
    private Data popupData;

    public override void OnActived(object data)
    {
        if (data is Data) popupData = data as Data;
        base.OnActived(data);
    }

    public override void OnDeactived()
    {
        base.OnDeactived();
    }

    public void OnReplayClick()
    {
        if (popupData != null) popupData.OnReplay?.Invoke();
    }

    public class Data
    {
        public Action OnReplay;
    }
}
