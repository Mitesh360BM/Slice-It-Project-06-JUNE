using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level : MonoBehaviour
{
    public int zPos = 165;
    public Vector3 StartPosition
    {
        get
        {
            if (activeStands == null) return Vector3.zero;
            if (activeStands.Count == 0) return Vector3.zero;
            
            return activeStands[0].transform.position;
            //return Vector3.zero; 
        }
    }

    public List<Stand> activeStands = new List<Stand>();
    public List<ItemGroup> activeItems = new List<ItemGroup>();
    public FinishStand finishStand;

    public void Init(List<Stand> stands, List<ItemGroup> items, FinishStand finish)
    {
        DoDestroy();

        activeStands.AddRange(stands);
        activeItems.AddRange(items);

        finishStand = finish;
    }

    public void DoDestroy()
    {
        DestroyAllStands();
        DestroyAllItems();
    }

    public void DestroyAllStands()
    {
        if (activeStands != null)
        {
            for (int i = 0; i < activeStands.Count; i++)
            {
                if (activeStands[i] == null) continue;

                #if UNITY_EDITOR
                DestroyImmediate(activeStands[i].gameObject);
                #else
                Destroy(activeStands[i].gameObject);
                #endif
            }

            activeStands.Clear();
        }

        if (finishStand != null)
        {
            #if UNITY_EDITOR
            DestroyImmediate(finishStand.gameObject);
            #else
            Destroy(finishStand.gameObject);
            #endif
        }
    }

    public void DestroyAllItems()
    {
        if (activeItems == null) return;

        for (int i = 0; i < activeItems.Count; i++)
        {
            if (activeItems[i] == null) continue;

            #if UNITY_EDITOR
            DestroyImmediate(activeItems[i].gameObject);
            #else
            Destroy(activeItems[i].gameObject);
            #endif
        }

        activeItems.Clear();
    }
}
