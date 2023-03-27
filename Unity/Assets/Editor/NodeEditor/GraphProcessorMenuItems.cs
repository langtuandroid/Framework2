using GraphProcessor;
using UnityEditor;

public class GraphProcessorMenuItems : NodeGraphProcessorMenuItems
{
    private static void CreateNodeCSharpScritpt()
    {
        CreateDefaultNodeCSharpScritpt();
    }

    private static void CreateNodeViewCSharpScritpt()
    {
        CreateDefaultNodeViewCSharpScritpt();
    }

    // To add your C# script creation with you own templates, use ProjectWindowUtil.CreateScriptAssetFromTemplateFile(templatePath, defaultFileName)
}