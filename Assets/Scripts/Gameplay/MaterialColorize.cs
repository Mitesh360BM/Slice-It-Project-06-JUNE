using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaterialColorize : MonoBehaviour
{
    public List<TargetInfo> targets;

    void Start()
    {
        var randomMat = GameManager.Instance.GetRandomMaterial();
        for (int i = 0; i < targets.Count; i++)
        {
            var info = targets[i];
            if (info == null) continue;
            if (info.renderer == null) continue;

            var materials = info.renderer.sharedMaterials;

            for (int ii = 0; ii < info.materialIndex.Count; ii++)
            {
                var index = info.materialIndex[ii];
                if (index < 0 | index >= materials.Length) continue;

                materials[index] = randomMat;
            }

            info.renderer.sharedMaterials = materials;
        }
    }
    
    [Serializable]
    public class TargetInfo
    {
        public Renderer renderer;
        public List<int> materialIndex;
    }
}
