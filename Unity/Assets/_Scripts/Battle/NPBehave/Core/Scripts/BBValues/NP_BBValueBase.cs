using System;
using Framework;
using NPBehave;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Framework
{
    public abstract class NP_BBValueBase<T> : ANP_BBValue, INP_BBValue<T>
    {
        [LabelText("@valueLabel")] public T Value;

#if UNITY_EDITOR
        private string valueLabel => $"{typeof(T).Name}";
#endif
        public T GetValue()
        {
            return Value;
        }

        public override void SetValueFrom(ANP_BBValue anpBbValue)
        {
            if (anpBbValue == null || !(anpBbValue is NP_BBValueBase<T>))
            {
                Log.Error($"{typeof(T)} 拷贝失败，anpBbValue为空或类型非法");
                return;
            }

            this.SetValueFrom((INP_BBValue<T>)anpBbValue);
        }

        protected virtual void SetValueFrom(INP_BBValue<T> bbValue)
        {
            if (bbValue == null || !(bbValue is NP_BBValueBase<T>))
            {
                Log.Error($"{typeof(T)} 拷贝失败，anpBbValue为空或类型非法");
                return;
            }

            this.SetValueFrom(bbValue.GetValue());
        }

        public virtual void SetValueFrom(T bbValue)
        {
            Value = bbValue;
        }
    }
}

public interface IBlackboardOrValue
{
    object GetObjValue(Blackboard blackboard);
}

[Serializable]
public abstract class ABlackboardOrValue<T> : IBlackboardOrValue
{
    [LabelText("@label")]
    [ShowIf("@UseBlackboard")]
    public NP_BlackBoardRelationData<T> BlackboardKey = new NP_BlackBoardRelationData<T>();

    [LabelText("@string.IsNullOrEmpty(this.label) ? \"值\" : label")]
    [ShowIf("@!UseBlackboard")]
    [SerializeField]
    public T OriginValue;

    [SerializeField]
    [LabelText("是否使用黑板")]
    public bool UseBlackboard = true;

    [SerializeField]
    [HideInInspector]
    private string label = "值";

    protected ABlackboardOrValue(string label)
    {
        if (!string.IsNullOrEmpty(this.label))
            this.label = label;
    }

    public object GetObjValue(Blackboard blackboard)
    {
        if (UseBlackboard)
        {
            return BlackboardKey.GetBlackBoardValue(blackboard);
        }

        return OriginValue;
    }
    
    public T GetValue(Blackboard blackboard)
    {
        if (UseBlackboard)
        {
            return BlackboardKey.GetBlackBoardValue(blackboard);
        }

        return OriginValue;
    }
} 
