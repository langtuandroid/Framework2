using System;
using System.Collections.Generic;
using System.IO;
using Framework;
using Sirenix.OdinInspector;

[Serializable]
public struct VTD_ColliderId
{
    [ReadOnly]
    public long Id;

#if UNITY_EDITOR
    [ValueDropdown("GetEventId")] 
    [OnValueChanged(nameof(OnNameChanged))]
    [ShowInInspector]
    private string Name;
    
    private static ColliderNameAndIdInflectSupporter supporter;

    private IEnumerable<string> GetEventId()
    {
        if (supporter == null)
        {
            var mfile0 =
                File.ReadAllText($"{B2D_BattleColliderExportPathDefine.ColliderNameAndIdInflectSavePath}");
            if (mfile0.Length > 0)
                supporter =
                    SerializeHelper.Deserialize<ColliderNameAndIdInflectSupporter>(mfile0);

            if (string.IsNullOrEmpty(Name))
            {
                foreach (var item in supporter.colliderNameAndIdInflectDic)
                {
                    if (item.Value == Id)
                    {
                        Name = item.Key;
                        break;
                    }
                }
            }
        }

        return supporter.colliderNameAndIdInflectDic.Keys;
    }

    private void OnNameChanged()
    {
        if(string.IsNullOrEmpty(Name)) return;
        Id = supporter.colliderNameAndIdInflectDic[Name];
    }
#endif
}