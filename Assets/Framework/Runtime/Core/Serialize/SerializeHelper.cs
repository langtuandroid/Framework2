using System.IO;
using System;
using MongoDB.Bson.Serialization;

namespace Framework
{
    public static class SerializeHelper
    {
        public static object Deserialize(Type type, byte[] bytes, int index, int count)
        {
            return null;
        }

        public static byte[] Serialize(object message)
        {
            return null;
        }

        public static void Serialize(object message, Stream stream)
        {
        }

        public static object Deserialize(Type type, Stream stream)
        {
            return null;
        }
    }
}