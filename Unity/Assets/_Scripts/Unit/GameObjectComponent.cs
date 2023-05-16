using Framework;
using UnityEngine;

public class GameObjectComponent: Entity, IDestroySystem
{
    public GameObject GameObject { get; set; }

    public Transform Find(string path)
    {
        if (GameObject == null) return null;
        if (string.IsNullOrEmpty(path))
        {
            return GameObject.transform;
        }
        return GameObject.transform.Find(path);
    }

    public void OnDestroy()
    {
        if (GameObject != null)
            Object.Destroy(GameObject);
        GameObject = null;
    }
}