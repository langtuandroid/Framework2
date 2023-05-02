#if UNITY_EDITOR

using System.Collections.Generic;
using UnityEngine;

public class B2D_DebuggerHandler : MonoBehaviour
{
    public List<B2D_ColliderVisualHelperBase> MB2DColliderVisualHelpers = new List<B2D_ColliderVisualHelperBase>();

    private void OnDrawGizmos()
    {
        foreach (var VARIABLE in this.MB2DColliderVisualHelpers)
        {
            if (VARIABLE.canDraw)
                VARIABLE.OnDrawGizmos();
        }
    }

    public void CleanCollider()
    {
        MB2DColliderVisualHelpers.Clear();
    }

    public void OnUpdate()
    {
        foreach (var VARIABLE in this.MB2DColliderVisualHelpers)
        {
            VARIABLE.OnUpdate();
        }
    }
}

#endif