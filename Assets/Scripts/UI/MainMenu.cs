using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenu : Tiny.Controller
{
    public override void OnActived(object data) 
    { 
        base.OnActived(data);
    }

    public override void OnDeactived()
    {
        base.OnDeactived();
    }

    public void StartBtnClicked()
    {
        GameManager.Instance.StartGame();
    }

    public void SettingBtnClicked()
    {
        Tiny.SceneManager.Instance.Popup(PopupName.Settings);
    }
}
