using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinishBonus : MonoBehaviour
{
    public int bonus;
    
    private void OnCollisionEnter(Collision other)
    {
        if (other.collider.CompareTag("Blade"))
        {
            GameManager.Instance.Bonus(bonus);
        }
    }

}
