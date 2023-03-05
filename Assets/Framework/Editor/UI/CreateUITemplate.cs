﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using Object = UnityEngine.Object;

namespace Framework.Editor
{
    public static class CreateUITemplate
    {
        private static FrameworkEditorConfig _config;

        public static Func<string, string> CustomLoadPath;

        [MenuItem("Assets/@Create UICode")]
        public static void CreateUiCode()
        {
            var go = Selection.activeGameObject;
            if(go == null) return;
            _config = ConfigBase.Load<FrameworkEditorConfig>();
            CreateCode(go, AssetDatabase.GetAssetPath(go));
            AssetDatabase.Refresh();
        }

        public static void ClearUIMark()
        {
            var go = Selection.activeGameObject;
            if(go == null) return;
            var ins = PrefabUtility.InstantiatePrefab(go) as GameObject;
            if(ins == null) return;
            var marks = ins.GetComponentsInChildren<UIMark>();
            foreach (var mark in marks)
            {
                Object.DestroyImmediate(mark, true);
            }
            Log.Msg(">> clear uimark success");
            PrefabUtility.ApplyPrefabInstance(ins,InteractionMode.AutomatedAction);
            AssetDatabase.Refresh();
            Object.DestroyImmediate(ins);
        }

        private static void CreateCode(GameObject obj, string uiPrefabPath)
        {
            var prefabType = PrefabUtility.GetPrefabAssetType(obj);
            if (prefabType == PrefabAssetType.NotAPrefab)
            {
                return;
            }

            var clone = PrefabUtility.InstantiatePrefab(obj) as GameObject;
            if (null == clone)
            {
                return;
            }
		
            var panelCodeInfo = new PanelCodeInfo();

            FillPanelInfo(clone.transform, uiPrefabPath, panelCodeInfo);

            Generate(panelCodeInfo);
		
            StartAddComponent2PrefabAfterCompile(obj);

            Object.DestroyImmediate(clone);
        }

        private static void FillPanelInfo(Transform transform, string prefabPath, PanelCodeInfo panelCodeInfo)
        {
            panelCodeInfo.BehaviourName = transform.name.Replace("(clone)", string.Empty);
            panelCodeInfo.PanelPath = prefabPath;
            panelCodeInfo.PanelGo = transform.gameObject;
            List<_uiMark> marks = new List<_uiMark>();
            CollectMark(transform, ref marks);
            if (marks.Count <= 0) return;
            foreach (var uiMark in marks)
            {
                string fullPath = PathToParent(uiMark.transform, transform);
                if (!panelCodeInfo.FieldFullPathToUIMark.TryGetValue(fullPath, out var uiMarks))
                {
                    uiMarks = new List<_uiMark>();
                    panelCodeInfo.FieldFullPathToUIMark.Add(fullPath, uiMarks);
                }
                uiMarks.Add(uiMark);
            }
        }

        private static void CollectMark(Transform trans, ref List<_uiMark> uiMarks)
        {
            UIMark[] uiMark = trans.GetComponents<UIMark>();
            if (uiMark.Length > 0 && uiMark[0].IgnoreChild && uiMark[0].IgnoreSelf)
            {
                return;
            }

            if (uiMark.Length <= 0 || (uiMark.Length > 0 && !uiMark[0].IgnoreChild))
            {
                for (int i = 0; i < trans.childCount; i++)
                {
                    CollectMark(trans.GetChild(i), ref uiMarks);
                }
            }

            if (uiMark.Length > 0 && uiMark[0].IgnoreSelf)
            {
                return;
            }

            string fieldName = trans.name;
            if (uiMark.Length > 0)
            {
                foreach (var mark in uiMark)
                {
                    var _mark = new _uiMark(mark);
                    uiMarks.Add(_mark);
                }
            }
            else
            {
                foreach (var component in autoAddComponents)
                {
                    var com = trans.GetComponent(component);
                    if (com != null)
                    {
                        var _mark = new _uiMark(fieldName, com, trans);
                        uiMarks.Add(_mark);
                        break;
                    }
                }
            }
        }

        private static void Generate(PanelCodeInfo panelCodeInfo)
        {
            GeneratorView(panelCodeInfo);
            GeneratorVM(panelCodeInfo);
        }

