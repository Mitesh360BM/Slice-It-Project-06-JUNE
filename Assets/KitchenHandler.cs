using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KitchenHandler : MonoBehaviour
{
    #region PUBLIC_VARS
    public List<GameObject> kitchen;
    public Player player;
    public float zPosDiffrent = 40.5f;          // 40.5
    private List<Vector3> initalKitchenPosition=new List<Vector3>();
    #endregion

    #region PRIVATE_VARS
    #endregion

    #region UNITY_CALLBACKS

    public void Start()
    {
        for (int i = 0; i < kitchen.Count; i++)
        {
            initalKitchenPosition.Add(kitchen[i].transform.position);
        }
    }

    public void Update()
    {
        for (int i = 0; i < kitchen.Count; i++)
        {
            if(player.transform.position.z > kitchen[i].transform.position.z + 20)        //15
            {
                kitchen[i].transform.position = new Vector3(kitchen[i].transform.position.x,
                    kitchen[i].transform.position.y, kitchen[i].transform.position.z + 4 * zPosDiffrent);
            }
        }
    }

    public void ResetKitchen()
    {
        for (int i = 0; i < kitchen.Count; i++)
        {
            kitchen[i].transform.position= initalKitchenPosition[i];
        }
    }

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
