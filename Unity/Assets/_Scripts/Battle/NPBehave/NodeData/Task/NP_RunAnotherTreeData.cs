using System.Collections.Generic;
using Framework;
using NPBehave;
using Sirenix.OdinInspector;

public class NP_RunAnotherTreeData : NP_NodeDataBase
{
    public override NodeType BelongNodeType => NodeType.Task;

    [LabelText("对应配置表id")] public int ConfigId;

    [LabelText("是否等待完成")] public bool IsWaitFinish = false;

    [LabelText("传递过去的数据")] public SerializeDictionary<IBlackboardOrValue, string> PassValue = new();

    [LabelText("从那边拿到的数据")] public SerializeDictionary<NP_BlackBoardKeyData, string> GetValue = new();

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
                        tree.GetBlackboard().Set(item.Value, item.Key.GetObjValue(runtimeTree.GetBlackboard()));
                    }

                    foreach (var item in GetValue.Dic)
                    {
                        runtimeTree.GetBlackboard().Set(item.Key.BBKey, runtimeTree.GetBlackboard().Get(item.Value));
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
                    var bvalue =
                        NP_BBValueHelper.AutoCreateNPBBValueFromTValue(
                            item.Key.GetObjValue(runtimeTree.GetBlackboard()));
                    tree.GetBlackboard().Set(item.Value, bvalue);
                }

                foreach (var item in GetValue.Dic)
                {
                    runtimeTree.GetBlackboard().Set(item.Key.BBKey, runtimeTree.GetBlackboard().Get(item.Value));
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