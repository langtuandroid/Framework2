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
        var type = typeof(LanguageDataFactory);
        print(type.IsSubclassOf(typeof(ConfigSingleton<>)));
        print(typeof(ConfigSingleton<>).IsAssignableFrom(type));
        print(IsSubclassOfRawGeneric(typeof(ConfigSingleton<>), type));
        print(type.IsSubclassOfGenericTypeDefinition(typeof(ConfigSingleton<>)));
        return;
        var q = typeof(BaseConfig).Assembly.GetTypes()
            .Where(x => !x.IsAbstract)
            .Where(x => !x.IsGenericTypeDefinition)
            .Where(x => x.IsSubclassOf(typeof(ConfigSingleton<>))); 
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