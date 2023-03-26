using ET;
using Framework;

public class CurrentScenesComponentAwakeSystem : AwakeSystem<CurrentScenesComponent>
{
    protected override void Awake(CurrentScenesComponent self)
    {
    }
}

public static class CurrentScenesComponentSystems
{
    public static void ChangeScene(this CurrentScenesComponent self, SceneType type, object vale = null)
    {
        self.Scene = EntitySceneFactory.CreateScene(type);
        switch (type)
        {
            case SceneType.Root:
                break;
            case SceneType.Launch:
                self.Scene.AddComponent<LaunchSceneComponent>();
                break;
            case SceneType.Battle:
                break;
        }
    }
}