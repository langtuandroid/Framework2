namespace Framework
{
    public static class EntitySceneFactory
    {
        public static Scene CreateScene(int zone, SceneType sceneType, string name, Entity parent = null)
        {
            long instanceId = IdGenerator.Instance.GenerateInstanceId();
            Scene scene = new Scene(zone, instanceId, zone, sceneType, name, parent);
            return scene;
        }
    }
}