using Framework;
using Unity.Mathematics;
using UnityEngine;

public class TowerPreview : Entity, IAwakeSystem<TowerData>, IRendererUpdateSystem
{
    private float sphereCastRadius = 1;

    private LayerMask ghostWorldPlacementMask = LayerMask.GetMask("Placement") | LayerMask.GetMask("Default");

    private LayerMask placementAreaMask = LayerMask.GetMask("Placement");

    private Material material;

    private Material invalidPositionMaterial;

    private IPlacementArea currentArea;
    private int2 m_GridPosition;

    private TowerData towerData;
    private float3 targetPos;

    private Vector3 moveVel;

    private bool validPos = false;
    private bool visible = true;
    private bool canPlace = false;

    private Renderer[] renderers;

    public bool CanPlace
    {
        get { return canPlace; }
    }

    public void Awake(TowerData data)
    {
        towerData = data;
        renderers = parent.GetComponent<GameObjectComponent>().GameObject.GetComponentsInChildren<Renderer>();
        material = ResComponent.Instance.LoadAsset<Material>("Assets/Res/LoadArt/Tower/Material/TowerGhost.mat");
        invalidPositionMaterial =
            ResComponent.Instance.LoadAsset<Material>("Assets/Res/LoadArt/Tower/Material/TowerGhostInvalid.mat");
    }

    public void Show()
    {
        canPlace = false;
        validPos = false;
        moveVel = Vector3.zero;
        SetVisiable(true);
    }

    public void RenderUpdate(float deltaTime)
    {
        Move(false);

        float3 currentPos = GetParent<Unit>().Position;

        if (math.lengthsq(currentPos - targetPos) > 0.01f)
        {
            currentPos = Vector3.SmoothDamp(currentPos, targetPos, ref moveVel, 0.1f);
            GetParent<Unit>().Position = currentPos;
        }
        else
        {
            moveVel = Vector3.zero;
        }

        if (Input.GetMouseButtonDown(0))
        {
            if (TryBuildTower())
            {
                parent.GetComponent<GameObjectComponent>().DestroyGameObject();
                parent.Dispose();
            }
        }
    }

    private void SetVisiable(bool value)
    {
        if (visible == value)
            return;

        if (!visible)
        {
            moveVel = Vector3.zero;
            validPos = false;
        }

        foreach (var item in renderers)
        {
            item.enabled = value;
        }

        visible = value;
    }

    private void Move(bool hideWhenInvalid = true)
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, float.MaxValue, placementAreaMask))
        {
            MoveWithRaycastHit(hit);
        }
        else
        {
            MoveOntoWorld(ray, hideWhenInvalid);
        }
    }

    private void MoveWithRaycastHit(RaycastHit raycast)
    {
        currentArea = raycast.collider.GetComponent<IPlacementArea>();

        if (currentArea == null)
        {
            Log.Error("There is not an IPlacementArea attached to the collider found on the m_PlacementAreaMask");
            return;
        }

        m_GridPosition = currentArea.WorldToGrid(raycast.point, towerData.Dimensions);
        TowerFitStatus fits = currentArea.Fits(m_GridPosition, towerData.Dimensions);

        SetVisiable(true);
        canPlace = fits == TowerFitStatus.Fits;
        if (canPlace)
            currentArea.TempUse(m_GridPosition, towerData.Dimensions);
        else
            currentArea.ClearTempUse();
        Move(currentArea.GridToWorld(m_GridPosition, towerData.Dimensions),
            currentArea.transform.rotation,
            canPlace);
    }

    private void MoveOntoWorld(Ray ray, bool hideWhenInvalid)
    {
        currentArea?.ClearTempUse();
        currentArea = null;

        if (!hideWhenInvalid)
        {
            RaycastHit hit;
            // check against all layers that the ghost can be on
            Physics.SphereCast(ray, sphereCastRadius, out hit, float.MaxValue, ghostWorldPlacementMask);
            if (hit.collider == null)
            {
                return;
            }

            SetVisiable(true);
            Move(hit.point, hit.collider.transform.rotation, false);
        }
        else
        {
            SetVisiable(false);
        }
    }

    private void Move(Vector3 worldPosition, Quaternion rotation, bool validLocation)
    {
        targetPos = worldPosition;

        if (!validPos)
        {
            // Immediately move to the given position
            validPos = true;
            GetParent<Unit>().Position = targetPos;
        }

        GetParent<Unit>().Rotation = rotation;
        foreach (MeshRenderer meshRenderer in renderers)
        {
            meshRenderer.sharedMaterial = validLocation ? material : invalidPositionMaterial;
        }
    }

    public bool TryBuildTower()
    {
        if (currentArea == null)
        {
            Log.Error("Current area is null");
            return false;
        }

        Vector3 position = Vector3.zero;
        Quaternion rotation = currentArea.transform.rotation;

        TowerFitStatus fits = currentArea.Fits(m_GridPosition, towerData.Dimensions);

        if (fits == TowerFitStatus.Fits)
        {
            position = currentArea.GridToWorld(m_GridPosition, towerData.Dimensions);
            currentArea.Use(m_GridPosition, towerData.Dimensions);
            EventSystem.Instance.Publish(this.DomainScene(),
                new TowerCreateData() { Pos = position, Rotation = rotation, TowerData = towerData });
            return true;
        }

        return false;
    }
}