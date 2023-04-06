using System;
using Framework;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Sirenix.OdinInspector;
using UnityEngine;

public class Test : MonoBehaviour
{
    public struct AA
    {
        [LabelText("此节点ID在数据仓库中的Key")]
        [ValueDropdown("GetIdKey")]
        [OnValueChanged("ApplayId")]
        [BsonIgnore]
        public string IdKey;

        [LabelText("Id")]
        [InfoBox("无法对其直接赋值，需要在CanvasDataManager中Ids中注册键值对，然后选择NodeIdKey的值")]
        [ReadOnly]
        public long Value;

        public override string ToString()
        {
            return Value.ToString();
        }
    }
    
    [Button]
    private void Start()
    {
        MongoHelper.RegisterStruct<AA>();
        //MongoHelper.RegisterStruct<VTD_Id>();
        Log(Vector3.one * 0.2f);
        Log(new AA(){Value = 123});
        Log(new VTD_Id(){Value = 333});
    }

    private void Log<T>(T val)
    {
        var json = val.ToJson();
        print(json);
        var v3 = SerializeHelper.Deserialize<T>(json);
        print(v3);
    }
    
    static bool IsSubclassOfRawGeneric(Type generic, Type toCheck) {
        while (toCheck != null && toCheck != typeof(object)) {
            var cur = toCheck.IsGenericType ? toCheck.GetGenericTypeDefinition() : toCheck;
            if (generic == cur) {
                return true;
            }
            toCheck = toCheck.BaseType;
        }
        return false;
    }
}