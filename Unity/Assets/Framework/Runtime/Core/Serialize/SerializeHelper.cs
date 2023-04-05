using System.IO;
using System;
using MongoDB.Bson;
using MongoDB.Bson.IO;
using MongoDB.Bson.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Bson;
using JsonConvert = Newtonsoft.Json.JsonConvert;

namespace Framework
{
    public static class SerializeHelper
    {
        public static T Deserialize<T>(byte[] bytes, int index = 0, int count = -1)
        {
            return (T)Deserialize(typeof(T), bytes, index, count);
        }
        
        public static object Deserialize(Type type, byte[] bytes, int index = 0, int count = -1)
        {
            if (count == -1) count = bytes.Length;
            using MemoryStream memoryStream = new MemoryStream(bytes,index, count);
            return BsonSerializer.Deserialize(memoryStream, type);
        }

        public static byte[] Serialize(object message)
        {
            return message.ToBson();
        }

        public static void Serialize(object message, Stream stream)
        {
            using BsonBinaryWriter bsonWriter = new BsonBinaryWriter(stream, BsonBinaryWriterSettings.Defaults);
            BsonSerializationContext context = BsonSerializationContext.CreateRoot(bsonWriter);
            BsonSerializationArgs args = default;
            args.NominalType = typeof (object);
            IBsonSerializer serializer = BsonSerializer.LookupSerializer(args.NominalType);
            serializer.Serialize(context, args, message);
        }

        public static object Deserialize(Type type, Stream stream)
        {
            return BsonSerializer.Deserialize(stream, type);
        }

        public static object NTDeserialize(Type type, string json)
        {
            return JsonConvert.DeserializeObject(json, type);
        }

        public static object Deserialize(Type type, string json)
        {
            return BsonSerializer.Deserialize(json, type);
        }

        public static T Deserialize<T>(string json)
        {
            return (T)Deserialize(typeof(T), json);
        }

        public static T DeserializeNT<T>(string json)
        {
            return (T)JsonConvert.DeserializeObject<T>(json);
        }


        public static byte[] ToBson(this object obj)
        {
            return BsonExtensionMethods.ToBson(obj);
        }
        
        public static string ToJson(this object obj)
        {
            return BsonExtensionMethods.ToJson(obj);
        }

        /// <summary>
        /// 使用Newtonsoft.json，优点不需要增加额外属性就可以序列化字典
        /// 缺点是慢，特殊情况使用
        /// </summary>
        public static string ToNTJson(this object obj)
        {
            return JsonConvert.SerializeObject(obj, Formatting.Indented);
        }
    }
}