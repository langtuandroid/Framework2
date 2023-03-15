namespace Framework
{
    /// <summary>
    /// 监听Buff回调
    /// </summary>
    public class ListenBuffCallBackBuffSystem: ABuffSystemBase<ListenBuffCallBackBuffData>
    {
        public ListenBuffEvent_Normal ListenBuffEventNormal;
        
        public override void OnExecute(float currentTime)
        {
            if (GetBuffDataWithTType.HasOverlayerJudge)
            {
                ListenBuffEventNormal = ReferencePool.Allocate<ListenBuffEvent_CheckOverlay>();
                ListenBuffEventNormal.BuffInfoWillBeAdded = GetBuffDataWithTType.BuffInfoWillBeAdded;
                var listenOverLayer = ListenBuffEventNormal as ListenBuffEvent_CheckOverlay;
                listenOverLayer.TargetOverlay = GetBuffDataWithTType.TargetOverLayer;
            }
            else
            {
                ListenBuffEventNormal = ReferencePool.Allocate<ListenBuffEvent_Normal>();
                ListenBuffEventNormal.BuffInfoWillBeAdded = GetBuffDataWithTType.BuffInfoWillBeAdded;
            }
            this.GetBuffTarget().DomainScene().GetComponent<BattleEventSystemComponent>().RegisterEvent($"{this.GetBuffDataWithTType.EventId.Value}{this.TheUnitFrom.Id}", ListenBuffEventNormal);
        }

        public override void OnFinished(float currentTime)
        {
            this.GetBuffTarget().DomainScene().GetComponent<BattleEventSystemComponent>().UnRegisterEvent($"{this.GetBuffDataWithTType.EventId.Value}{this.TheUnitFrom.Id}", ListenBuffEventNormal);
        }
    }
}