using Framework;
using UnityEngine;

public class AMoveComponent : Entity, IUpdate
{
}

public class AMoveUpdateSystem : UpdateSystem<AMoveComponent>
{
    protected override void Update(AMoveComponent self, float deltaTime)
    {
        var unit = self.GetParent<Unit>();
        unit.Position += unit.Forward * (deltaTime);
        unit.GetComponent<GameObjectComponent>().GameObject.transform.position = unit.Position;
        Log.Msg(deltaTime);
    }
}