using System.Collections.Generic;
using Framework;

namespace ET
{
    public class UnitAttributesDataRepositoryComponent : Entity
    {
        public Dictionary<long, UnitAttributesDataSupportor> AllUnitAttributesBaseDataDic =
            new Dictionary<long, UnitAttributesDataSupportor>();
    }
}