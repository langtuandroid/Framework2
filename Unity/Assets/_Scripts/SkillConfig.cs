using System.Collections.Generic;
using Framework;
using Sirenix.OdinInspector;

public class SkillConfig : SerializedScriptableObject
{
    public string Name;
    public SerializeDictionary<int, float> Cd;

    public static SkillConfig GetById(int id)
    {
        return null;
    }
}