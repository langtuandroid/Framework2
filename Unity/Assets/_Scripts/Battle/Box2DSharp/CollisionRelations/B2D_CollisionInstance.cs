using System.Collections.Generic;
using Newtonsoft.Json;
using Sirenix.OdinInspector;


/// <summary>
/// 碰撞实例
/// </summary>
public class B2D_CollisionInstance
{
    [LabelText("此结点标识")] [JsonIgnore] public string Flag;

    [LabelText("此结点ID")] public long nodeDataId;

    [LabelText("此结点所使用的碰撞体ID")] public List<long> collisionId;

    [LabelText("是否跟随Unit进行同步")] public bool FollowUnit;

    [InfoBox("（请前往Canvas处点击“自动配置所有Node数据”）")] [LabelText("此结点归属Group")] [JsonIgnore] [DisableInEditorMode]
    public string BelongGroup;

    [LabelText("与此结点有碰撞关系的结点ID")] [JsonIgnore] [DisableInEditorMode]
    public List<long> CollisionRelations = new List<long>();
}