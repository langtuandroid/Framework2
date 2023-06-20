using Box2DSharp.Dynamics;
using NPBehave;
using Sirenix.OdinInspector;
using UnityEngine;

public class Test : MonoBehaviour
{
    private World world;
    private Clock clock;
    [Button]
    void Start()
    {
    }

    private void Update()
    {
        clock.Update(Time.deltaTime);
    }
}
