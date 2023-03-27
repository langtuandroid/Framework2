using Framework;
using UnityEditor;

public static class EditorInstance
{
    [InitializeOnLoadMethod]
    private static void AddEditorInstance()
    {
        Game.AddSingleton<TimeInfo>();
        Game.AddSingleton<IdGenerator>();
    }
}