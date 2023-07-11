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
        [BoxGroup("Dictionary/添加item")]
        [PropertyOrder(1)]
        [InfoBox("有相同的key",InfoMessageType.Error, visibleIfMemberName: "HasSameKey")]
        [ShowInInspector]
        [HideIf(nameof(hasCustomAddFunc))]
        private TKey Key;
        
        [BoxGroup("Dictionary/添加item")]
        [PropertyOrder(2)]
        [ShowInInspector]
        [HideIf(nameof(hasCustomAddFunc))]
        private TValue Value;

        [BoxGroup("Dictionary/添加item")]
        [PropertyOrder(3)]
        [Button]
        private void Add()
        {
            if (CustomAddFunc != null)
            {
                CustomAddFunc(list);
            }
            else
            {
                if (HasSameKey) return;
                list.Add(new SerializeDicKeyValue<TKey, TValue>(Key, Value));
            }
        }

        private bool HasSameKey
        {
            get
            {
                foreach (var item in list)
                {
                    if (item.Key.Equals(Key)) return true;
                }

                return false;
            }
        }

        [BoxGroup("Dictionary", false)]
        [PropertyOrder(4)]
        [ShowInInspector]
        [SerializeField]
        [ListDrawerSettings(HideAddButton = true)]
        [OnValueChanged(nameof(OnListChanged))]
        private List<SerializeDicKeyValue<TKey, TValue>> list = new List<SerializeDicKeyValue<TKey, TValue>>();

        [BsonIgnore]
        [HideInInspector]
        public Dictionary<TKey, TValue> Dic = new Dictionary<TKey, TValue>();

        private bool hasCustomAddFunc => CustomAddFunc != null && CustomAddFunc.GetInvocationList().Length > 0;
        public event Action<List<SerializeDicKeyValue<TKey,TValue>>> CustomAddFunc;

        private void OnListChanged()
        {
            if(list == null) return;
            Dic ??= new Dictionary<TKey, TValue>();
            Dic.Clear();
            foreach (var value in list)
            {
                Dic[value.Key] = value.Value;
            }
        }

        public void OnBeforeSerialize()
        {
        }

        public void OnAfterDeserialize()
        {
            Dic = new Dictionary<TKey, TValue>();
            OnListChanged();
        }

        public void BeginInit()
        {
        }

        public void EndInit()
        {
            OnAfterDeserialize();
        }
    }

    [Serializable]
    public struct SerializeDicKeyValue<TKey, TValue>
    {
        [HorizontalGroup()]
        [LabelWidth(50)]
        public TKey Key;
        [HorizontalGroup()]
        [LabelWidth(60)]
        public TValue Value;

        public SerializeDicKeyValue(TKey key, TValue value)
        {
            Key = key;
            Value = value;
        }
    }
}