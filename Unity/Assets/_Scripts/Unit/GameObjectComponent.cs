using Framework;
using UnityEngine;

public class GameObjectComponent: Entity, IDestroySystem
{
    public GameObject GameObject { get; set; }

    public Transform Find(string path)
    {
        return GameObject.transform.Find(path);
    }

    public void OnDestroy()
    {
        if (GameObject != null)
            Object.Destroy(GameObject);
        GameObject = null;
    }
}