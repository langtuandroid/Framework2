using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Framework;
using GraphProcessor;
using MongoDB.Bson;
using Plugins.NodeEditor;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;

public class SkillGraph : NPBehaveGraph
{
    [Button("保存技能树信息为二进制文件", 25)]
    [GUIColor(0.4f, 0.8f, 1)]
    public new void Save()
    {
        if (string.IsNullOrEmpty(SavePathClient) ||
            string.IsNullOrEmpty(Name))
        {
            Log.Error($"保存路径或文件名不能为空，请检查配置");
            return;
        }

        AutoSetCanvasDatas();
        AutoSetSkillData_NodeData(NpDataSupportor_Client);
        File.WriteAllText($"{SavePathClient}/{Name}.bytes", NpDataSupportor_Client.ToJson());
        Log.Msg($"保存 {SavePathClient}/{Name}.bytes 成功");
    }

    [Button("测试技能树反序列化", 25)]
    [GUIColor(0.4f, 0.8f, 1)]
    public new void TestDe()
    {
        try
        {
            string data = File.ReadAllText($"{SavePathClient}/{Name}.bytes");
            NpDataSupportor_Client_Des = SerializeHelper.Deserialize<NP_DataSupportor>(data);
            Log.Msg($"反序列化 {SavePathClient}/{Name}.bytes 成功");
        }
        catch (Exception e)
        {
            Log.Msg(e.ToString());
            throw;
        }
    }

    private void AutoSetSkillData_NodeData(NP_DataSupportor npDataSupportor)
    {
        if (npDataSupportor.BuffNodeDataDic == null)
        {
            return;
        }

        npDataSupportor.BuffNodeDataDic.Clear();

        if (npDataSupportor == null)
        {
            return;
        }

        npDataSupportor.NPBehaveTreeDataId = 0;
        npDataSupportor.NP_DataSupportorDic.Clear();

        //当前Canvas所有NP_Node
        List<NP_NodeBase> m_ValidNodes = new();

        foreach (NP_NodeBase node in m_AllNodes)
        {
            if (node is NP_NodeBase mNode)
            {
                m_ValidNodes.Add(mNode);
            }
        }

        foreach (BaseNode node in nodes)
        {
            if (node is BuffNodeBase buffNodeBase)
            {
                buffNodeBase.AutoAddLinkedBuffs();
                BuffNodeDataBase buffNodeDataBase = buffNodeBase.GetBuffNodeData();
                npDataSupportor.BuffNodeDataDic.Add(buffNodeDataBase.NodeId.Value, buffNodeDataBase);
            }
        }

        string configPath =
            (typeof(SkillBehaveConfigFactory).GetCustomAttribute(typeof(ConfigAttribute)) as ConfigAttribute)
            .Path;
        string bytes = AssetDatabase.LoadAssetAtPath<TextAsset>(configPath).text;
        SkillBehaveConfigFactory factory = SerializeHelper.Deserialize<SkillBehaveConfigFactory>(bytes);
        SkillBehaveConfig skillCanvasData = factory.Get(IdInConfig);
        if (skillCanvasData != null)
        {
            npDataSupportor.NPBehaveTreeDataId = factory.Get(IdInConfig).NPBehaveId;
            npDataSupportor.ExcelId = IdInConfig;
            foreach (BuffNodeDataBase data in npDataSupportor.BuffNodeDataDic.Values)
            {
                if (data is SkillDesNodeData desNodeData)
                {
                    desNodeData.SkillId = skillCanvasData.ID;
                    break;
                }
            }
        }

        if (npDataSupportor.NPBehaveTreeDataId == 0)
        {
            //设置为根结点Id
            npDataSupportor.NPBehaveTreeDataId = m_ValidNodes[m_ValidNodes.Count - 1].NP_GetNodeData().id;
        }
        else
        {
            m_ValidNodes[m_ValidNodes.Count - 1].NP_GetNodeData().id = npDataSupportor.NPBehaveTreeDataId;
        }

        foreach (NP_NodeBase node in m_ValidNodes)
        {
            //获取结点对应的NPData
            NP_NodeDataBase mNodeData = node.NP_GetNodeData();
            if (mNodeData.LinkedIds == null)
            {
                mNodeData.LinkedIds = new List<long>();
            }

            mNodeData.LinkedIds.Clear();

            //出结点连接的Nodes
            List<NP_NodeBase> theNodesConnectedToOutNode = new();

            foreach (BaseNode outputNode in node.GetOutputNodes())
            {
                if (m_ValidNodes.Contains(outputNode))
                {
                    theNodesConnectedToOutNode.Add(outputNode as NP_NodeBase);
                }
            }

            //对所连接的节点们进行排序
            theNodesConnectedToOutNode.Sort((x, y) => x.position.x.CompareTo(y.position.x));

            //配置连接的Id，运行时实时构建行为树
            foreach (NP_NodeBase npNodeBase in theNodesConnectedToOutNode)
            {
                mNodeData.LinkedIds.Add(npNodeBase.NP_GetNodeData().id);
            }

            //将此结点数据写入字典
            npDataSupportor.NP_DataSupportorDic.Add(mNodeData.id, mNodeData);
        }
    }
}