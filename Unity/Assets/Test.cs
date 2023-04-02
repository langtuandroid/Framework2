using System;
using System.Collections.Generic;
using System.Linq;
using Framework;
using Sirenix.OdinInspector;
using UnityEngine;

public class Test : MonoBehaviour
{

    [Button]
    private void Start()
    {
    }
    
    static bool IsSubclassOfRawGeneric(Type generic, Type toCheck) {
        while (toCheck != null && toCheck != typeof(object)) {
            var cur = toCheck.IsGenericType ? toCheck.GetGenericTypeDefinition() : toCheck;
            if (generic == cur) {
                return true;
            }
            toCheck = toCheck.BaseType;
        }
        return false;
    }
}