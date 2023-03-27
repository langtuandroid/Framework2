using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public class Test : MonoBehaviour
{

    [Button]
    private void Start()
    {
        int a = 1;
        Dictionary<int, int> dic = new Dictionary<int, int>();
        dic[1] = 1;
        dic[2] = 2;
    }
}