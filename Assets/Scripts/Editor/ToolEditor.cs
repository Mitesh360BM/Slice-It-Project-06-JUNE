using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class ToolEditor
{
    [MenuItem("Tools/Clear Data")]
    public static void ClearData()
    {
        PlayerPrefs.DeleteAll();
        PlayerPrefs.Save();
    }
}
