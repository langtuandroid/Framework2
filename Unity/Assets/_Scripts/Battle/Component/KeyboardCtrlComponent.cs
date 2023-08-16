using Framework;
using Unity.Mathematics;
using UnityEngine;

public class KeyboardCtrlComponent : Entity, IAwakeSystem, IUpdateSystem
{
    private MoveComponent moveComponent;

    public void Awake()
    {
        moveComponent = parent.GetComponent<MoveComponent>();
    }


    public void Update(float deltaTime)
    {
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");
        if (h == 0 && v == 0)
        {
            moveComponent.Stop(true);
            return;
        }

        float3 oldPos = GetParent<Unit>().Position;
        float3 targetPos = oldPos + math.normalize(new float3(h, 0, v) * 0.5f);
        moveComponent.MoveTo(targetPos, 2);
    }
}