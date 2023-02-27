using System.IO;
using System;
using MongoDB.Bson.Serialization;

namespace Framework
{
    public static class SerializeHelper
    {
        public static object Deserialize(Type type, byte[] bytes, int index, int count)
        {
            return MongoHelper.Deserialize(type, bytes, index, count);
        }

        public static byte[] Serialize(object message)
        {
            return MongoHelper.Serialize(message);
        }

        public static void Serialize(object message, Stream stream)
        {
            MongoHelper.Serialize(message, stream);
        }

        public static object Deserialize(Type type, Stream stream)
        {
            return MongoHelper.Deserialize(type, stream);
        }
    }
}