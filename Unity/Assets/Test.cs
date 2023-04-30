using System;
using System.Collections;
using Framework;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityExtensions.Tween;

public class Test : MonoBehaviour
{

    [Button]
    async void Start()
    {
        print(11);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            GetComponent<TweenPlayer>().Play();
        }
    }

}