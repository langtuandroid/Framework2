using System.Collections.Generic;
using Framework;
using NPBehave;
using Sirenix.OdinInspector;

public class NP_RunAnotherTreeData :  NP_NodeDataBase
{
    public override NodeType BelongNodeType => NodeType.Task;

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
            long behaveId = BehaveConfigFactory.Instance.Get(ConfigId).NPBehaveId;
            runTask = new WaitUntil(() =>
            {
               var tree = NP_RuntimeTreeFactory.CreateNpRuntimeTree(unit, ConfigId);
               foreach (var item in PassValue.Dic)
               {
                   tree.GetBlackboard().Set(item.Key, item.Value.GetObjValue(runtimeTree.GetBlackboard()));
               }
               tree.Start();
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
                   var bvalue = NP_BBValueHelper.AutoCreateNPBBValueFromTValue(item.Value.GetObjValue(runtimeTree.GetBlackboard()));
                   tree.GetBlackboard().Set(item.Key, bvalue);
               }
               tree.Start();
            });
        }

        return runTask;
    }

    public override Node NP_GetNode()
    {
        return runTask;
    }
}