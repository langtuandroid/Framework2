using System.Collections.Generic;
using Framework;


public class SkillManagerComponent : Entity, IAwakeSystem
{
    /// <summary>
    /// 技能Id与其等级映射
    /// </summary>
    private Dictionary<int, int> SkillLevels = new();

    public bool IsSkillRunning { get; private set; }

    public void SkillStart(int skillId)
    {
        IsSkillRunning = true;
    }

    public void SkillEnd(int skillId)
    {
        IsSkillRunning = false;
    }

    public void AddSkill(int skillId, int level = 0)
    {
        SkillLevels[skillId] = level;
    }

    /// <summary>
    /// 给技能升级
    /// </summary>
    /// <param name="skillId"></param>
    /// <param name="count"></param>
    public void AddSkillLevel(int skillId, int count = 1)
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
    public int GetSkillLevel(int skillId)
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

    public void Awake()
    {
    }
}