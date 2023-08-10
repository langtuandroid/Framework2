using Framework;
using Unity.Mathematics;
using UnityEngine;

public class KeyboardCtrlComponent : Entity , IAwakeSystem , IUpdateSystem
{
    private MoveComponent moveComponent;
    public void Awake()
    {
        moveComponent = parent.GetComponent<MoveComponent>();
    }


    public void Update(float deltaTime)
    {
        var h = Input.GetAxis("Horizontal");
        var v = Input.GetAxis("Vertical");
        if(h == 0 && v == 0) return;
        var oldPos = GetParent<Unit>().Position;
        var targetPos = oldPos + math.normalize(new float3(h, 0, v));
        moveComponent.MoveTo(targetPos, 2);
    }
}