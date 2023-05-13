using System.Collections.Generic;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.Options;
using Sirenix.OdinInspector;

/// <summary>
/// 技能配置数据载体
/// </summary>
[HideLabel]
public class NP_DataSupportor
{
    [BoxGroup("技能中的Buff数据结点")]
    [BsonDictionaryOptions(DictionaryRepresentation.ArrayOfArrays)]
    [BsonIgnoreIfDefault]
    public Dictionary<long, BuffNodeDataBase> BuffNodeDataDic = new Dictionary<long, BuffNodeDataBase>();

    [LabelText("此行为树Id，也是根节点Id")] public long NPBehaveTreeDataId;

    [LabelText("此行为树所属技能或行为的配置表Id")] public int ExcelId;

    [LabelText("单个行为树所有结点")]
    [BsonDictionaryOptions(DictionaryRepresentation.ArrayOfArrays)]
    public Dictionary<long, NP_NodeDataBase> NP_DataSupportorDic = new Dictionary<long, NP_NodeDataBase>();

    [LabelText("黑板数据")]
    [BsonDictionaryOptions(DictionaryRepresentation.ArrayOfArrays)]
    public Dictionary<string, ANP_BBValue> NP_BBValueManager = new Dictionary<string, ANP_BBValue>();
 
}