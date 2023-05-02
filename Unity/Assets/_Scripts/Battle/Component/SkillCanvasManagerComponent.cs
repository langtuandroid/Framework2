using System.Collections.Generic;
using Framework;

public class SkillCanvasManagerComponent : Entity, IDestroySystem
{
    /// <summary>
    /// 技能Id与其对应行为树映射,因为一个技能可能由多个行为树组成，所以value使用了List的形式
    /// </summary>
    public Dictionary<long, List<NP_RuntimeTree>> Skills = new Dictionary<long, List<NP_RuntimeTree>>();

    /// <summary>
    /// 技能Id与其等级映射
    /// </summary>
    public Dictionary<long, int> SkillLevels = new Dictionary<long, int>();


    /// <summary>
    /// 添加技能Canvas
    /// </summary>
    /// <param name="skillId">归属技能Id，不是技能图本身的id</param>
    /// <param name="npRuntimeTree">对应行为树</param>
    public void AddSkillCanvas(long skillId,
        NP_RuntimeTree npRuntimeTree)
    {
        if (npRuntimeTree == null)
        {
            Log.Error($"试图添加的id为{skillId}的技能图为空");
            return;
        }

        if (Skills.TryGetValue(skillId, out var skillContent))
        {
            skillContent.Add(npRuntimeTree);
        }
        else
        {
            Skills.Add(skillId, new List<NP_RuntimeTree>() { npRuntimeTree });
        }

        //TODO 这里默认一级了
        if (!SkillLevels.ContainsKey(skillId))
        {
            SkillLevels.Add(skillId, 1);
        }
    }


    /// <summary>
    /// 获取所有技能行为树
    /// </summary>
    /// <param name="skillId">技能标识</param>
    public Dictionary<long, List<NP_RuntimeTree>> GetAllSkillCanvas()
    {
        return Skills;
    }

    /// <summary>
    /// 获取行为树
    /// </summary>
    /// <param name="skillId">技能标识</param>
    public List<NP_RuntimeTree> GetSkillCanvas(long skillId)
    {
        if (Skills.TryGetValue(skillId, out var skillContent))
        {
            return skillContent;
        }
        else
        {
            Log.Error($"请求的ID标识为{skillId}的技能图不存在");
            return null;
        }
    }

    /// <summary>
    /// 移除行为树(移除一个技能标识对应所有技能图)
    /// </summary>
    /// <param name="skillId">技能标识</param>
    public void RemoveSkillCanvas(long skillId)
    {
        foreach (var skillCanvas in GetSkillCanvas(skillId))
        {
            RemoveSkillCanvas(skillId, skillCanvas);
        }

        if (SkillLevels.ContainsKey(skillId))
        {
            SkillLevels.Remove(skillId);
        }
    }

    /// <summary>
    /// 移除行为树(移除一个技能标识对应的目标技能图)
    /// </summary>
    /// <param name="skillId">技能标识</param>
    /// <param name="npRuntimeTree">对应行为树</param>
    public void RemoveSkillCanvas(long skillId,
        NP_RuntimeTree npRuntimeTree)
    {
        List<NP_RuntimeTree> targetSkillContent = GetSkillCanvas(skillId);
        if (targetSkillContent != null)
        {
            for (int i = targetSkillContent.Count - 1; i >= 0; i--)
            {
                if (targetSkillContent[i] == npRuntimeTree)
                {
                    GetParent<Unit>().GetComponent<NP_RuntimeTreeManager>().RemoveTree(npRuntimeTree.Id);
                    targetSkillContent.RemoveAt(i);
                }
            }
        }
    }

    /// <summary>
    /// 给技能升级
    /// </summary>
    /// <param name="skillId"></param>
    /// <param name="count"></param>
    public void AddSkillLevel(long skillId, int count = 1)
    {
        if (SkillLevels.TryGetValue(skillId, out var level))
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
        if (SkillLevels.TryGetValue(skillId, out var level))
        {
            return level;
        }
        else
        {
            Log.Error($"请求等级的SkillId:{skillId}不存在");
            return -1;
        }
    }


    public void OnDestroy()
    {
        foreach (var skillContent in Skills)
        {
            foreach (var skillCanvas in skillContent.Value)
            {
                skillCanvas.Dispose();
            }
        }

        Skills.Clear();
    }
}