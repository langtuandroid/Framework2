using Sirenix.OdinInspector;
using UnityEngine;

namespace Framework
{
    public class VTD_BuffInfo
    {
        [BoxGroup("Buff节点Id信息")] [HideLabel] public VTD_Id BuffNodeId;

        [LabelText("层数是否被黑板值决定")] public bool LayersDetermindByBBValue = false;

        [Tooltip("如果为绝对层数，且此时Layers设置为10，意思是添加Buff到10层，否则就是添加10层Buff")] [LabelText("层数是否为绝对层数")]
        public bool LayersIsAbs;

        [HideIf("LayersDetermindByBBValue")] [LabelText("操作Buff层数")]
        public int Layers = 1;

        [ShowIf("LayersDetermindByBBValue")] [LabelText("操作Buff层数")]
        public NP_BlackBoardRelationData LayersThatDetermindByBBValue;

        public void AutoAddBuff(long dataId, long buffNodeId, Unit theUnitFrom, Unit theUnitBelongTo,
            NP_RuntimeTree theSkillCanvasBelongTo)
        {
            int layers = 0;
            if (LayersDetermindByBBValue)
            {
                layers = theSkillCanvasBelongTo.GetBlackboard().Get<int>(LayersThatDetermindByBBValue.BBKey);
            }
            else
            {
                Layers = layers;
            }

            if (LayersIsAbs)
            {
                IBuffSystem nextBuffSystemBase = BuffFactory.AcquireBuff(dataId, buffNodeId, theUnitFrom,
                    theUnitBelongTo,
                    theSkillCanvasBelongTo);
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
                BuffFactory.AcquireBuff(dataId, buffNodeId, theUnitFrom, theUnitBelongTo,
                    theSkillCanvasBelongTo);
            }
        }

        public void AutoAddBuff(NP_DataSupportor npDataSupportor, long buffNodeId, Unit theUnitFrom,
            Unit theUnitBelongTo,
            NP_RuntimeTree theSkillCanvasBelongTo)
        {
            int layers = 0;
            if (LayersDetermindByBBValue)
            {
                layers = theSkillCanvasBelongTo.GetBlackboard().Get<int>(LayersThatDetermindByBBValue.BBKey);
            }
            else
            {
                layers = Layers;
            }

            if (LayersIsAbs)
            {
                IBuffSystem nextBuffSystemBase = BuffFactory.AcquireBuff(npDataSupportor, buffNodeId, theUnitFrom,
                    theUnitBelongTo,
                    theSkillCanvasBelongTo);
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
                BuffFactory.AcquireBuff(npDataSupportor, buffNodeId, theUnitFrom, theUnitBelongTo,
                    theSkillCanvasBelongTo);
            }
        }
    }
}