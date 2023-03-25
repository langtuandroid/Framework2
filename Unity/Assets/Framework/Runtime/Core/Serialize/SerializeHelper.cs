using System.IO;
using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Bson;

namespace Framework
{
    public static class SerializeHelper
    {
        public static T Deserialize<T>(byte[] bytes, int index = 0, int count = 0)
        {
            return (T)Deserialize(typeof(T), bytes, index, count);
        }
        
        public static object Deserialize(Type type, byte[] bytes, int index = 0, int count = 0)
        {
            if (count == -1) count = bytes.Length;
            MemoryStream ms = new MemoryStream(bytes, index, count);
            using BsonDataReader reader = new BsonDataReader(ms);
            JsonSerializer serializer = JsonSerializer.Create();
            return serializer.Deserialize(reader, type);
        }

        public static byte[] Serialize(object message)
        {
            return message.ToBson();
        }

        public static void Serialize(object message, Stream stream)
        {
            using BsonDataWriter writer = new BsonDataWriter(stream);
            JsonSerializer serializer = JsonSerializer.Create();
            serializer.Serialize(writer, message);
        }

        public static object Deserialize(Type type, Stream stream)
        {
            using BsonDataReader reader = new BsonDataReader(stream);
            JsonSerializer serializer = JsonSerializer.Create();
            return serializer.Deserialize(reader, type); 
        }


        public static object Deserialize(Type type, string json)
        {
            return JsonConvert.DeserializeObject(json, type);
        }

        public static T Deserialize<T>(string json)
        {
            return (T)Deserialize(typeof(T), json);
        }

        public static byte[] ToBson(this object obj)
        {
            using MemoryStream ms = new MemoryStream();
            using BsonDataWriter writer = new BsonDataWriter(ms);
            JsonSerializer serializer = JsonSerializer.Create();
            serializer.Serialize(writer, obj);
            return ms.GetBuffer();
        }

        public static string ToJson(this object obj)
        {
            return JsonConvert.SerializeObject(obj);
        }
    }
}