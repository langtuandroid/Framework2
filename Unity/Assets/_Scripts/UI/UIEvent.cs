using Framework;

[Event(SceneType.Battle)]
public class NumericUIEvent: AEvent<NumericChange>
{
    protected override async ETTask Run(Scene scene, NumericChange a)
    {
        var unitInfo = UIComponent.Instance.Get<UI_UnitInfo>();
        if (unitInfo != null && unitInfo.ViewModel != null)
        {
            var unitInfoVm = unitInfo.ViewModel as UI_UnitInfoVM;
            unitInfoVm.Refresh();
        } 
        await ETTask.CompletedTask;
    }
}

//[NumericWatcher(SceneType.Battle, NumericType.MaxHp)]
//[NumericWatcher(SceneType.Battle, NumericType.Hp)]
public class UnitInfoNumericEvent : INumericWatcher
{
    public void Run(Unit unit, NumericChange args)
    {
        var unitInfo = UIComponent.Instance.Get<UI_UnitInfo>();
        if (unitInfo != null && unitInfo.ViewModel != null)
        {
            var unitInfoVm = unitInfo.ViewModel as UI_UnitInfoVM;
            unitInfoVm.Refresh();
        }
    }
}