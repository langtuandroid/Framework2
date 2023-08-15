using System.Collections.Generic;
using Framework;
using Sirenix.OdinInspector;

#if UNITY_EDITOR
/// <summary>
/// 单个Canvas数据管理器，包括但不限于黑板数据，常用字符串数据等
/// </summary>
public class NP_BlackBoardDataManager
{
    [InfoBox("这是这个NPBehaveCanvas的所有黑板数据\n键为string，值为NP_BBValue子类\n如果要添加新的黑板数据类型，请参照BBValues文件夹下的定义")]
    [Title("黑板数据", TitleAlignment = TitleAlignments.Centered)]
    [LabelText("黑板内容")]
    [BoxGroup]
    [OnValueChanged(nameof(OnBBValueChanged))]
    [DictionaryDrawerSettings(KeyLabel = "键(string)", ValueLabel = "值(NP_BBValue)",
        DisplayMode = DictionaryDisplayOptions.OneLine)]
    public Dictionary<string, ANP_BBValue> BBValues = new Dictionary<string, ANP_BBValue>();

    [InfoBox("这是这个NPBehaveCanvas的所有事件数据")]
    [Title("事件名", TitleAlignment = TitleAlignments.Centered)]
    [LabelText("事件内容")]
    [BoxGroup]
    public List<string> EventValues = new List<string>();

    [InfoBox("这是这个NPBehaveCanvas的所有ID相关的映射数据，key为ID描述，value为Id的值")]
    [Title("Id描述映射", TitleAlignment = TitleAlignments.Centered)]
    [LabelText("Id描述与值")]
    [BoxGroup]
    [DictionaryDrawerSettings(KeyLabel = "键(string)", ValueLabel = "值(long)",
        DisplayMode = DictionaryDisplayOptions.CollapsedFoldout)]
    public Dictionary<string, long> Ids = new Dictionary<string, long>();

    private void OnBBValueChanged()
    {
        using RecyclableList<string> oldNameKeys = RecyclableList<string>.Create();
        foreach (var valuesKey in BBValues.Keys)
        {
            if (!valuesKey.EndsWith($"[{BehaveId}]"))
            {
                oldNameKeys.Add(valuesKey);
            }
        }

        foreach (var oldNameKey in oldNameKeys)
        {
            var newKey = $"{oldNameKey} [{BehaveId}]";
            BBValues[newKey] = BBValues[oldNameKey];
            BBValues.Remove(oldNameKey);
        }
    }

    // debug时用来显示黑板信息的
    public void RefreshFromDataSupporter(NP_DataSupportor dataSupportor)
    {
        BBValues.Clear();
        BBValues.AddRange(dataSupportor.NP_BBValueManager);
    }

    /// <summary>
    /// 由GraphEditor点击Blackboard按钮时传递进来
    /// </summary>
    public static NP_BlackBoardDataManager CurrentEditedNP_BlackBoardDataManager;

    public static int BehaveId;
}
#endif