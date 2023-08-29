﻿using Unity.Mathematics;
using UnityEngine;

/// <summary>
/// An area suitable for placing a single tower
/// </summary>
[RequireComponent(typeof(Collider))]
public class SingleTowerPlacementArea : MonoBehaviour, IPlacementArea
{
    /// <summary>
    /// Visualisation prefab to instantiate
    /// </summary>
    public PlacementTile placementTilePrefab;

    /// <summary>
    /// <see cref="PlacementTile"/> we've spawned on our spot
    /// </summary>
    PlacementTile m_SpawnedTile;

    /// <summary>
    /// If the area is occupied
    /// </summary>
    bool m_IsOccupied;

    /// <summary>
    /// Set up visualisation tile
    /// </summary>
    protected void Awake()
    {
        PlacementTile tileToUse;

        tileToUse = placementTilePrefab;

        if (tileToUse != null)
        {
            m_SpawnedTile = Instantiate(tileToUse);
            m_SpawnedTile.transform.SetParent(transform);
            m_SpawnedTile.transform.localPosition = new Vector3(0f, 0.05f, 0f);
        }
    }

    /// <summary>
    /// Returns (0, 0), as there is only one available spot
    /// </summary>
    /// <param name="worldPosition"><see cref="Vector3"/> indicating world space coordinates to convert.</param>
    /// <param name="sizeOffset"><see cref="int2"/> indicating size of object to center.</param>
    public int2 WorldToGrid(Vector3 worldPosition, int2 sizeOffset)
    {
        return new int2(0, 0);
    }

    /// <summary>
    /// Returns transform.position, as there is only one available spot
    /// </summary>
    /// <param name="gridPosition">The coordinate in grid space</param>
    /// <param name="sizeOffset"><see cref="int2"/> indicating size of object to center.</param>
    public Vector3 GridToWorld(int2 gridPosition, int2 sizeOffset)
    {
        return transform.position;
    }

    /// <summary>
    /// Tests whether the placement area is valid.
    /// </summary>
    /// <param name="gridPos">The grid location</param>
    /// <param name="size">The size of the item</param>
    public TowerFitStatus Fits(int2 gridPos, int2 size)
    {
        return m_IsOccupied ? TowerFitStatus.Overlaps : TowerFitStatus.Fits;
    }

    /// <summary>
    /// Occupies the area
    /// </summary>
    /// <param name="gridPos"></param>
    /// <param name="size"></param>
    public void Use(int2 gridPos, int2 size)
    {
        m_IsOccupied = true;

        if (m_SpawnedTile != null)
        {
            m_SpawnedTile.SetState(PlacementTileState.Filled);
        }
    }

    private bool isTempUse = false;

    public void TempUse(int2 gridPos, int2 size)
    {
        if (isTempUse) return;
        isTempUse = true;
        if (m_SpawnedTile != null)
        {
            m_SpawnedTile.SetState(PlacementTileState.Filled);
        }
    }

    public void ClearTempUse()
    {
        isTempUse = false;
        Clear(default, default);
    }

    /// <summary>
    /// Clears the area
    /// </summary>
    /// <param name="gridPos"></param>
    /// <param name="size"></param>
    public void Clear(int2 gridPos, int2 size)
    {
        m_IsOccupied = false;

        if (m_SpawnedTile != null)
        {
            m_SpawnedTile.SetState(PlacementTileState.Empty);
        }
    }

#if UNITY_EDITOR
    /// <summary>
    /// Draw the spot as a smalls phere in the scene view.
    /// </summary>
    void OnDrawGizmos()
    {
        Color prevCol = Gizmos.color;
        Gizmos.color = Color.cyan;

        Matrix4x4 originalMatrix = Gizmos.matrix;
        Gizmos.matrix = transform.localToWorldMatrix;

        Gizmos.DrawWireSphere(Vector3.zero, 1);

        Gizmos.matrix = originalMatrix;
        Gizmos.color = prevCol;

        // Draw icon too
        Gizmos.DrawIcon(transform.position + Vector3.up, "build_zone.png", true);
    }
#endif
}