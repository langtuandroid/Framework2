using System.Collections.Generic;
using Sirenix.OdinInspector;

namespace Framework
{
    /// <summary>
    /// 刷新某个或某几个Buff的持续时间
    /// </summary>
    public class RefreshTargetBuffTimeBuffData : BuffDataBase
    {
        [BoxGroup("自定义项")] [LabelText("要刷新的BuffNodeId")]
        public List<string> TheBuffNodeIdToBeRefreshed = new List<string>();
    }
}