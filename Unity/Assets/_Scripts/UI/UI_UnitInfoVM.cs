using Framework;

public class UI_UnitInfoVM : ViewModel
{
    private Unit unit;
    public ObservableProperty<int> MaxHp;
    public ObservableProperty<int> CurHp;
    public UI_UnitInfoVM(Unit unit)
    {
        this.unit = unit;
        MaxHp = AllocateObservable<ObservableProperty<int>>();
        CurHp = AllocateObservable<ObservableProperty<int>>();
        Refresh();
    }

    public void Refresh()
    {
        var numeric = unit.GetComponent<NumericComponent>();
        MaxHp.Value = (int)numeric.GetAsFloat(NumericType.MaxHp);
        CurHp.Value = (int)numeric.GetAsFloat(NumericType.Hp);
    }
}