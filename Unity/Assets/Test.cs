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


    private void Start()
    {
    }
}