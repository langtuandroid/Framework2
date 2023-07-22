using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using Framework;
using NPBehave;
using Sirenix.OdinInspector;
using UnityEngine;
using Action = NPBehave.Action;
using IAsyncResult = Framework.IAsyncResult;
using Parallel = NPBehave.Parallel;
using Root = NPBehave.Root;

public class Test : MonoBehaviour
{
    private Clock clock;
    private Blackboard blackboard;
    public Root root;

    [Button]
    private async void Start()
    {
        Game.AddSingleton<ObjectPool>();
        Game.AddSingleton<TimerComponent>();
        clock = new Clock();
        blackboard = new Blackboard(clock);
        var action1 = new Action(() => Debug.Log("foo"), string.Empty);
        var action2 = new Action(() => Debug.Log("bar"), string.Empty);
        var sequence1 = new Sequence(action1, new WaitUntilStopped());
        var sequence2 = new Sequence(action2, new WaitUntilStopped());
        var bbc = new BlackboardCondition("foo", Operator.IS_EQUAL, new NP_BBValue_Bool() { Value = true },
            Stops.IMMEDIATE_RESTART,
            action1
        );
        var selector = new Selector(bbc, sequence2);
        var service = new Service(0.5f, () => { blackboard.Set("foo", !blackboard.Get<bool>("foo"), true); }, selector);
        var behaviorTree = new Root(blackboard, clock, service);
        // behaviorTree.Start();
        root = new Root(blackboard, clock, new Parallel(Parallel.Policy.ALL, Parallel.Policy.ALL,
            bbc, new Sequence(
                new Wait(5), action2
            )
        ));
        root.Start();
    }

    private void Update()
    {
        clock.Update(Time.deltaTime);
        if (Input.GetKeyDown(KeyCode.Space)) blackboard.Set("foo", true, true);
    }

    private async IAsyncResult EE()
    {
    }
    
}