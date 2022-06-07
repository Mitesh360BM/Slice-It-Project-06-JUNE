using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventManager : MonoBehaviour
{
    public static EventManager Instance;
    public Action OnGameStart;
    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
    }
    public void RaiseOnGameStart()
    {
        if (OnGameStart != null)
            OnGameStart();
    }
}
