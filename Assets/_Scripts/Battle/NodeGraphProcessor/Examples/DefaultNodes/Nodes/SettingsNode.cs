using System;
using GraphProcessor;

public enum Setting
{
    S1,
    S2,
    S3
}

[Serializable]
[NodeMenuItem("Custom/SettingsNode")]
public class SettingsNode : BaseNode
{
    public Setting setting;

    [Input] public float input;

    [Output] public float output;

    public override string name => "SettingsNode";

    protected override void Process()
    {
        TryGetInputValue(nameof(input), ref input);
        output = input;
    }

    public override void TryGetOutputValue<T>(NodePort outputPort, NodePort inputPort, ref T value)
    {
        if (output is T finalValue) value = finalValue;
    }
}