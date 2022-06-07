using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimerObject : MonoBehaviour
{
    [SerializeField] private Text _timerText;
    [SerializeField] float timer = 120;
    [SerializeField] float startTimer = 8;
   
    private void OnEnable()
    {
        timer = startTimer;

    }
    private void Update()
    {
        _timerText.text = ((int)timer).ToString();
        timer -= Time.deltaTime;
        if(timer <= 0)
        {
            gameObject.SetActive(false);
            Debug.Log("GameOver");
            GameManager.Instance.Lose();
            TimeManager.Instance.StopTimer();
        }
    }
}
