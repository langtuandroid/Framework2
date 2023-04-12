using System;
using System.Collections;
using Framework;
using Sirenix.OdinInspector;
using UnityEngine;

public class Test : MonoBehaviour
{

    [Button]
    private async void Start()
    {
        Game.AddSingleton<TimerComponent>();
        Game.AddSingleton<ObjectPool>();
        print(11);
        await Load();
        print(2222222);
        await TimerComponent.Instance.WaitFrameAsync();
        print(333);
        ProgressResult<float> result = ProgressResult<float>.Create();
        StartCoroutine(Wait(result));
        await result;
        print(444);
    }

    private void Update()
    {
        TimerComponent.Instance.Update(Time.deltaTime);
    }

    IProgressResult<float> Load()
    {
        ProgressResult<float> result = ProgressResult<float>.Create();
        result.SetResult();
        return result;
    }

    IEnumerator Wait(IProgressPromise<float> result)
    {
        float timer = 1;
        while (timer > 0)
        {
            timer -= Time.deltaTime;
            yield return null;
        }

        result.SetResult();
    }
}
