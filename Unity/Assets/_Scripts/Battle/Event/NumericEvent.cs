using Framework;

[NumericWatcher(SceneType.Battle, NumericType.Speed)]
public class SpeedReduceWatcher : INumericWatcher
{
    public void Run(Unit unit, NumericChange args)
    {
        var numeric = unit.GetComponent<NumericComponent>();
        unit.GetComponent<MoveComponent>()?.ChangeSpeed(numeric.GetFloatByInt(args.New));
    }
}