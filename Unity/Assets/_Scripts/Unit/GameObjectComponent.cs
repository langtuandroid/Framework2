using Framework;
using UnityEngine;

public class GameObjectComponent : Entity, IDestroySystem, IAwakeSystem<bool, bool>, IRendererUpdateSystem
{
    private GameObject gameObject;

    public GameObject GameObject
    {
        get => gameObject;
        set
        {
            if (gameObject == value)
            {
                return;
            }

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

    private bool updatePosToUnit;
    private bool updatePosFromUnit;

    public void Awake(bool updatePosToUnit, bool updatePosFromUnit)
    {
        this.updatePosFromUnit = updatePosFromUnit;
        this.updatePosToUnit = updatePosToUnit;
    }

    public Transform Find(string path)
    {
        if (GameObject == null)
        {
            return null;
        }

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
        if (gameObject == null)
        {
            return;
        }

        GoConnectedUnitId goConnectedId = gameObject.GetComponent<GoConnectedUnitId>();
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

    public void RenderUpdate(float deltaTime)
    {
        if (gameObject == null)
        {
            return;
        }

        if (updatePosToUnit)
        {
            GetParent<Unit>().Position = gameObject.transform.position;
        }

        if (updatePosFromUnit)
        {
            gameObject.transform.position = GetParent<Unit>().Position;
        }
    }
}