using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stand : MonoBehaviour
{
    public float length = 1f;
    public bool shouldRandomLength = false;
    public int minLength = 1;
    public int maxLength = 10;

    public GameObject standRenderer;
    public Vector3 EndPosition
    {
        get
        {
            var pos = transform.position;
            pos.z += length;
            
            return pos;
        }
    }

    public void Init(Vector3 position)
    {
        transform.position = position;

        if (shouldRandomLength)
        {
            length = Random.Range(minLength, maxLength);
        }

        UpdateRenderer();
    }

    private void UpdateRenderer()
    {
        var scale = standRenderer.transform.localScale;
        scale.z = length;

        standRenderer.transform.localScale = scale;
    }
}
