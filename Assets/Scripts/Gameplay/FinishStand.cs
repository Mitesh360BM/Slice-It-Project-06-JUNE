using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinishStand : MonoBehaviour
{
    private void OnTriggerEnter(Collider other) 
    {
        if ((other.CompareTag("Blade") || other.CompareTag("Body"))&&GameManager.Instance.IsReachEnd == false)
        {
            GameManager.Instance.ReachEnd();
            LevelManager.Instance.GenerateNewLevel();
        }
    }
}
