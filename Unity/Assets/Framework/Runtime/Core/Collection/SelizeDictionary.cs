using System;
using System.Collections.Generic;
using System.ComponentModel;
using MongoDB.Bson.Serialization.Attributes;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Framework
{
    [Serializable]
    public class SerializeDictionary<TKey,TValue> : ISerializationCallbackReceiver , ISupportInitialize
    {
        [SerializeField]
        [HideInInspector]
        [BsonElement]
        private List<TKey> keys = new List<TKey>();
        
        [SerializeField]
        [HideInInspector]
        [BsonElement]
        private List<TValue> values = new List<TValue>();
        
        [ShowInInspector]
        [OnValueChanged("OnDicChanged")]
        [BsonIgnore]
        public Dictionary<TKey, TValue> Dic = new Dictionary<TKey, TValue>();

        private void OnDicChanged()
        {
            keys.Clear();
            values.Clear();
            foreach (var value in Dic)
            {
                keys.Add(value.Key);
                values.Add(value.Value);
            }
        }
        
        public void OnBeforeSerialize()
        {
        }

        public void OnAfterDeserialize()
        {
            Dic = new Dictionary<TKey, TValue>();
            for (int i = 0; i < keys.Count; i++)
            {
                Dic[keys[i]] = values[i];
            }
        }

        public void BeginInit()
        {
        }

        public void EndInit()
        {
            OnAfterDeserialize();
        }
    }
}