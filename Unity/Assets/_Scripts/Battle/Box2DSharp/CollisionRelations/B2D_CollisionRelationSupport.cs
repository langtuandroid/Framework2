using System.Collections.Generic;
using Sirenix.OdinInspector;


public class B2D_CollisionsRelationSupport
{
    [LabelText("此数据载体ID")] public long SupportId;

    public Dictionary<long, B2D_CollisionInstance> B2D_CollisionsRelationDic =
        new Dictionary<long, B2D_CollisionInstance>();
}