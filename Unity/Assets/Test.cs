using System;
using System.Collections;
using Framework;
using Sirenix.OdinInspector;
using UnityEngine;

public class Test : MonoBehaviour
{

    [Button]
    IEnumerator Start()
    {
        Game.AddSingleton<TimerComponent>();
        Game.AddSingleton<ObjectPool>();
        yield return new WaitForSeconds(0.5f);
        new Test2().AA();
    }

    private void Update()
    {
        //print($"update " + Time.frameCount);
        TimerComponent.Instance.Update(Time.deltaTime);
    }


}

public class Test2
{
    public async void AA()
    {
        Debug.Log(Time.frameCount);
        await Load();
        Debug.Log(Time.frameCount);
        await TimerComponent.Instance.WaitFrameAsync();
        Debug.Log(Time.frameCount);
        ProgressResult<float> result = ProgressResult<float>.Create();
        Executors.RunOnCoroutineNoReturn(Wait(result));
        await result;
        Debug.Log(Time.frameCount); 
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
