using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;
    public Vector3 offset;
    public float minY;
    public float smoothness = 0.5f;

    private Vector3 velocity;

    void Start()
    {
        // offset = transform.position - target.position;
    }

    void LateUpdate()
    {
        if (target == null) return;

        var postion = target.position + offset;
        postion = Vector3.SmoothDamp(transform.position, postion, ref velocity, smoothness * Time.deltaTime);
        postion.y = Mathf.Max(minY, postion.y);

        transform.position = postion;
    }

    public void ForceUpdate()
    {
        velocity = Vector3.zero;

        var postion = target.position + offset;
        transform.position = postion;
    }
}
