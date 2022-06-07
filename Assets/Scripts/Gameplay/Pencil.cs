using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pencil : Item
{
    public List<Mesh> meshes;
    public MeshFilter meshFilter;

    protected override void OnInit()
    {
        base.OnInit();
    
        var meshIndex = Random.Range(0, meshes.Count);
        var mesh = meshes[meshIndex];
        meshFilter.sharedMesh = mesh;
    }
}
