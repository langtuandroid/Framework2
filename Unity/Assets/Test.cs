using Box2DSharp.Dynamics;
using NPBehave;
using Sirenix.OdinInspector;
using UnityEngine;

public class Test : MonoBehaviour
{
    private World world;
    private Clock clock;
    private Root _root;
    [Button]
    void Start()
    {
        clock = new Clock();
        _root = new Root(new Sequence(new Action(() =>
        {
            print(11);
        })), clock, false);
        _root.Start();
    }

    private void Update()
    {
        int a = 1;
        print(_root.CurrentState);
        clock.Update(Time.deltaTime);
    }
}
