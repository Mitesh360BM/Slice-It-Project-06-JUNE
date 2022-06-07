using UnityEngine;

public class Rotator : MonoBehaviour
{
    public float speed;

    private void Update()
    {
        var rotation = transform.localEulerAngles;
        rotation.z += speed * Time.deltaTime;

        transform.localEulerAngles = rotation;
    }
}