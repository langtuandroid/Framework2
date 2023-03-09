using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

using Framework;
using MongoDB.Bson;
using MongoDB.Bson.IO;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Conventions;
using UnityEditor;
using UnityEngine;
using Object = System.Object;

/// <summary>
/// Bson序列化反序列化辅助类
/// </summary>
public class MongoHelper
{
    /// <summary>
    /// 这里之所以使用Unity特性执行一次静态初始化，是因为Mongo.Bson要求在使用Bson之前（不管是序列化还是反序列化）必须准备就绪所有的序列化器，否则就会出问题
    /// 比如我在执行序列化的时候没有注册序列化器，在执行反序列化的时候才想起来注册序列化器，那么就会报错
    /// </summary>
#if UNITY_EDITOR
    [InitializeOnLoadMethod]
#endif
    public static void Init()
    {
    }

    static MongoHelper()
    {
        // 自动注册IgnoreExtraElements
        ConventionPack conventionPack = new ConventionPack { new IgnoreExtraElementsConvention(true) };
        ConventionRegistry.Register("IgnoreExtraElements", conventionPack, type => true);

        //技能配置反序列化相关(manually because these type cannot Automatically register)
        BsonClassMap.LookupClassMap(typeof(NP_BBValue_Int));
        BsonClassMap.LookupClassMap(typeof(NP_BBValue_Bool));
        BsonClassMap.LookupClassMap(typeof(NP_BBValue_Float));
        BsonClassMap.LookupClassMap(typeof(NP_BBValue_String));
        BsonClassMap.LookupClassMap(typeof(NP_BBValue_Vector3));
        BsonClassMap.LookupClassMap(typeof(NP_BBValue_Long));
        BsonClassMap.LookupClassMap(typeof(NP_BBValue_List_Long));

#if UNITY_EDITOR
        Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();
        var types = new List<Type>();
        foreach (var assembly in assemblies)
        {
            if (assembly.FullName.Contains("Assembly-CSharp"))
            {
                types.AddRange(assembly.GetTypes());
            }
        }
#else
            var types = Game.EventSystem.GetTypes();
#endif

        foreach (Type type in types)
        {
            if (!type.IsSubclassOf(typeof(Object)))
            {
                continue;
            }

            if (type.IsGenericType)
            {
                continue;
            }

            BsonClassMap.LookupClassMap(type);
        }

        RegisterAllSubClassForDeserialize(types);
    }

    /// <summary>
    /// 注册所有供反序列化的子类
    /// </summary>
    public static void RegisterAllSubClassForDeserialize(List<Type> allTypes)
    {
        List<Type> parenTypes = new List<Type>();
        List<Type> childrenTypes = new List<Type>();
        // registe by BsonDeserializerRegisterAttribute Automatically
        foreach (Type type in allTypes)
        {
            BsonDeserializerRegisterAttribute[] bsonDeserializerRegisterAttributes =
                type.GetCustomAttributes(typeof(BsonDeserializerRegisterAttribute), false) as
                    BsonDeserializerRegisterAttribute[];
            if (bsonDeserializerRegisterAttributes.Length > 0)
            {
                parenTypes.Add(type);
            }

            BsonDeserializerRegisterAttribute[] bsonDeserializerRegisterAttributes1 =
                type.GetCustomAttributes(typeof(BsonDeserializerRegisterAttribute), true) as
                    BsonDeserializerRegisterAttribute[];
            if (bsonDeserializerRegisterAttributes1.Length > 0)
            {
                childrenTypes.Add(type);
            }
        }

        foreach (Type type in childrenTypes)
        {
            foreach (var parentType in parenTypes)
            {
                if (parentType.IsAssignableFrom(type) && parentType != type)
                {
                    BsonClassMap.LookupClassMap(type);
                }
            }
        }
    }
}