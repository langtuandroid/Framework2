using System;
using Framework;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class Test : MonoBehaviour
{

    [Button]
    private void Start()
    {
        var s1 = AssetDatabase.LoadAssetAtPath<MonoScript>("Assets/_Scripts/UI/UI_Image.cs");
        TextAsset t2 = new TextAsset("public class BB{}");
        AssetDatabase.AddObjectToAsset(t2, s1);
        AssetDatabase.ImportAsset(AssetDatabase.GetAssetPath(t2));
    }
}