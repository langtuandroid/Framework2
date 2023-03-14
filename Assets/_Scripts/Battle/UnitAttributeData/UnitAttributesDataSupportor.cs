using System.Collections.Generic;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.Options;
using Sirenix.OdinInspector;

namespace ET
{
    public class UnitAttributesDataSupportor
    {
        [LabelText("此数据载体ID")]
        public int SupportId;
        
        [BsonDictionaryOptions(DictionaryRepresentation.ArrayOfArrays)]
        public Dictionary<long, UnitAttributesNodeDataBase> UnitAttributesDataSupportorDic = new Dictionary<long, UnitAttributesNodeDataBase>();
    }
}