using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Tiny;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : Singleton<GameManager>
{
    public Action<GameState> OnStateChange;
    public Action<int, GameObject> OnAddScore;
    public Action<int> OnAddScoreAction;
    public GameObject OptionButton;
    public bool OptionActive;
    int index;
    public float disableScreenTime = 3.0f;

    public enum GameState
    {
        LOADING,
        LOAD_ERROR,
        READY,
        PLAY,
        WIN,
        GAMEOVER
    }

    private GameState gameState = GameState.READY;
    public GameState CurrentState
    {
        get
        {
            return gameState;
        }
        set
        {
            if (gameState == value) return;

            gameState = value;
            OnStateChange?.Invoke(gameState);

            OnStateChange_Internal();
        }
    }
    public KitchenHandler kitchenHandler;

    public List<Material> colorMaterials;
    public GameObject levelParent;
    public Level currentLevel;
    public int maxLevel = 10;
    public Player player;
    public Vector3 playerStartOffset = new Vector3(0, 2f, 2f);
    public CameraFollow cameraFollow;

    [Header("Score effect")]
    public Camera mainCamera;
    public Transform effectParent;
    public ScoreTextEffect effectPrefab;

    [Header("WOW effect")]
    public GameObject wowObj;
    public Text wowText;

    private bool reachEnd = false;
    public bool IsReachEnd
    {
        get { return reachEnd; }
    }

    public void ReachEnd()
    {
        reachEnd = true;
    }

    public void Bonus(int b)
    {
        if (b <= 0) return;
        if (bonus > 1) return;

        bonus = b;
        ScoreManager.Instance.GetBonus(b);
    }

    private int score = 0;
    public int Score { get { return score; }}

    private int bonus = 1;

    private void Awake() 
    {
        DontDestroyOnLoad(this);
    }

    private void Start() 
    {
        StartCoroutine(LoadGame());
    }

    private IEnumerator LoadGame()
    {
        CurrentState = GameState.LOADING;

        UserDataManager.Instance.Init();
        Vibration.Init();

        yield return new WaitForEndOfFrame();

        //yield return AdManager.Instance.Init();

        yield return new WaitForEndOfFrame();

        Init();
    }

    public void Init()
    {
        reachEnd = false;
        score = 0;
        bonus = 1;
        LoadLevel();
    }

    private void LoadLevel()
    {
        //var userLevel = (UserDataManager.Instance.UserData.level % maxLevel) + 1;
        //Debug.Log("LoadLevel: " + userLevel);

        //var path = $"Levels/Level_{userLevel}";
        //var prefab = Resources.Load<GameObject>(path);
        //if (prefab == null)
        //{
        //    OnLevelLoadError();
        //    return;
        //}

        //Level levelObj = Instantiate(prefab);
        //levelObj.transform.SetParent(levelParent.transform, false);
        //levelObj.transform.position = Vector3.zero;

        //if (levelObj == null)
        //{
        //    OnLevelLoadError();
        //    return;
        //}

        //Level level = levelObj.GetComponent<Level>();
        //if (level == null)
        //{
        //    OnLevelLoadError();
        //    return;
        //}

        //Destroy(currentLevel.gameObject);

        //currentLevel = level;

        OnLevelLoadDone();
    }

    private void OnLevelLoadDone()
    {
        player.Init(currentLevel.StartPosition + playerStartOffset);
        cameraFollow.ForceUpdate();

        CurrentState = GameState.READY;
    }

    private void OnLevelLoadError()
    {
        CurrentState = GameState.LOAD_ERROR;
    }

    public void StartGame()
    {
        CurrentState = GameState.PLAY;
    }

    private void OnStateChange_Internal()
    {
        SceneManager.Instance.Close();

        if (CurrentState == GameState.LOADING)
        {
            SceneManager.Instance.Popup(PopupName.Loading);
        }
        else if (CurrentState == GameState.LOAD_ERROR)
        {

        }
        else if (CurrentState == GameState.READY)
        {
            SceneManager.Instance.Popup(PopupName.MainMenu);
        }
        else if (CurrentState == GameState.PLAY)
        {
            SceneManager.Instance.Popup(PopupName.GamePlay, false, new GameplayUI.Data
            {
                OnReplay = () => {
                    Replay();
                }
            });
        }
        else if (CurrentState == GameState.GAMEOVER)
        {
            ScoreManager.Instance.Init();
            SceneManager.Instance.Popup(PopupName.GameOver, false, new GameOver.Data
            {
                OnReplay = () => {
                    Replay();
                }, 
                OnSkip = () => {
                    NextLevel();
                }
            });
        }
        else if (CurrentState == GameState.WIN)
        {
            SceneManager.Instance.Popup(PopupName.Win, false, new WinScreen.Data
            {
                coin = score,
                bonus = bonus,
                OnContinue = () => {
                    NextLevel();
                }
            });
        }
    }

    private void NextLevel()
    {
        //DestroyOldLevelItems();
        UserDataManager.Instance.NextLevel();
        Init();
        kitchenHandler.ResetKitchen();
    }


    private void Replay()
    {
        Init();
    }

    public Material GetRandomMaterial()
    {
        var rand = UnityEngine.Random.Range(0, colorMaterials.Count);
        return colorMaterials[rand];
    }

    public void AddScore(GameObject from)
    {
        score++;
        OnAddScore?.Invoke(1, from);
        CreateScoreEffect(1, from);
        OnAddScoreAction?.Invoke(1);
        //ScoreManager.Instance.UpdateScoreText(score);
    }

    public void Win()
    {
        StartCoroutine(DelayWin());
    }

    public void Lose()
    {
        CurrentState = GameState.GAMEOVER;
    }

    private IEnumerator DelayWin()
    {
        yield return new WaitForSeconds(0.5f);

        CurrentState = GameState.WIN;
    }

    public void CreateScoreEffect(int score, GameObject from)
    {
        if (from == null) return;

        var effect = Instantiate(effectPrefab);
        effect.transform.SetParent(effectParent, false);

        var total = from.transform.parent.childCount;
        effect.Init(score, from.transform, mainCamera, total - from.transform.GetSiblingIndex());
    }

    public void ShowWowText(string text)
    {
        var playerPos = player.transform.position;
        playerPos += new Vector3(-3, -1, 6);
        wowObj.transform.position = playerPos;

        wowText.text = text;
        wowText.transform.DOKill();
        wowText.DOKill();

        wowText.transform.localPosition = Vector3.zero;
        wowText.color = new Color(1, 1, 1, 0f);

        var local = wowText.transform.localPosition;
        local.y += 300f;

        wowText.DOFade(1f, 0.25f).SetDelay(0.5f);

        var seq = DOTween.Sequence();
        seq.AppendInterval(0.5f);
        seq.Append(wowText.transform.DOLocalMove(local, 0.5f).SetEase(Ease.OutBack));
        seq.AppendInterval(1f);
        seq.Append(wowText.DOFade(0f, 0.5f));
        seq.Play();
    }

    public void CheckShowWowText(int diff)
    {
        if (diff > 15)
        {
            ShowWowText("PERFECT!");
        }
        else if (diff > 10)
        {
            ShowWowText("GOOD!");
        }
        else if (diff > 5)
        {
            ShowWowText("WOW!");
        }
    }

    public void DoHaptic()
    {
        if (!UserDataManager.Instance.UserData.hapticOption) return;

        Vibration.VibratePop();
    }

    public void OnClickOptions()
    {           
         OptionButton.SetActive(true);                  
    }

    public void OnClickOption2()
    {      
        OptionButton.SetActive(false);      
    }

    public void OnApplicationQuit()
    {
        Application.Quit();
        Debug.Log("Application Quit");
    }


}
