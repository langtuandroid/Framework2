using Framework;
using Unity.Mathematics;

public class FollowTargetComponent : Entity , IUpdateSystem , IAwakeSystem
{
    private Unit targetUnit;
    private float endDis;
    private float3 lastMovePos = new float3(float.MinValue);
    
    public void Follow(long unitId, float endDis)
    {
        UnitComponent unitComponent = Domain.GetComponent<UnitComponent>();
        targetUnit = unitComponent.Get(unitId);
        this.endDis = endDis;
    }

    public void Update(float deltaTime)
    {
        if(targetUnit == null) return;
        // 如果是动态寻路，可能要在所有unit动的时候就重新移动
        if(targetUnit.Position.NearEqual(lastMovePos)) return;
        var selfUnit = GetParent<Unit>();
        var speed = parent.GetComponent<NumericComponent>().GetAsFloat(NumericType.Speed);
        var normal = math.normalize(targetUnit.Position - selfUnit.Position);
        // 如果是寻路，则需要求出最后一个点减去目标点的normal
        parent.GetComponent<MoveComponent>().MoveTo(targetUnit.Position - normal * endDis, speed);
        lastMovePos = targetUnit.Position;
    }

    public void Awake()
    {
        
    }
}