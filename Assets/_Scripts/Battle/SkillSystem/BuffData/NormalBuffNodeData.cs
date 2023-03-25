using Newtonsoft.Json;
using Sirenix.OdinInspector;

[Title("Buff节点数据块", TitleAlignment = TitleAlignments.Centered)]
[HideReferenceObjectPicker]
[HideLabel]
public class NormalBuffNodeData : BuffNodeDataBase
{
    [LabelText("Buff描述")] [JsonIgnore] public string BuffDes;

    /// <summary>
    /// Buff数据
    /// </summary>
    public BuffDataBase BuffData;
}