using UnityEngine;

public class GoConnectedUnitId : MonoBehaviour
{
    public long UnitId { get; private set; }

    public void SetUnitId(long unitId)
    {
        UnitId = unitId;
    }
}