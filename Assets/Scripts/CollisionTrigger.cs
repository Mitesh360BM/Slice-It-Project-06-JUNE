using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CollisionTrigger : MonoBehaviour
{
    [Serializable]
    public class CollisionTriggerEvent : UnityEvent<Collider, Collision> { }
    
    public Collider thisCollider;
    public CollisionTriggerEvent OnCollisionTriggerEnter;
    public CollisionTriggerEvent OnCollisionTriggerStay;
    public CollisionTriggerEvent OnCollisionTriggerExit;

#if UNITY_EDITOR
    private void OnValidate() 
    {
        if (thisCollider == null) thisCollider = GetComponent<Collider>();
    }
#endif

    private void OnCollisionEnter(Collision other) 
    {
        OnCollisionTriggerEnter?.Invoke(thisCollider, other);
    }

    private void OnCollisionExit(Collision other) 
    {
        OnCollisionTriggerExit?.Invoke(thisCollider, other);
    }

    private void OnCollisionStay(Collision other) 
    {
        OnCollisionTriggerStay?.Invoke(thisCollider, other);
    }
}
