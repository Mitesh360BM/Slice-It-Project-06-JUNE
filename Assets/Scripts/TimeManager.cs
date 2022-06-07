using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;
using UnityEngine.UI;

public class TimeManager : MonoBehaviour
{
    public static TimeManager Instance;
    [SerializeField] private TimerObject timer;
    [SerializeField] private int totalTime = 120;
    private void Awake()
    {
        if (Instance == null)
            Instance = this;
    }

    private void OnEnable()
    {
        EventManager.Instance.OnGameStart += StartTimer;
    }
    private void OnDisable()
    {
        EventManager.Instance.OnGameStart -= StartTimer;
    }

    public void StartTimer()
    {
        Debug.Log("Timer is started");

        timer.gameObject.SetActive(true);
        //StartCoroutine(TimerCO());
    }

    public void StopTimer()
    {
        timer.gameObject.SetActive(false);
    }
    public IEnumerator TimerCO()
    {
        while (totalTime > 0)
        {
            UpdateTimerText();
            yield return new WaitForSeconds(1);
        }
    }
    public void UpdateTimerText()
    {
        //_timerText.text = totalTime + "";
        totalTime--;
    }
    //public void timemenagement()
    //{
    //    if (timer == 120)
    //    {
    //        player.StartTimer();
    //    }
    //    if (timer == 0)
    //    {
    //        player.stoptimer();
    //    }
    //}

}
