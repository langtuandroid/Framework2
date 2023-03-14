using Framework;

namespace ET
{
    /// <summary>
    /// 英雄数据组件，负责管理英雄数据
    /// </summary>
    public class UnitAttributesDataComponent: Entity
    {
        public UnitAttributesNodeDataBase UnitAttributesNodeDataBase;

        public NumericComponent NumericComponent;

        public T GetAttributeDataAs<T>() where T : UnitAttributesNodeDataBase
        {
            return UnitAttributesNodeDataBase as T;
        }
        
        public float GetAttribute(int numericType)
        {
            return NumericComponent[numericType];
        }

        public override void Dispose()
        {
            if (this.IsDisposed)
            {
                return;
            }

            base.Dispose();
            this.UnitAttributesNodeDataBase = null;
            NumericComponent = null;
        }
    }
}