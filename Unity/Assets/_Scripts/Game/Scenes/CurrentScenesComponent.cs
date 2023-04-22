using Framework;

namespace ET
{
    public class CurrentScenesComponent : Entity
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
    }
}