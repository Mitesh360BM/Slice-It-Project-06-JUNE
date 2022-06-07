using System.Collections;
using System.Collections.Generic;
using Tiny;
using UnityEngine;

public class LevelManager :Singleton<LevelManager>
    { 
    #region PUBLIC_VARS

    public List<Level> listOfLevels;
    public int zPos=40;
    #endregion

    #region PRIVATE_VARS
    #endregion

    #region UNITY_CALLBACKS
    public void Start()
    {
        GenerateNewLevel(); 
    }
    #endregion

    #region PUBLIC_FUNCTIONS
    public void GenerateNewLevel()
    {
        if (GameManager.Instance.currentLevel.gameObject != null)
        {
            Destroy(GameManager.Instance.currentLevel.gameObject);
        }
        int randomNumber = Random.Range(0, listOfLevels.Count);
        GameManager.Instance.currentLevel = Instantiate(listOfLevels[randomNumber],
            new Vector3(0, 0, zPos),Quaternion.identity,transform);
        GameManager.Instance.currentLevel.transform.SetAsFirstSibling();
    }

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
