using System;
using System.Collections.Generic;
using GraphProcessor;
using UnityEngine;

[Serializable]
[NodeMenuItem("Custom/PortData")]
public class CustomPortData : BaseNode
{
    private static PortData[] portDatas =
    {
        new() { displayName = "0", displayType = typeof(float), identifier = "0" },
        new() { displayName = "1", displayType = typeof(int), identifier = "1" },
        new() { displayName = "2", displayType = typeof(GameObject), identifier = "2" },
        new() { displayName = "3", displayType = typeof(Texture2D), identifier = "3" },
        new() { displayName = "4", displayType = typeof(float), identifier = "4" },
    };

    [Output] public float output;

    [Input(name = "In Values", allowMultiple = true)]
    public IEnumerable<object> inputs;

    public override string name => "Port Data";

    protected override void Process()
    {
        output = 0;

        inputs = TryGetAllInputValues<object>(nameof(inputs));
        foreach (var inputPort in inputPorts)
        {
            foreach (var connectedEdge in inputPort.GetEdges())
            {
                object t = default;
                connectedEdge.outputPort.GetOutputValue(inputPort, ref t);
                Debug.Log(t);
            }
        }

        if (inputs == null)
            return;

        foreach (float input in inputs)
            output += input;
    }

    public override void TryGetOutputValue<T>(NodePort outputPort, NodePort inputPort, ref T value)
    {
        if (output is T finalValue) value = finalValue;
    }

    [CustomPortBehavior(nameof(inputs))]
    private IEnumerable<PortData> GetPortsForInputs(List<SerializableEdge> edges)
    {
        var pd = new PortData();

        foreach (var portData in portDatas) yield return portData;
    }
}