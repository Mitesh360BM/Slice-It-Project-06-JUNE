using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ColliderTrigger : MonoBehaviour
{
    [Serializable]
    public class TriggerEvent : UnityEvent<Collider, Collider> { }

    public Collider thisCollider;
    public TriggerEvent OnColliderTriggerEnter;
    public TriggerEvent OnColliderTriggerStay;
    public TriggerEvent OnColliderTriggerExit;

#if UNITY_EDITOR
    private void OnValidate() 
    {
        if (thisCollider == null) thisCollider = GetComponent<Collider>();
    }
#endif

    private void OnTriggerEnter(Collider other) 
    {
        OnColliderTriggerEnter?.Invoke(thisCollider, other);
    }

    private void OnTriggerStay(Collider other) 
    {
        OnColliderTriggerStay?.Invoke(thisCollider, other);
    }

    private void OnTriggerExit(Collider other) 
    {
        OnColliderTriggerExit?.Invoke(thisCollider, other);
    }
}
