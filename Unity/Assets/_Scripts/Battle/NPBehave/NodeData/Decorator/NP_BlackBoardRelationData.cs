using System.Collections.Generic;
using System.Reflection;
using Framework;
using NPBehave;
using Sirenix.OdinInspector;
using UnityEngine;

[HideReferenceObjectPicker]
[HideLabel]
public class NP_BlackBoardKeyData
{
    [LabelText("字典键")] [ValueDropdown("GetBBKeys")] [OnValueChanged("OnValueChanged")]
    public string BBKey;
    
#if UNITY_EDITOR
    private List<string> keys;
    
    public object EditorValue
    {
        get
        {
            if (string.IsNullOrEmpty(BBKey)) return default;
            if (NP_BlackBoardDataManager.CurrentEditedNP_BlackBoardDataManager.BBValues.TryGetValue(BBKey,
                    out var value))
            {
                return value.GetType().GetField("Value", BindingFlags.Instance | BindingFlags.Public).GetValue(value);
            }

            return default;
        }
    } 
    
    private IEnumerable<string> GetBBKeys()
    {
        if (keys == null)
        {
            keys = new();
        }
        keys.Clear();
        
        if (NP_BlackBoardDataManager.CurrentEditedNP_BlackBoardDataManager != null)
        {
            foreach (var item in NP_BlackBoardDataManager.CurrentEditedNP_BlackBoardDataManager.BBValues)
            {
                keys.Add($"{item.Key}|({item.Value.NP_BBValueType.Name})");
            }
        }

        return keys;
    }

    private void OnValueChanged()
    {
        BBKey = BBKey.Split('|')[0];
    }
    #endif
}

/// <summary>
/// 与黑板节点相关的数据
/// </summary>
[BoxGroup("黑板数据配置"), GUIColor(0.961f, 0.902f, 0.788f, 1f)]
[HideLabel]
[HideReferenceObjectPicker]
public class NP_BlackBoardRelationData<T>
{
    [LabelWidth(60)]
    [LabelText("字典键")] [ValueDropdown("GetBBKeys")] [OnValueChanged("OnBBKeySelected")]
    public string BBKey;

    [LabelWidth(120)]
    [LabelText("把值写入黑板或对比")] public bool WriteOrCompareToBB;

    [ShowIf("WriteOrCompareToBB")] public ANP_BBValue NP_BBValue;


#if UNITY_EDITOR
    private List<string> keys;

    public T EditorValue
    {
        get
        {
            if (string.IsNullOrEmpty(BBKey)) return default;
            if (NP_BlackBoardDataManager.CurrentEditedNP_BlackBoardDataManager.BBValues.TryGetValue(BBKey,
                    out var value))
            {
                return (value as NP_BBValueBase<T>).Value;
            }

            return default;
        }
    }

    private IEnumerable<string> GetBBKeys()
    {
        if (keys == null)
        {
            keys = new();
        }
        keys.Clear();
        if (NP_BlackBoardDataManager.CurrentEditedNP_BlackBoardDataManager != null)
        {
            foreach (var item in NP_BlackBoardDataManager.CurrentEditedNP_BlackBoardDataManager.BBValues)
            {
                if (item.Value.NP_BBValueType == typeof(T) || item.Value.NP_BBValueType.IsSubclassOf(typeof(T)))
                {
                    keys.Add($"{item.Key}|({item.Value.NP_BBValueType.Name})");
                }
            }
        }

        return keys;
    }

    private void OnBBKeySelected()
    {
        BBKey = BBKey.Split('|')[0];
        if (NP_BlackBoardDataManager.CurrentEditedNP_BlackBoardDataManager != null)
        {
            foreach (var bbValues in NP_BlackBoardDataManager.CurrentEditedNP_BlackBoardDataManager.BBValues)
            {
                List<int> li = new List<int>();
                if (bbValues.Key == this.BBKey)
                {
                    NP_BBValue = bbValues.Value.DeepCopy();
                }
            }
        }
    }
#endif

    /// <summary>
    /// 获取目标黑板对应的此处的键的值
    /// </summary>
    /// <returns></returns>
    public T GetBlackBoardValue(Blackboard blackboard)
    {
        return blackboard.Get<T>(this.BBKey);
    }

    /// <summary>
    /// 获取配置的BB值
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public T GetTheBBDataValue()
    {
        return (this.NP_BBValue as NP_BBValueBase<T>).GetValue();
    }

    /// <summary>
    /// 自动根据预先设定的值设置值
    /// </summary>
    /// <param name="blackboard">要修改的黑板</param>
    public void SetBlackBoardValue(Blackboard blackboard)
    {
        NP_BBValueHelper.SetTargetBlackboardUseANP_BBValue(this.NP_BBValue, blackboard, BBKey);
    }

    /// <summary>
    /// 自动根据传来的值设置值
    /// </summary>
    /// <param name="blackboard">将要改变的黑板值</param>
    /// <param name="value">值</param>
    public void SetBlackBoardValue(Blackboard blackboard, T value)
    {
        blackboard.Set(this.BBKey, value);
    }

    /// <summary>
    /// 自动将一个黑板的对应key的value设置到另一个黑板上
    /// </summary>
    /// <param name="oriBB">数据源黑板</param>
    /// <param name="desBB">目标黑板</param>
    public void SetBBValueFromThisBBValue(Blackboard oriBB, Blackboard desBB)
    {
        NP_BBValueHelper.SetTargetBlackboardUseANP_BBValue(oriBB.Get(BBKey), desBB, BBKey);
    }
}