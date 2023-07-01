using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace ToolKits
{
    public class Example_55_Window : EditorWindow
    {
        private static Example_55_Window _window;
        private static readonly Vector2 MIN_SIZE = new Vector2(800, 800);

        private Editor _editor;

        [MenuItem("Tools/DrawInspectorOnEditorWindow", priority = 55)]
        private static void PopUp()
        {
            _window = GetWindow<Example_55_Window>("");
            _window.minSize = MIN_SIZE;
            _window.Init();
            _window.Show();
        }

        private void Init()
        {
            var asset = AssetDatabase.LoadAssetAtPath<NPBehaveGraph>("Assets/Configs/AI_CloseSingleAttack.asset");
            _editor = Editor.CreateEditor(asset);
        }

        private void OnGUI()
        {
            if (null != _editor)
            {
                _editor.OnInspectorGUI();
            }
        }
    }
}