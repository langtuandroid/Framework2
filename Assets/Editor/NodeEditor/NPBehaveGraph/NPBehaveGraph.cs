//------------------------------------------------------------
// Author: 烟雨迷离半世殇
// Mail: 1778139321@qq.com
// Data: 2021年6月15日 11:19:33
//------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

using Framework;
using GraphProcessor;
using Plugins.NodeEditor;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using UnityEngine;

public class NPBehaveGraph : BaseGraph
{
    [BoxGroup("本Canvas所有数据整理部分")] [LabelText("保存文件名"), GUIColor(0.9f, 0.7f, 1)]
    public string Name;

    [BoxGroup("本Canvas所有数据整理部分")] [LabelText("保存路径(客户端)"), GUIColor(0.1f, 0.7f, 1)] [FolderPath]
    public string SavePathClient;

    [BoxGroup("此行为树数据载体(客户端)")] [DisableInEditorMode]
    public NP_DataSupportorBase NpDataSupportor_Client = new NP_DataSupportorBase();

    [BoxGroup("行为树反序列化测试(客户端)")] [DisableInEditorMode]
    public NP_DataSupportorBase NpDataSupportor_Client_Des = new NP_DataSupportorBase();

    /// <summary>
    /// 黑板数据管理器
    /// </summary>
    [HideInInspector] public NP_BlackBoardDataManager NpBlackBoardDataManager = new NP_BlackBoardDataManager();

    //当前Canvas所有NP_Node
    private List<NP_NodeBase> m_AllNodes = new List<NP_NodeBase>();

    /// <summary>
    /// 自动配置当前图所有数据（结点，黑板）
    /// </summary>
    /// <param name="npDataSupportorBase">自定义的继承于NP_DataSupportorBase的数据体</param>
    [Button("自动配置所有结点数据(客户端)", 25), GUIColor(0.4f, 0.8f, 1)]
    public virtual void AutoSetCanvasDatas()
    {
        if (this.NpDataSupportor_Client == null)
        {
            NpDataSupportor_Client = new NP_DataSupportorBase();
        }

        this.OnGraphEnable();
        NP_BlackBoardHelper.SetCurrentBlackBoardDataManager(this);

        PrepareAllNodeData();
        this.AutoSetNP_NodeData(this.NpDataSupportor_Client);
                                     this.AutoSetNP_BBDatas(this.NpDataSupportor_Client);
    }

    // 准备所有节点的数据
    private void PrepareAllNodeData()
    {
        m_AllNodes.Clear();

        foreach (var node in this.nodes)
        {
            if (node is NP_NodeBase mNode)
            {
                m_AllNodes.Add(mNode);
            }
        }

        //排序
        m_AllNodes.Sort((x, y) => -x.position.y.CompareTo(y.position.y));

        //配置每个节点Id
        foreach (var node in m_AllNodes)
        {
            node.NP_GetNodeData().id = IdGenerator.Instance.GenerateId();
        }
    }

    [Button("保存行为树信息为二进制文件", 25), GUIColor(0.4f, 0.8f, 1)]
    public void Save()
    {
        if (string.IsNullOrEmpty(SavePathClient) ||
            string.IsNullOrEmpty(Name))
        {
            Log.Error($"保存路径或文件名不能为空，请检查配置");
            return;
        }

        using (FileStream file = File.Create($"{SavePathClient}/{this.Name}.bytes"))
        {
            var bytes = NpDataSupportor_Client.ToBson();
            file.Write(bytes,0,bytes.Length);
        }

        Log.Msg($"保存 {SavePathClient}/{this.Name}.bytes 成功");
    }

    [Button("测试反序列化", 25), GUIColor(0.4f, 0.8f, 1)]
    public void TestDe()
    {
        try
        {
            this.NpDataSupportor_Client_Des = null;
            using (var fs = new FileStream($"{SavePathClient}/{this.Name}.bytes", FileMode.Open, FileAccess.Read,
                       FileShare.Read))
            using (var reader = new BinaryReader(fs))
            {
                this.NpDataSupportor_Client_Des =
                    SerializationUtility.DeserializeValue<NP_DataSupportorBase>(reader.BaseStream,
                        DataFormat.Binary);
                Log.Msg($"反序列化{SavePathClient}/{this.Name}.bytes成功");
            }
        }
        catch (Exception e)
        {
            Log.Msg(e.ToString());
            throw;
        }
    }

    /// <summary>
    /// 自动配置所有行为树结点
    /// </summary>
    /// <param name="npDataSupportorBase">自定义的继承于NP_DataSupportorBase的数据体</param>
    private void AutoSetNP_NodeData(NP_DataSupportorBase npDataSupportorBase)
    {
        if (npDataSupportorBase == null)
        {
            return;
        }

        npDataSupportorBase.NPBehaveTreeDataId = 0;
        npDataSupportorBase.NP_DataSupportorDic.Clear();

        //当前Canvas所有NP_Node
        List<NP_NodeBase> m_ValidNodes = new List<NP_NodeBase>();

        foreach (var node in m_AllNodes)
        {
            if (node is NP_NodeBase mNode)
            {
                m_ValidNodes.Add(mNode);
            }
        }

        if (npDataSupportorBase.NPBehaveTreeDataId == 0)
        {
            //设置为根结点Id
            npDataSupportorBase.NPBehaveTreeDataId = m_ValidNodes[m_ValidNodes.Count - 1].NP_GetNodeData().id;
        }
        else
        {
            m_ValidNodes[m_ValidNodes.Count - 1].NP_GetNodeData().id = npDataSupportorBase.NPBehaveTreeDataId;
        }

        foreach (var node in m_ValidNodes)
        {
            //获取结点对应的NPData
            NP_NodeDataBase mNodeData = node.NP_GetNodeData();
            if (mNodeData.LinkedIds == null)
            {
                mNodeData.LinkedIds = new List<long>();
            }

            mNodeData.LinkedIds.Clear();

            //出结点连接的Nodes
            List<NP_NodeBase> theNodesConnectedToOutNode = new List<NP_NodeBase>();

            foreach (var outputNode in node.GetOutputNodes())
            {
                if (m_ValidNodes.Contains(outputNode))
                {
                    theNodesConnectedToOutNode.Add(outputNode as NP_NodeBase);
                }
            }

            //对所连接的节点们进行排序
            theNodesConnectedToOutNode.Sort((x, y) => x.position.x.CompareTo(y.position.x));

            //配置连接的Id，运行时实时构建行为树
            foreach (var npNodeBase in theNodesConnectedToOutNode)
            {
                mNodeData.LinkedIds.Add(npNodeBase.NP_GetNodeData().id);
            }

            //将此结点数据写入字典
            npDataSupportorBase.NP_DataSupportorDic.Add(mNodeData.id, mNodeData);
        }
    }

    /// <summary>
    /// 自动配置黑板数据
    /// </summary>
    /// <param name="npDataSupportorBase">自定义的继承于NP_DataSupportorBase的数据体</param>
    private void AutoSetNP_BBDatas(NP_DataSupportorBase npDataSupportorBase)
    {
        npDataSupportorBase.NP_BBValueManager.Clear();
        //设置黑板数据
        foreach (var bbvalues in NP_BlackBoardDataManager.CurrentEditedNP_BlackBoardDataManager.BBValues)
        {
            npDataSupportorBase.NP_BBValueManager.Add(bbvalues.Key, bbvalues.Value);
        }
    }

}