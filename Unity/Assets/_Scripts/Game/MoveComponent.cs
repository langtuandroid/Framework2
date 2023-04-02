using Framework;

public class AMoveComponent : Entity, IUpdateSystem
{
    public void Update(float deltaTime)
    {
        var unit = GetParent<Unit>();
        unit.Position += unit.Forward * (deltaTime);
        unit.GetComponent<GameObjectComponent>().GameObject.transform.position = unit.Position;
        Log.Msg(deltaTime);
    }
}