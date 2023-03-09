using System.Collections.Generic;

using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.Options;
using Sirenix.OdinInspector;

[HideLabel]
public class NP_DataSupportorBase
{
    [LabelText("此行为树Id，也是根节点Id")] public long NPBehaveTreeDataId;

    [LabelText("单个行为树所有结点")] [BsonDictionaryOptions(DictionaryRepresentation.ArrayOfArrays)]
    public Dictionary<long, NP_NodeDataBase> NP_DataSupportorDic = new Dictionary<long, NP_NodeDataBase>();

    [LabelText("黑板数据")]
    public Dictionary<string, ANP_BBValue> NP_BBValueManager = new Dictionary<string, ANP_BBValue>();
}