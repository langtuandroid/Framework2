using System;
using System.Collections.Generic;
using Framework;
using UnityEngine;

public class Init : MonoBehaviour
{
    private async void Start()
    {
        DontDestroyOnLoad(gameObject);

        AppDomain.CurrentDomain.UnhandledException += (sender, e) => { Log.Error(e.ExceptionObject.ToString()); };

        Game.AddSingleton<MainThreadSynchronizationContext>();

        Game.AddSingleton<TimeInfo>();
        Game.AddSingleton<ObjectPool>();
        Game.AddSingleton<IdGenerator>();
        Game.AddSingleton<EventSystem>();
        Game.AddSingleton<TimerComponent>();
        Game.AddSingleton<CoroutineLockComponent>();
        Game.AddSingleton<Root>();
        Game.AddSingleton<ResComponent>();
        Game.AddSingleton<ConfigComponent>();

        EventSystem.Instance.Add(AssemblyHelper.GetAssemblyTypes(typeof(Game).Assembly));
        EventSystem.Instance.Add(AssemblyHelper.GetAssemblyTypes(typeof(Init).Assembly));
        EventSystem.Instance.InitType();
        await ResComponent.Instance.Init();

        ConfigComponent.Instance.Load();

        ETTask.ExceptionHandler += e => Log.Error(e);

    }

    /// <summary>
    /// 按每秒15帧的频率更新逻辑帧
    /// </summary>
    private float logicUpdateInterval = 1000 / 15f;
    private float logicUpdateTimer = 1000 / 15f;
    private void Update()
    {
        logicUpdateTimer += Time.deltaTime;
        if (logicUpdateTimer >= logicUpdateInterval)
        {
            Game.Update(logicUpdateTimer);
            logicUpdateTimer = 0;
        }

        Game.RendererUpdate(Time.deltaTime);
    }

    private void LateUpdate()
    {
        Game.LateUpdate(Time.deltaTime);
        Game.FrameFinishUpdate();
    }

    private void OnApplicationQuit()
    {
        Game.Close();
    }
}