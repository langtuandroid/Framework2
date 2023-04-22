using ET;
using Framework;

public class LaunchSceneComponent : Entity, IAwakeSystem
{
    public void Awake()
    {
        var currentScene = Root.Instance.Scene.GetComponent<CurrentScenesComponent>();
        currentScene.ChangeScene(SceneType.Battle);
    }
}