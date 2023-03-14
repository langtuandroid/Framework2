using System.Collections.Generic;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.Options;

namespace Framework
{
    public static class NumericComponentSystem
    {
        public static float GetAsFloat(this NumericComponent self, int numericType)
        {
            return (float)self.GetByKey(numericType) / 1000;
        }

        public static int GetAsInt(this NumericComponent self, int numericType)
        {
            return (int)self.GetByKey(numericType);
        }

        public static void Set(this NumericComponent self, int nt, float value, bool isPublicEvent = true)
        {
            self.Insert(nt, (int)(value * 1000), isPublicEvent);
        }

        public static void Set(this NumericComponent self, int nt, int value, bool isPublicEvent = true)
        {
            self.Insert(nt, value, isPublicEvent);
        }

        public static void ApplyChange(this NumericComponent self, int nt, float value, bool isPublicEvent = true)
        {
            self.Insert(nt, self[nt] + (int)(value * 1000), isPublicEvent);
        }

        public static void ApplyChange(this NumericComponent self, int nt, int value, bool isPublicEvent = true)
        {
            self.Insert(nt, self[nt] + value, isPublicEvent);
        }

        public static void SetNoEvent(this NumericComponent self, int numericType, int value)
        {
            self.Insert(numericType, value, false);
        }

        public static void Insert(this NumericComponent self, int numericType, int value, bool isPublicEvent = true)
        {
            int oldValue = self.GetByKey(numericType);
            if (oldValue == value)
            {
                return;
            }

            self.NumericDic[numericType] = value;

            if (numericType >= NumericType.Max)
            {
                self.Update(numericType, isPublicEvent);
                return;
            }

            if (isPublicEvent)
            {
                EventSystem.Instance.Publish(self.DomainScene(),
                    new EventType.NumbericChange()
                        { Unit = self.GetParent<Unit>(), New = value, Old = oldValue, NumericType = numericType });
            }
        }

        public static int GetByKey(this NumericComponent self, int key)
        {
            int value = 0;
            self.NumericDic.TryGetValue(key, out value);
            return value;
        }

        public static void Update(this NumericComponent self, int numericType, bool isPublicEvent)
        {
            int final = (int)numericType / 10;
            int bas = final * 10 + 1;
            int add = final * 10 + 2;
            int pct = final * 10 + 3;
            int finalAdd = final * 10 + 4;
            int finalPct = final * 10 + 5;

            // 一个数值可能会多种情况影响，比如速度,加个buff可能增加速度绝对值100，也有些buff增加10%速度，所以一个值可以由5个值进行控制其最终结果
            // final = (((base + add) * (100 + pct) / 100) + finalAdd) * (100 + finalPct) / 100;
            int result = (int)(((self.GetByKey(bas) + self.GetByKey(add)) * (100 + self.GetAsFloat(pct)) / 100f +
                                  self.GetByKey(finalAdd)) *
                (100 + self.GetAsFloat(finalPct)) / 100f);
            self.Insert(final, result, isPublicEvent);
        }
    }

    namespace EventType
    {
        public struct NumbericChange
        {
            public Unit Unit;
            public int NumericType;
            public int Old;
            public int New;
        }
    }

    public class NumericComponent : Entity, IAwake, ITransfer
    {
        [BsonDictionaryOptions(DictionaryRepresentation.ArrayOfArrays)]
        public Dictionary<int, int> NumericDic = new Dictionary<int, int>();

        public int this[int numericType]
        {
            get { return this.GetByKey(numericType); }
            set { this.Insert(numericType, value); }
        }
    }
}