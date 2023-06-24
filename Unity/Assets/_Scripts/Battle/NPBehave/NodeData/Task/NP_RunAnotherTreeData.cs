using System.Collections.Generic;
using Framework;
using NPBehave;
using Sirenix.OdinInspector;

public class NP_RunAnotherTreeData :  NP_NodeDataBase
{
    public override NodeType BelongNodeType => NodeType.Task;

    [LabelText("是否是技能")]
    public bool IsSkill = true;

    [LabelText("对应配置表id")]
    public int ConfigId;

    [LabelText("是否等待完成")]
    public bool IsWaitFinish = false;

    [LabelText("传递的数据")]
    public SerializeDictionary<string, IBlackboardOrValue> PassValue = new();

    private Task runTask;

    public override Task CreateTask(Unit unit, NP_RuntimeTree runtimeTree)
    {
        if (IsWaitFinish)
        {
            long behaveId = 0;
            if (IsSkill)
            {
                SkillCanvasData skillCanvasData = SkillCanvasDataFactory.Instance.Get(ConfigId);
                behaveId = skillCanvasData.NPBehaveId;
            }
            else
            {
                AICanvasConfig aiCanvasConfig = AICanvasConfigFactory.Instance.Get(ConfigId);
                behaveId = aiCanvasConfig.NPBehaveId;
            }

            runTask = new WaitUntil(() =>
            {
               var tree = NP_RuntimeTreeFactory.CreateNpRuntimeTree(unit, ConfigId);
               foreach (var item in PassValue.Dic)
               {
                   tree.GetBlackboard().Set(item.Key, item.Value.GetObjValue(runtimeTree.GetBlackboard()));
               }
            },
                () => unit.GetComponent<NP_RuntimeTreeManager>().GetTreeByRootID(behaveId)
                    .RootNode.CurrentState == Node.State.INACTIVE);
        }
        else
        {
            runTask = new Action(() =>
            {
               var tree = NP_RuntimeTreeFactory.CreateNpRuntimeTree(unit, ConfigId);
               foreach (var item in PassValue.Dic)
               {
                   tree.GetBlackboard().Set(item.Key, item.Value.GetObjValue(runtimeTree.GetBlackboard()));
               }
            });
        }

        return runTask;
    }

    public override Node NP_GetNode()
    {
        return runTask;
    }
}