using Framework;
using Framework.EventType;

[Event(SceneType.Battle)]
public class BattleChangeUnitPositionEvent : AEvent<ChangePosition>
{
    protected override async ETTask Run(Scene scene, ChangePosition a)
    {
        var gameObjectC = a.Unit.GetComponent<GameObjectComponent>();
        if (gameObjectC != null)
        {
            gameObjectC.GameObject.transform.position = a.Unit.Position;
        }

        await ETTask.CompletedTask;
    }
}