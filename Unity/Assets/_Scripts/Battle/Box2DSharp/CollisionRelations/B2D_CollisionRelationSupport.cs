//------------------------------------------------------------
// Author: 烟雨迷离半世殇
// Mail: 1778139321@qq.com
// Data: 2019年7月25日 16:09:42
//------------------------------------------------------------

using System.Collections.Generic;
using Sirenix.OdinInspector;

namespace ET
{
    public class B2D_CollisionsRelationSupport
    {
        [LabelText("此数据载体ID")]
        public long SupportId;

        public Dictionary<long, B2D_CollisionInstance> B2D_CollisionsRelationDic = new Dictionary<long, B2D_CollisionInstance>();
    }
}