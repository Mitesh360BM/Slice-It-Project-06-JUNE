using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectFollow : MonoBehaviour
{
    public Transform target;
    public Vector3 offset;
    public Vector3 direction = Vector3.one;

    private Vector3 velocity;

    void Start()
    {
        offset = transform.position - target.position;
    }

    void LateUpdate()
    {
        if (target == null) return;

        var postion = target.position + offset;
        postion.x *= direction.x;
        postion.y *= direction.y;
        postion.z *= direction.z;

        transform.position = postion;
    }
}
