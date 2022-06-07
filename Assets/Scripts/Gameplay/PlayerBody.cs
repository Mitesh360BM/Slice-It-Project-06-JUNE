using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBody : MonoBehaviour
{
    public Player player;
    public float reverseForce = -1.5f;
    int layerMask = 1 << 8;
    RaycastHit hit;
    void FixedUpdate()
    {
        //layerMask = ~layerMask;
        //if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.up), out hit, 1f, layerMask)&& player.CurrentState == Player.State.FLIPPING )
        //{
        //    Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.up) * hit.distance, Color.yellow);
        //    Debug.Log("Hit");
        //    reveceflip();
        //}
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (CanFlipReverse(collision) && !collision.gameObject.CompareTag("levelSpawer"))
        {
            Debug.Log(collision.gameObject.name);
            ReveseFlip();
        }
    }

    private void ReveseFlip()
    {
        float playerMoveSpeed = player.moveSpeed;
        player.moveSpeed = reverseForce;
        player.StartFlip();
        player.moveSpeed = playerMoveSpeed;
    }

    private bool CanFlipReverse(Collision collision)
    {
        return !collision.gameObject.CompareTag("Water") &&
            player.CurrentState == Player.State.FLIPPING;
    }
}