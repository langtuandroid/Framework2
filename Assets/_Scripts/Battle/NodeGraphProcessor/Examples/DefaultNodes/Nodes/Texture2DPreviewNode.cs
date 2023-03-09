using System;
using GraphProcessor;
using NodeGraphProcessor.Examples;
using UnityEngine;

[Serializable]
[NodeMenuItem("Custom/Texture2DPreview")]
public class Texture2DPreviewNode : LinearConditionalNode
{
    [Input(name = "Texture2D")] public Texture2D input;

    public override string name => "Texture2DPreview";

    protected override void Process()
    {
        TryGetInputValue(nameof(input), ref input);
    }
}