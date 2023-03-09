using System.Collections.Generic;
using Sirenix.OdinInspector;

namespace Framework
{
    public class ChangeMaterialBuffData: BuffDataBase
    {
        /// <summary>
        /// 将要被添加的材质名
        /// </summary>
        [BoxGroup("自定义项")]
        [LabelText("将要被添加的材质名")]
        public List<string> TheMaterialNameWillBeAdded = new List<string>();
    }
}