﻿using Framework;

public class CurrentScenesComponent : Entity, IAwakeSystem
{
    public Scene Scene { get; set; }

    public void ChangeScene(SceneType type, object vale = null)
    {
        Scene = EntitySceneFactory.CreateScene(type);
        switch (type)
        {
            case SceneType.Root:
                break;
            case SceneType.Launch:
                Scene.AddComponent<LaunchSceneComponent>();
                break;
            case SceneType.Battle:
                Scene.AddComponent<BattleSceneComponent>();
                break;
        }
    }

    public void Awake()
    {
    }
}