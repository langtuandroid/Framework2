using Framework;
using UnityEngine;

public class GameObjectComponent: Entity, IDestroySystem
{
    public GameObject GameObject { get; set; }

    public void OnDestroy()
    {
        Object.Destroy(GameObject);
        GameObject = null;
    }
}