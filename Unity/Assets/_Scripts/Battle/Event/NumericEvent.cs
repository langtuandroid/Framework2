using Framework;
using UnityEngine.AI;

[NumericWatcher(SceneType.Battle, NumericType.Speed)]
public class SpeedReduceWatcher : INumericWatcher
{
    public void Run(Unit unit, NumericChange args)
    {
        var numeric = unit.GetComponent<NumericComponent>();
        var moveComponent = unit.GetComponent<MoveComponent>();
        if (moveComponent != null)
        {
            moveComponent.ChangeSpeed(numeric.GetFloatByInt(args.New));
        }

        var goCom = unit.GetComponent<GameObjectComponent>();
        if (goCom != null)
        {
            var go = goCom.GameObject;
            if (go != null)
            {
                var nav = go.GetComponent<NavMeshAgent>();
                if (nav != null)
                {
                    nav.speed = numeric.GetFloatByInt(args.New);
                }
            }
        }
    }
}