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
[PropertySpace(5, 5)]
public abstract class ABlackboardOrValue<T> : IBlackboardOrValue
{
    [ShowIf("@UseBlackboard")]
    public NP_BlackBoardRelationData<T> BlackboardKey = new NP_BlackBoardRelationData<T>();

    [LabelText("Value")]
    [ShowIf("@!UseBlackboard")]
    [SerializeField]
    public T OriginValue;

    [SerializeField]
    [LabelText("使用黑板")]
    public bool UseBlackboard = false;

    protected ABlackboardOrValue()
    {
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
    
#if UNITY_EDITOR
    public T GetEditorValue()
    {
        if (UseBlackboard && BlackboardKey != null)
        {
            return BlackboardKey.EditorValue;
        }

        return OriginValue;
    }

    [OnInspectorGUI]
    private void OnInspectorGUI()
    {
        var rect = UnityEditor.EditorGUILayout.GetControlRect(false, 1);
        UnityEditor.EditorGUI.DrawRect(rect, new Color(0.5f, 0.5f, 0.5f, 1));
    }

    private void AA()
    {
    }
#endif
} 
