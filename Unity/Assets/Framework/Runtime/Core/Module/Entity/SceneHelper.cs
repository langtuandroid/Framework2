namespace Framework
{
    public static class SceneHelper
    {
        public static Scene DomainScene(this Entity entity)
        {
            return (Scene)entity.Domain;
        }
        
        public static Scene RootScene(this Entity entity)
        {
            return Root.Instance.Scene;
        }
    }
}