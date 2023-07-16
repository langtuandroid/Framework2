using Framework;
using UnityEngine;

public class GameObjectComponent: Entity, IDestroySystem, IAwakeSystem<GameObject>
{
    private GameObject gameObject;

    public GameObject GameObject
    {
        get => gameObject;
        set
        {
            if(gameObject == value) return;
            if (gameObject != null)
            {
                gameObject.GetOrAddComponent<GoConnectedUnitId>().SetUnitId(0);
#if UNITY_EDITOR
                gameObject.GetOrAddComponent<EditorVisibleUnit>().SetUnit(null);
#endif
            }

            gameObject = value;
            if (gameObject != null)
            {
                gameObject.GetOrAddComponent<GoConnectedUnitId>().SetUnitId(parent.Id);
#if UNITY_EDITOR
                gameObject.GetOrAddComponent<EditorVisibleUnit>().SetUnit(parent as Unit);
#endif
            }

        }
    }
    
    public void Awake(GameObject a)
    {
        
    }

    public Transform Find(string path)
    {
        if (GameObject == null) return null;
        if (string.IsNullOrEmpty(path))
        {
            return GameObject.transform;
        }
        return GameObject.transform.Find(path);
    }

    public void DestroyGameObject()
    {
        if (GameObject != null)
        {
            Object.Destroy(gameObject);
        }
        OnDestroy();
    }

    public void OnDestroy()
    {
        if (gameObject == null) return;
        var goConnectedId = gameObject.GetComponent<GoConnectedUnitId>();
        if (goConnectedId != null)
        {
            if (goConnectedId.UnitId == parent.Id)
            {
                goConnectedId.SetUnitId(0);
            }
        }
#if UNITY_EDITOR
        gameObject.GetComponent<EditorVisibleUnit>()?.SetUnit(null);
#endif
        GameObject = null;
    }
}