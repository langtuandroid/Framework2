using System.Collections.Generic;

namespace Framework
{
    public struct NumericChange
    {
        public Unit Unit;
        public int NumericType;
        public int Old;
        public int New;
    }

    public class NumericComponent : Entity, IAwakeSystem
    {
        public Dictionary<int, int> NumericDic = new Dictionary<int, int>();

        public int this[int numericType]
        {
            get { return this.GetByKey(numericType); }
            set { this.Insert(numericType, value); }
        }

        public float GetAsFloat(int numericType)
        {
            return (float)GetByKey(numericType) / 1000;
        }

        public int GetAsInt(int numericType)
        {
            return (int)GetByKey(numericType);
        }

        public void Set(int nt, float value, bool isPublicEvent = true)
        {
            Insert(nt, (int)(value * 1000), isPublicEvent);
        }

        public void Set(int nt, int value, bool isPublicEvent = true)
        {
            Insert(nt, value, isPublicEvent);
        }

        public void ApplyChange(int nt, float value, bool isPublicEvent = true)
        {
            Insert(nt, this[nt] + (int)(value * 1000), isPublicEvent);
        }

        public void ApplyChange(int nt, int value, bool isPublicEvent = true)
        {
            Insert(nt, this[nt] + value, isPublicEvent);
        }

        public void SetNoEvent(int numericType, int value)
        {
            Insert(numericType, value, false);
        }

        public void Insert(int numericType, int value, bool isPublicEvent = true)
        {
            int oldValue = GetByKey(numericType);
            if (oldValue == value)
            {
                return;
            }

            NumericDic[numericType] = value;

            if (numericType >= NumericType.Max)
            {
                Update(numericType, isPublicEvent);
                return;
            }

            if (isPublicEvent)
            {
                EventSystem.Instance.Publish(this.DomainScene(),
                    new NumericChange()
                        { Unit = GetParent<Unit>(), New = value, Old = oldValue, NumericType = numericType });
            }
        }

        public int GetByKey(int key)
        {
            int value = 0;
            NumericDic.TryGetValue(key, out value);
            return value;
        }

        private void Update(int numericType, bool isPublicEvent)
        {
            int final = (int)numericType / 10;
            int bas = final * 10 + 1;
            int add = final * 10 + 2;
            int pct = final * 10 + 3;
            int finalAdd = final * 10 + 4;
            int finalPct = final * 10 + 5;

            // 一个数值可能会多种情况影响，比如速度,加个buff可能增加速度绝对值100，也有些buff增加10%速度，所以一个值可以由5个值进行控制其最终结果
            // final = (((base + add) * (100 + pct) / 100) + finalAdd) * (100 + finalPct) / 100;
            int result = (int)(((GetByKey(bas) + GetByKey(add)) * (100 + GetAsFloat(pct)) / 100f +
                                GetByKey(finalAdd)) *
                (100 + GetAsFloat(finalPct)) / 100f);
            Insert(final, result, isPublicEvent);
        }


        public void Awake()
        {
            Set(NumericType.MaxHp, 10f);
            Set(NumericType.Hp, 10f);
            Set(NumericType.Speed, 1f);
        }
    }
}