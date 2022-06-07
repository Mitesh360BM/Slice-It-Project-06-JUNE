using System.Collections;
using System.Collections.Generic;
using System.IO;
using EzySlice;
using UnityEditor;
using UnityEngine;

public class SliceToolEditor : EditorWindow
{
    [MenuItem ("Tools/Slice Tool")]

    public static void  ShowWindow () 
    {
        var window = (SliceToolEditor)EditorWindow.GetWindow(typeof(SliceToolEditor));
        window.Show();
    }

    private void OnGUI() 
    {
        EditorGUILayout.BeginVertical(new GUIStyle { padding = new RectOffset(10, 10, 10, 10) });

        objectToSlice = (GameObject)EditorGUILayout.ObjectField("Object to Slice", objectToSlice, typeof(GameObject), true);
        crossMaterial = (Material)EditorGUILayout.ObjectField("Cross Material", crossMaterial, typeof(Material), false);
        direction = EditorGUILayout.Vector3Field("Direction", direction);
        offset = EditorGUILayout.Vector3Field("Offset", offset);
        savePath = EditorGUILayout.TextField("Save Path", savePath);

        if (GUILayout.Button("Slice"))
        {
            Slice();
        }

        EditorGUILayout.EndVertical();
    }

    private GameObject objectToSlice;
    private Material crossMaterial;
    private Vector3 offset = Vector3.zero;
    private Vector3 direction = Vector3.up;
    private string savePath = "Assets/Models";
    
    public void Slice()
    {
        if (objectToSlice == null)
        {
            Debug.Log("objectToSlice is NULL!");
            return;
        }

        var parts = objectToSlice.SliceInstantiate(objectToSlice.transform.position + offset, direction, crossMaterial);
        if (parts == null)
        {
            Debug.Log("SliceInstantiate ERROR!");
            return;
        }

        var parent = new GameObject($"{objectToSlice.name}_Slice");

        for (int i = 0; i < parts.Length; i++)
        {
            var item = parts[i];

            item.transform.SetParent(parent.transform, false);

            var collider = item.AddComponent<MeshCollider>();
            collider.convex = true;

            item.AddComponent<Rigidbody>();

            var meshFilter = item.GetComponent<MeshFilter>();
            var mesh = SaveMesh(meshFilter.sharedMesh, $"{objectToSlice.name}_{i}");
            if (mesh != null)
            {
                meshFilter.sharedMesh = mesh;
            }
        }
    }

    public Mesh SaveMesh(Mesh mesh, string name)
    {
        if (string.IsNullOrEmpty(name)) return null;
        if (mesh == null) return null;

        if (!Directory.Exists(savePath))
        {
            Directory.CreateDirectory(savePath);
        }

        var path = $"{savePath}/{name}.asset";

        AssetDatabase.CreateAsset(mesh, path);
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();

        Debug.Log("Save: " + path);

        var newMesh = AssetDatabase.LoadAssetAtPath<Mesh>(path);
        return newMesh;
    }
}
