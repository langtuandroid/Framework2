using System;
using GraphProcessor;
using NPBehave;

[NodeMenuItem("NPBehave行为树/Task/WaitUntilStopped", typeof (NPBehaveGraph))]
[NodeMenuItem("NPBehave行为树/Task/WaitUntilStopped", typeof (SkillGraph))]
public class NP_WaitUntilStoppedNode: NP_TaskNodeBase
{
    public override string name => "一直等待，直到Stopped";

    public NP_WaitUntilStoppedData NpWaitUntilStoppedData = new NP_WaitUntilStoppedData { NodeDes = "阻止轮询，提高效率" };

    public override NP_NodeDataBase NP_GetNodeData()
    {
        return NpWaitUntilStoppedData;
    }

    public override string CreateNodeName => nameof(WaitUntilStopped);

    public override void Debug_SetNodeData(object data)
    {
        NpWaitUntilStoppedData = (NP_WaitUntilStoppedData)data;
    }
}