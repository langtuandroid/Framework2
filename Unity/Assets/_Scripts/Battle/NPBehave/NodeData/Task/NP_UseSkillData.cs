using Framework;
using NPBehave;
using Sirenix.OdinInspector;

public class NP_UseSkillData : NP_NodeDataBase
{
    public override NodeType BelongNodeType => NodeType.Task;

    private WaitUntil _waitUntil;
    private Action _action;

    [LabelText("使用的技能id")]
    public long SkillId;

    [LabelText("是否等待技能完成")]
    public bool IsWaitFinish;
    
    public override Task CreateTask(Unit unit, NP_RuntimeTree runtimeTree)
    {
        return _action;
    } 
    
    public override Node NP_GetNode()
    {
        return _action;
    }
}