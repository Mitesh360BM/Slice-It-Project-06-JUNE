using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EzySlice;

public class Item : MonoBehaviour
{
    public GameObject meshRenderer;
    public float height = 1f;

    [Header("FOR RUNTIME SLICE")]
    public Material crossMaterial;

    [Header("FOR PRE CREATE SLICE")]
    public GameObject slicePrefab;

    private bool sliced = false;

    void Start()
    {
        sliced = false;
        OnInit();
    }

    protected virtual void OnInit()
    {
    }

    public void OnCollisionTriggerEnter(Collider thisCollider, Collision other) 
    {
        if (sliced) return;
        if (other == null) return;
        if (other.collider == null) return;

        if (other.collider.CompareTag("Blade") && !sliced)
        {
            gameObject.SetActive(false);
            sliced = true;

            if (slicePrefab != null)
            {
                var sliceObj = Instantiate(slicePrefab);
                sliceObj.transform.SetParent(transform.parent, false);
                sliceObj.transform.position = transform.position;
                sliceObj.transform.localRotation = slicePrefab.transform.localRotation;

                for (int i = 0; i < sliceObj.transform.childCount; i++)
                {
                    var child = sliceObj.transform.GetChild(i);
                    var rigid = child.GetComponent<Rigidbody>();
                    if (rigid == null) continue;

                    AddForce(rigid, i);
                }
            }
            else
            {
                Slice(other.gameObject.transform.position);
            }

            GameManager.Instance.AddScore(gameObject);
            GameManager.Instance.DoHaptic();
            SoundManager.Instance.PlayRandomCutClip();
        }
    }

    private void Slice(Vector3 slicePosition)
    {
        if (meshRenderer == null) return;

        var crossMat = (crossMaterial != null) ? crossMaterial : GameManager.Instance.GetRandomMaterial();
        var parts = meshRenderer.SliceInstantiate(slicePosition, Vector3.right, crossMat);

        if (parts == null)
        {
            Debug.Log("SliceInstantiate ERROR!");
            return;
        }

        var parent = new GameObject($"{gameObject.name}_Slice");
        parent.transform.SetParent(transform.parent, false);
        parent.transform.position = transform.position;

        for (int i = 0; i < parts.Length; i++)
        {
            var item = parts[i];

            item.transform.SetParent(parent.transform, false);

            var collider = item.AddComponent<MeshCollider>();
            collider.convex = true;
            collider.tag = "Item";

            var rigid = item.AddComponent<Rigidbody>();
            AddForce(rigid, i);
        }
    }

    private void AddForce(Rigidbody rigid, int i)
    {
        rigid.mass = 500f;
        rigid.interpolation = RigidbodyInterpolation.Interpolate;

        var dir = (i % 2 == 0) ? 1 : -1;
        rigid.AddForce(Vector3.right * 300f * dir, ForceMode.Acceleration);
    }
}
