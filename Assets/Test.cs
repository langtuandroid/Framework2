using System;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;

public class Test : MonoBehaviour
{
    [Button]
    private void Start()
    {
        UnityAction action = default;
        print(action);
    }
}