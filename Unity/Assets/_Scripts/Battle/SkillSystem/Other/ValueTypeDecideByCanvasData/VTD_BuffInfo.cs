using Sirenix.OdinInspector;
using UnityEngine;

namespace Framework
{
    public class VTD_BuffInfo
    {
        // 如果为绝对层数，且此时Layers设置为10，意思是添加Buff到10层，否则就是添加10层Buff
        public bool LayersIsAbs;

        // 操作Buff层数
        public int Layers = 1;

        public void AutoAddBuff(BuffDataBase buffData, Unit theUnitFrom, Unit theUnitBelongTo)
        {
            int layers = Layers;
            if (LayersIsAbs)
            {
                IBuffSystem nextBuffSystemBase = BuffFactory.AcquireBuff(
                    buffData, theUnitFrom,
                    theUnitBelongTo);
                if (nextBuffSystemBase.CurrentOverlay < nextBuffSystemBase.BuffData.MaxOverlay &&
                    nextBuffSystemBase.CurrentOverlay < layers)
                {
                    layers -= nextBuffSystemBase.CurrentOverlay;
                }
                else
                {
                    return;
                }
            }

            for (int i = 0; i < layers; i++)
            {
                BuffFactory.AcquireBuff(buffData, theUnitFrom, theUnitBelongTo);
            }
        }
    }
}