using System.Collections.Generic;
using Framework;
using NPBehave;
using Sirenix.OdinInspector;

public class NP_RunAnotherTreeData : NP_NodeDataBase
{
    public override NodeType BelongNodeType => NodeType.Tree;

    [LabelText("对应配置表id")] public int ConfigId;

    [LabelText("传递过去的数据")] public SerializeDictionary<IBlackboardOrValue, string> PassValue = new();

    [LabelText("从那边拿到的数据")] public SerializeDictionary<NP_BlackBoardKeyData, string> GetValue = new();

    private Task runTask;

    public override ExtraBehave CreateTree(Unit unit, NP_RuntimeTree runtimeTree)
    {
        ExtraBehave behave = default;
        Dictionary<string, ANP_BBValue> dictionary = new Dictionary<string, ANP_BBValue>();
        behave.Blackboard = dictionary;
        behave.Node = NP_RuntimeTreeFactory.LoadExtraTree(unit, runtimeTree, ConfigId, dictionary);
        behave.ConfigId = ConfigId;
        return behave;
    }

    public override Node NP_GetNode()
    {
        if (runTask == null)
        {
            runTask = new Action(EmptyFunc);
        }

        return runTask;
    }

    private void EmptyFunc()
    {
    }
}