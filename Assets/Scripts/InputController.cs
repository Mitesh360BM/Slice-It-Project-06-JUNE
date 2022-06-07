using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class InputController : MonoBehaviour
{
    public static event Action<Vector3> OnTouchBegan;
    public static event Action<Vector3> OnTouchMoved;
    public static event Action<Vector3> OnTouchEnded;

    public Camera mainCamera;
    public bool convertToWorldPoint;

    private static InputController instance = null;
    public static InputController Instance 
    {
        get
        {
            return instance;
        }
    }

    private Vector3 _lastPosition = Vector3.zero;
    private bool _enable = true;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Debug.LogWarning("InputController should have only 1 instance! " + instance.gameObject.name);
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(this);
    }

    void Update()
    {
        if (!_enable) return;

        if (mainCamera == null)
        {
            mainCamera = Camera.main;
        }

        if (Input.GetMouseButtonDown(0))
        {
            var curPos = convertToWorldPoint ? mainCamera.ScreenToWorldPoint(Input.mousePosition) : Input.mousePosition;
            touchBegan(curPos);
            _lastPosition = curPos;
            return;
            
        }

        if (Input.GetMouseButton(0))
        {
            var curPos = convertToWorldPoint ? mainCamera.ScreenToWorldPoint(Input.mousePosition) : Input.mousePosition;
            if (_lastPosition != curPos)
            {
                touchMoved(curPos);
                _lastPosition = curPos;
            }
            return;
        }

        if (Input.GetMouseButtonUp(0))
        {
            var curPos = convertToWorldPoint ? mainCamera.ScreenToWorldPoint(Input.mousePosition) : Input.mousePosition;
            touchEnded(curPos);
        }
    }

    public void SetEnable(bool active)
    {
        _enable = active;
    }

    public bool IsOnUI()
    {
        var eventDataCurrentPosition = new PointerEventData(EventSystem.current);
        eventDataCurrentPosition.position = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventDataCurrentPosition, results);
        return results.Count > 0;
    }

    public static Vector2 ScreenPointToLocalPointInRectangle(RectTransform target, Vector2 position)
    {
        var result = Vector2.zero;
        if (Instance.mainCamera == null) 
        {
            Debug.LogWarning("mainCamera is null!");
            return result;
        }

        RectTransformUtility.ScreenPointToLocalPointInRectangle(target, position, Instance.mainCamera, out result);
        return result;
    }

    public static Vector2 WorldToScreenPoint(Vector3 position)
    {
        var result = Vector2.zero;
        if (Instance.mainCamera == null) 
        {
            Debug.LogWarning("mainCamera is null!");
            return result;
        }

        result = RectTransformUtility.WorldToScreenPoint(Instance.mainCamera, position);
        return result;
    }

    private void touchBegan(Vector3 position)
    {
        if (OnTouchBegan != null)
        {
            OnTouchBegan(position);
        }
    }

    private void touchMoved(Vector3 position)
    {
        if (OnTouchMoved != null)
        {
            OnTouchMoved(position);
        }
    }

    private void touchEnded(Vector3 position)
    {
        if (OnTouchEnded != null)
        {
            OnTouchEnded(position);
        }
    }
}
