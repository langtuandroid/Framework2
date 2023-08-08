﻿using System.Collections.Generic;
using Framework;

/// <summary>
/// 技能行为树管理器
/// </summary>
public class SkillCanvasManagerComponent : Entity
{
    /// <summary>
    /// 技能Id与其对应行为树映射,因为一个技能可能由多个行为树组成，所以value使用了List的形式
    /// </summary>
    public Dictionary<long, List<NP_RuntimeTree>> Skills = new();

    /// <summary>
    /// 技能Id与其等级映射
    /// </summary>
    private Dictionary<long, int> SkillLevels = new();

    public bool IsSkillRunning { get; private set; }

    public void SkillStart(long skillId)
    {
        IsSkillRunning = true;
    }

    public void SkillEnd(long skillId)
    {
        IsSkillRunning = false;
    }

    public void AddSkill(long skillId)
    {
        SkillLevels[skillId] = 0;
    }

    /// <summary>
    /// 给技能升级
    /// </summary>
    /// <param name="skillId"></param>
    /// <param name="count"></param>
    public void AddSkillLevel(long skillId, int count = 1)
    {
        if (SkillLevels.TryGetValue(skillId, out int level))
        {
            SkillLevels[skillId] = level + count;
        }
        else
        {
            Log.Error($"请求升级的SkillId:{skillId}不存在");
        }
    }

    /// <summary>
    /// 获取技能等级
    /// </summary>
    /// <param name="skillId"></param>
    public int GetSkillLevel(long skillId)
    {
        if (SkillLevels.TryGetValue(skillId, out int level))
        {
            return level;
        }
        else
        {
            Log.Error($"请求等级的SkillId:{skillId}不存在");
            return -1;
        }
    }
}