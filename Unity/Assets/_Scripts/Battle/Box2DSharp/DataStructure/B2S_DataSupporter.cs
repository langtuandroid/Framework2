using System.Collections.Generic;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.Options;

namespace ET
{
    /// <summary>
    /// 名称与id映射字典
    /// </summary>
    public class ColliderNameAndIdInflectSupporter
    {
        public Dictionary<string, long> colliderNameAndIdInflectDic = new Dictionary<string, long>();
    }

    /// <summary>
    /// 碰撞体数据字典
    /// </summary>
    public class ColliderDataSupporter
    {
        [BsonDictionaryOptions(DictionaryRepresentation.ArrayOfArrays)]
        public Dictionary<long, B2S_ColliderDataStructureBase> colliderDataDic = new Dictionary<long, B2S_ColliderDataStructureBase>();
    }
}