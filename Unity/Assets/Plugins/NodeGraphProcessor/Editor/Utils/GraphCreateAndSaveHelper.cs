﻿//------------------------------------------------------------
// Author: 烟雨迷离半世殇
// Mail: 1778139321@qq.com
// Data: 2021年6月9日 14:08:27
//------------------------------------------------------------

using System;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace GraphProcessor
{
    public static class GraphCreateAndSaveHelper
    {
        /// <summary>
        ///     NodeGraphProcessor路径前缀
        /// </summary>
        public const string NodeGraphProcessorPathPrefix = "Assets/Plugins/NodeGraphProcessor";

        public static BaseGraph CreateGraph(Type graphType)
        {
            var baseGraph = ScriptableObject.CreateInstance(graphType) as BaseGraph;
            var panelPath = $"{NodeGraphProcessorPathPrefix}/Examples/Saves/";
            Directory.CreateDirectory(panelPath);
            var panelFileName = "Graph";
            var path = EditorUtility.SaveFilePanelInProject("Save Graph Asset", panelFileName, "asset", "", panelPath);
            if (string.IsNullOrEmpty(path))
            {
                Debug.Log("创建graph已取消");
                return null;
            }

            AssetDatabase.CreateAsset(baseGraph, path);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            return baseGraph;
        }

        public static void SaveGraphToDisk(BaseGraph baseGraphToSave)
        {
            EditorUtility.SetDirty(baseGraphToSave);
            AssetDatabase.SaveAssets();
        }
    }
}