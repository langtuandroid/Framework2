using Framework;
using UnityEngine;

public class GameObjectComponent: Entity, IDestroySystem
{
    public GameObject GameObject { get; set; }

    public void OnDestroy(Entity o)
    {
        Object.Destroy(GameObject);
        GameObject = null;
    }
}