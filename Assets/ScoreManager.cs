using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{
    public int lscore=0;
    public Text score_text;
    public static ScoreManager Instance;
    // Start is called before the first frame update
    void Start()
    {
        if (Instance == null)
            Instance = this;
    }
    private void OnEnable()
    {
        GameManager.Instance.OnAddScoreAction += UpdateScoreText;
    }
    private void OnDisable()
    {
        GameManager.Instance.OnAddScoreAction -= UpdateScoreText;
    }
    public void Init()
    {
        //lscore = 0;                   // 13/06 after timer gets on 0 the score doesnt gets 0
        UpdateScoreText();
    }
    public void UpdateScoreText(int score=0)
    {
        lscore += score;
        score_text.text = lscore + "";
    }

    public void GetBonus(int bonus)
    {
        lscore *= bonus;
        UpdateScoreText();
    }
    #region PUBLIC_VARS
    #endregion

    #region PRIVATE_VARS
    #endregion

    #region UNITY_CALLBACKS
    #endregion

    #region PUBLIC_FUNCTIONS
    #endregion

    #region PRIVATE_FUNCTIONS
    #endregion

    #region CO-ROUTINES
    #endregion

    #region EVENT_HANDLERS
    #endregion

    #region UI_CALLBACKS
    #endregion
}
