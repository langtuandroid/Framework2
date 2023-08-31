using System.Collections.Generic;
using System.IO;
using System.Reflection;
using Framework;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;

/// <summary>
/// 其他行为树的黑板key
/// </summary>
[HideReferenceObjectPicker]
[HideLabel]
public class NP_OtherTreeBBKeyData
{
    [LabelWidth(40)]
    [LabelText("字典键")] [ValueDropdown("GetBBKeys")] [OnValueChanged("OnValueChanged")]
    public string BBKey;

#if UNITY_EDITOR
    private NP_DataSupportor dataSupportor;

    private int configId;

    private bool isSkill;

    public void Refresh(int configId, bool isSkill)
    {
        this.isSkill = isSkill;
        this.configId = configId;
    }
    
    private List<string> keys;

    private IEnumerable<string> GetBBKeys()
    {
        if (keys == null)
        {
            keys = new();
        }

        keys.Clear();

        if (dataSupportor == null || dataSupportor.ExcelId != configId)
        {
            string configPath = null;
            if (isSkill)
            {
                var data = SkillBehaveConfigFactory.Instance.Get(configId);
                configPath = data?.ConfigPath;
            }
            else
            {
                var data = BehaveConfigFactory.Instance.Get(configId);
                configPath = data?.ConfigPath;
            }

            if (!string.IsNullOrEmpty(configPath))
            {
                this.dataSupportor =
                    SerializeHelper.Deserialize<NP_DataSupportor>(File.ReadAllText(configPath));
            }
        }

        if (dataSupportor != null)
        {
            foreach (var item in dataSupportor.NP_BBValueManager)
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