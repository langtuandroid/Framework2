using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Framework;
using Newtonsoft.Json;

public partial class NodeData : BaseConfig
{
    /// <summary> ID </summary>
	public int ID { get; private set; }
/// <summary> 短节点路径 </summary>
	public List<string> ShortNodePath { get; private set; }
/// <summary> 长节点路径 </summary>
	public List<string> LongNodePath { get; private set; }
/// <summary> 长节点小组件的路径 </summary>
	public string LongNodeItemPath { get; private set; }
/// <summary> 障碍物路径 </summary>
	public List<string> ColliderNodePath { get; private set; }
/// <summary> 星星路径 </summary>
	public List<string> StarNodePath { get; private set; }
/// <summary> 尝试皮肤路径 </summary>
	public List<string> TrySkinNodePath { get; private set; }
/// <summary> 宝箱路径 </summary>
	public List<string> ChestKeyNode { get; private set; }
/// <summary> 移动障碍物路径 </summary>
	public List<string> MoveColliderNodePath { get; private set; }
/// <summary> 跳台路径 </summary>
	public List<string> TablePath { get; private set; }
/// <summary> 跳台路径 </summary>
	public List<string> JumpHolrPath { get; private set; }
/// <summary> 门路径 </summary>
	public List<string> DoorPath { get; private set; }
/// <summary> 双倍路径 </summary>
	public List<string> DoublePath { get; private set; }
/// <summary> 吸铁石路径 </summary>
	public List<string> MagnetPath { get; private set; }
/// <summary> 空节点 </summary>
	public List<string> Empty { get; private set; }

}

[Config("Assets/Res/Configs/NodeData.bytes")]
public class NodeDataFactory : ConfigSingleton<NodeDataFactory>
{
    private Dictionary<int, NodeData> dict = new Dictionary<int, NodeData>();

    [JsonProperty] 
    private List<NodeData> list = new List<NodeData>();

    public void Merge(object o)
    {
        NodeDataFactory s = o as NodeDataFactory;
        this.list.AddRange(s.list);
    }

    [OnDeserialized]
    public void ProtoEndInit(StreamingContext context)
    {
        foreach (NodeData config in list)
        {
            config.AfterInit();
            this.dict.Add(config.ID, config);
        }

        this.list.Clear();

        this.AfterEndInit();
    }

    public NodeData Get(int id)
    {
        this.dict.TryGetValue(id, out NodeData NodeData);

        if (NodeData == null)
        {
            throw new Exception($"配置找不到，配置表名: {nameof(NodeData)}，配置id: {id}");
        }

        return NodeData;
    }

    public bool Contain(int id)
    {
        return this.dict.ContainsKey(id);
    }

    public IReadOnlyDictionary<int, NodeData> GetAll()
    {
        return this.dict;
    }

    public NodeData GetOne()
    {
        if (this.dict == null || this.dict.Count <= 0)
        {
            return null;
        }

        return list.GetRandomValue();
    }
}