        private static void GeneratorView(PanelCodeInfo panelCodeInfo)
        {
            Directory.CreateDirectory(_config.UIConfig.GenUIScriptsPath);
            var fileName = $"{panelCodeInfo.BehaviourName}.cs";
            // 生成view
            string viewTemplate = String.Empty;
            var generateFilePath = Path.Combine(_config.UIConfig.GenUIScriptsPath, fileName);
            if (TryGetTemplate(panelCodeInfo.BehaviourName, out var templatePath))
            {
                generateFilePath = templatePath;
                viewTemplate = File.ReadAllText(templatePath);
            }
            else
            {
                viewTemplate = Resources.Load<TextAsset>("ViewTemplate").text;
            }

            string vmName = $"{panelCodeInfo.BehaviourName}VM";
            viewTemplate = viewTemplate.Replace("#ClassName", panelCodeInfo.BehaviourName);
            viewTemplate = viewTemplate.Replace("#VMName", vmName);
            viewTemplate = viewTemplate.Replace("#PrefabPath", GetPanelPath(panelCodeInfo));

            viewTemplate = Regex.Replace(viewTemplate, @"\[UI\((.*)?""", $"[UI(\"{panelCodeInfo.PanelPath}\"");

            StringBuilder componentData = new StringBuilder();
            string componentItemStr = @"
        private #type #lowName;
        public #type #name
        {
            get
            {
                if (#lowName == null)
                {
                    #lowName = go.transform.Find(""#path"").GetComponent<#type>();
                }
                return #lowName;
            }
        }";
            foreach (var uiMarks in panelCodeInfo.FieldFullPathToUIMark)
            {
                foreach (var uiMark in uiMarks.Value)
                {
                    var transformPath = uiMark.transform == panelCodeInfo.PanelGo.transform ? "" : uiMarks.Key;
                    var fieldName = uiMark.fieldName;
                    char first = fieldName[0];
                    var lowFieldName = char.ToLower(first) + fieldName.Substring(1);
                    fieldName = char.ToUpper(first) + fieldName.Substring(1);
                    var type = uiMark.component.GetType().FullName;
                    string item = componentItemStr.Replace("#type", type);
                    item = item.Replace("#lowName", lowFieldName);
                    item = item.Replace("#name", fieldName);
                    item = item.Replace("#path", transformPath);
                    componentData.AppendLine(item);
                }
            }

            viewTemplate = Regex.Replace(viewTemplate, @"#region component[\w\W]*?#endregion", $"#region component\n{componentData}\n#endregion");
            var sw = new StreamWriter(generateFilePath, false, Encoding.UTF8);
            sw.Write(viewTemplate);
            sw.Flush();
            sw.Close();
        }

        private static void GeneratorVM(PanelCodeInfo panelCodeInfo)
        {
            string className = $"{panelCodeInfo.BehaviourName}VM";
            var fileName = className + ".cs";
            var generateFilePath = Path.Combine(_config.UIConfig.GenUIScriptsPath, fileName);
            if (TryGetTemplate(panelCodeInfo.BehaviourName, out var tempPath))
            {
                generateFilePath = tempPath;
            }

            if (File.Exists(generateFilePath)) return;
            var template = Resources.Load<TextAsset>("VMTemplate").text;
            template = template.Replace("#ClassName", className);
            File.WriteAllText(generateFilePath,template);
        }

        private static bool TryGetTemplate(string fileName, out string path)
        {
            path = string.Empty;
            var guids = AssetDatabase.FindAssets($"t:script {fileName}");
            foreach (var guid in guids)
            {
                var path1 = AssetDatabase.GUIDToAssetPath(guid);
                if (Path.GetFileNameWithoutExtension(path1) == fileName)
                {
                    path = path1;
                    return true;
                }
            }
            return false;
        }

        private static void StartAddComponent2PrefabAfterCompile(GameObject uiPrefab)
        {
            var prefabPath = AssetDatabase.GetAssetPath(uiPrefab);
            if (string.IsNullOrEmpty(prefabPath))
                return;

            EditorPrefs.SetString("AutoGenUIPrefabPath", prefabPath);
        }
		
        private static string GetPanelPath(PanelCodeInfo panelCodeInfo)
        {
            if (CustomLoadPath != null)
            {
                return CustomLoadPath(panelCodeInfo.PanelPath);
            }
            return panelCodeInfo.PanelPath;
        }

        private static string PathToParent(Transform trans, Transform parent)
        {
            var retValue = new StringBuilder(trans.name);

            while (trans.parent != null)
            {
                if (trans.parent == parent)
                {
                    break;
                }

                retValue = trans.parent.name.Append("/").Append(retValue);

                trans = trans.parent;
            }

            return retValue.ToString();
        }
	
        class PanelCodeInfo
        {
            public PanelCodeInfo()
            {
                FieldFullPathToUIMark = new Dictionary<string, List<_uiMark>>();
            }
            public string BehaviourName;
            public string PanelPath;
            public GameObject PanelGo;
            public Dictionary<string, List<_uiMark>> FieldFullPathToUIMark;
        }
		
        private static List<Type> autoAddComponents = new List<Type>()
        {
            typeof(Button),
            typeof(InputField),
            typeof(Toggle),
            typeof(Dropdown),
            typeof(Slider),
            typeof(TMP_InputField)
        };

        class _uiMark
        {
            public string fieldName;
            public Object component;
            public Transform transform;
            public _uiMark(string fieldName, Object component, Transform transform)
            {
                this.fieldName = fieldName;
                this.component = component;
                this.transform = transform;
            }

            public _uiMark(UIMark uiMark)
            {
                fieldName = uiMark.FieldName;
                component = uiMark.CurComponent;
                transform = uiMark.transform;
            }
        }
    }
}