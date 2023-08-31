using GraphProcessor;
using UnityEngine.UIElements;

[NodeCustomEditor(typeof(BuffNodeBase))]
public class BuffNodeView: BaseNodeView
{
    public override void Enable()
    {
        BuffNodeDataBase nodeDataBase = (this.nodeTarget as BuffNodeBase).GetBuffNodeData();
        TextField textField = new TextField();
        if (nodeDataBase is NormalBuffNodeData normalBuffNodeData)
        {
            textField.value = normalBuffNodeData.BuffDes;
            textField.RegisterValueChangedCallback((changedDes) => { normalBuffNodeData.BuffDes = changedDes.newValue; });
        }

        textField.style.marginTop = 4;
        textField.style.marginBottom = 4;

        controlsContainer.Add(textField);
    }
}