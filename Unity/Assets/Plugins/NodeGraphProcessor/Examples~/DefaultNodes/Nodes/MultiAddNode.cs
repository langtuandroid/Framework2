using System;
using System.Collections.Generic;
using GraphProcessor;

[Serializable]
[NodeMenuItem("Custom/MultiAdd")]
public class MultiAddNode : BaseNode
{
    [Output] public float output;

    [Input] public IEnumerable<float> inputs;

    public override string name => "Add";

    protected override void Process()
    {
        output = 0;
        inputs = TryGetAllInputValues<float>(nameof(inputs));
        if (inputs == null)
            return;

        foreach (var input in inputs)
            output += input;
    }

    public override void TryGetOutputValue<T>(NodePort outputPort, NodePort inputPort, ref T value)
    {
        if (output is T finalValue) value = finalValue;
    }

    [CustomPortBehavior(nameof(inputs))]
    private IEnumerable<PortData> GetPortsForInputs(List<SerializableEdge> edges)
    {
        yield return new PortData { displayName = "In ", displayType = typeof(float), acceptMultipleEdges = true };
    }
}