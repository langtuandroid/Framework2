using UnityEngine;

public class EntityRadiusVisualizer : MonoBehaviour
{
    public void SetRadius(float radius)
    {
        transform.localScale = Vector3.one * radius * 2.0f;
    }
}