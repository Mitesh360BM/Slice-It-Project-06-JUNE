using UnityEngine;
using System;
using System.Threading;
using System.Collections.Generic;

public class DoOnMainThread : MonoBehaviour
{
    private static Queue<Action> _threadQueue = new Queue<Action>();

    private void Awake()
    {
        DontDestroyOnLoad(this);
    }

    public static void Do(Action action)
    {
        lock (_threadQueue)
        {
            _threadQueue.Enqueue(action);
        }
    }

    static void OnUpdate()
    {
        Queue<Action> _mainQueue;

        lock (_threadQueue)
        {
            if (_threadQueue.Count == 0) return;

            _mainQueue = new Queue<Action>(_threadQueue);
            _threadQueue.Clear();
        }

        while (_mainQueue.Count > 0)
        {
            var action = _mainQueue.Dequeue();
            if (action == null) continue;

            try
            {
                action();
            }
            catch (Exception e)
            {
                Debug.LogWarning("DoOnMainThread error: " + e);
            }
        }
    }

    void Update()
    {
        OnUpdate();
    }
}