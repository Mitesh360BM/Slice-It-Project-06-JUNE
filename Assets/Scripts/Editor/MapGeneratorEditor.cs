using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(MapGenerator))]
public class MapGeneratorEditor : Editor
{
    public override void OnInspectorGUI()
	{
        DrawDefaultInspector();

        var mapEditor = target as MapGenerator;

        GUILayout.Space(20f);

        if (GUILayout.Button("Random"))
        {
            mapEditor.RandomMap();
        }
    }
}
